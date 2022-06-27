using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles
{
	public class CrimsonPustuleTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorBottom = new AnchorData(Terraria.Enums.AnchorType.SolidTile, 2, 0);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<CrimsonPustuleTileEntity>().Hook_AfterPlacement, -1, 0, true);
			TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Crimson Pustule");
			AddMapEntry(new Color(242, 90, 60), name);

			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = DustID.Blood;
			HitSound = SoundID.NPCDeath12;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) => ModContent.GetInstance<CrimsonPustuleTileEntity>().Kill(i, j);

		public override bool IsTileDangerous(int i, int j, Player player) => true;

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			Point16 tileEntityPos = new Point16(i - tile.TileFrameX / 18 % 2, j - tile.TileFrameY / 18 % 2);

			var tileEntity = TileEntity.ByPosition[tileEntityPos] as CrimsonPustuleTileEntity;

			Color color = Main.LocalPlayer.dangerSense ? new Color(255, 50, 50, Main.mouseTextColor) : Lighting.GetColor(i, j);
			Vector2 offScreenRange = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
			Vector2 origin = new Vector2(tile.TileFrameX % 18 == 0 ? 18 : -18, 0);
			Vector2 drawPos = new Vector2(i * 16 + origin.X, j * 16) - Main.screenPosition + offScreenRange + Vector2.UnitY * 2;
			Texture2D tileTexture = TextureAssets.Tile[Type].Value;
			Texture2D flashTexture = Mod.Assets.Request<Texture2D>("Tiles/CrimsonPustuleTile_Flash").Value;
			float scale = 1f + tileEntity.Pulse * 0.08f;

			spriteBatch.Draw(tileTexture, drawPos, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), color, 0f, origin, scale, SpriteEffects.None, 0);
			spriteBatch.Draw(flashTexture, drawPos, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), color * tileEntity.Pulse, 0f, origin, scale, SpriteEffects.None, 0);
			return false;
		}

		public static bool Spawn(int i, int j)
		{
			bool success = WorldGen.PlaceTile(i, j, ModContent.TileType<CrimsonPustuleTile>(), style: Main.rand.Next(3));

			if (!success)
				return false;

			ModContent.GetInstance<CrimsonPustuleTileEntity>().Place(i, j);

			if (Main.netMode == NetmodeID.Server)
			{
				NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, ModContent.TileEntityType<CrimsonPustuleTileEntity>(), 0f, 0, 0, 0);
				NetMessage.SendTileSquare(-1, i, j, 2);
			}

			return true;
		}
	}

	public class CrimsonPustuleTileEntity : ModTileEntity
	{
		private const int StartingCountdown = 100;
		private const int Damage = 180; // self explanatory
		private const int Range = 80; // If a player enters this range, agitated becomes true. Entities within this range get hurt when the pustule explodes

		private bool agitated; // True if the pustule is exploding
		private int explodeCountdown = StartingCountdown; // Decreases if agitated is true. Pustule explodes when this hits zero

		public float Pulse => Math.Abs((float)Math.Sin(0.0018 * (StartingCountdown - explodeCountdown) * (StartingCountdown - explodeCountdown)));
		public Vector2 PustuleWorldCenter => Position.ToWorldCoordinates(16, 16);

		public override bool IsTileValidForEntity(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			return tile.HasTile && tile.TileType == ModContent.TileType<CrimsonPustuleTile>() && tile.TileFrameY % 36 == 0 && tile.TileFrameY == 0;
		}

		public int Hook_AfterPlacement(int i, int j, int type, int style, int direction) => Place(i, j);

		public override void Update()
		{
			int detectionRange = 3 * 16;

			if (!agitated)
			{
				foreach (Player player in Main.player)
				{
					if (Vector2.DistanceSquared(player.Center, PustuleWorldCenter) < detectionRange * detectionRange)
					{
						agitated = true;
						NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
						break;
					}
				}
			}

			if (agitated)
				explodeCountdown--;
		}

		public override void PreGlobalUpdate()
		{
			List<Point16> toBeDetonatedKeys = new List<Point16>();

			foreach (TileEntity tileEntity in ByID.Values)
				if (tileEntity is CrimsonPustuleTileEntity crimsonPustuleTileEntity)
					if (crimsonPustuleTileEntity.explodeCountdown <= 0)
						toBeDetonatedKeys.Add(tileEntity.Position);

			foreach (Point16 position in toBeDetonatedKeys)
				(ByPosition[position] as CrimsonPustuleTileEntity).Explode();
		}

		public override void NetSend(BinaryWriter writer) => writer.Write(agitated);
		public override void NetReceive(BinaryReader reader) => agitated = reader.ReadBoolean();

		// Detonates a pustule, damaging all players and npcs within its range, and killing the tile and tile entity
		public void Explode()
		{
			foreach (Player player in Main.player)
				if (player.active && !player.dead && player.DistanceSQ(PustuleWorldCenter) < Range * Range)
					player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " was grotesquely detonated"), Damage, player.Center.X > PustuleWorldCenter.X ? -1 : 1);

			foreach (NPC npc in Main.npc)
				if (npc.DistanceSQ(PustuleWorldCenter) < Range * Range)
					npc.StrikeNPC(Damage, 0f, npc.Center.X > PustuleWorldCenter.X ? -1 : 1);

			for (int i = 0; i < 30; i++)
				Dust.NewDustDirect(PustuleWorldCenter, 6, 6, DustID.Blood, Main.rand.NextFloat(3f, 6f) * Main.rand.NextFloatDirection(), Main.rand.NextFloat(3f, 6f) * Main.rand.NextFloatDirection(), Scale: Main.rand.NextFloat(1f, 2f));

			for (int i = 1; i <= 4; i++)
				Gore.NewGore(PustuleWorldCenter, Main.rand.NextVector2Unit() * Main.rand.NextFloat(3f, 6f), mod.GetGoreSlot("Gores/CrimsonPustule/CrimsonPustule" + i));

			WorldGen.KillTile(Position.X, Position.Y);
			Kill(Position.X, Position.Y);
		}
	}

	public class CrimsonPustuleGlobalTile : GlobalTile
	{
		// Will try to grow a crimson pustule on top of a crimstone
		public override void RandomUpdate(int i, int j, int type)
		{
			Tile tile = Framing.GetTileSafely(i, j);

			if (MyWorld.CrimHazards >= 20) //There shouldn't be too many in the world
				return;

			if (tile.TileType != TileID.Crimstone) //I should be crimstone
				return;

			Tile rightTile = Framing.GetTileSafely(i + 1, j);
			if (!rightTile.HasTile || rightTile.TileType != TileID.Crimstone) //And my friend should be too
				return;

			for (int x = i; x <= i + 1; x++) //And we should have space to breathe
				for (int y = j - 2; y <= j - 1; y++)
					if (Framing.GetTileSafely(x, y).HasTile)
						return;

			if (Main.rand.NextBool(120))
			{ // TODO: maybe config for this random chance?
				WorldGen.PlaceTile(i, j, TileID.Crimstone, forced: true);
				WorldGen.PlaceTile(i + 1, j, TileID.Crimstone, forced: true);
				CrimsonPustuleTile.Spawn(i, j - 2);
			}
		}
	}
}