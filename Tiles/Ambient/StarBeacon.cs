using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Placeable;
using SpiritMod.NPCs.Boss.SteamRaider;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient
{
	public class StarBeacon : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.Origin = new Point16(0, 2);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Astralite Beacon");
			AddMapEntry(new Color(50, 70, 150), name);
			disableSmartCursor = true;
			dustType = DustID.Electric;
		}

		float alphaCounter = 0;

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = .12f;
			g = .3f;
			b = 0.5f;

		}

		public override bool CanExplode(int i, int j) => false;

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			TileUtilities.BlockActuators(i, j);
			return base.TileFrame(i, j, ref resetFrame, ref noBreak);
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			alphaCounter += 0.04f;
			float sineAdd = (float)Math.Sin(alphaCounter) + 3;
			Tile tile = Main.tile[i, j];
			var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen) {
				zero = Vector2.Zero;
			}
			int height = tile.frameY == 36 ? 18 : 16;
			Main.spriteBatch.Draw(mod.GetTexture("Tiles/Ambient/StarBeacon_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(SpiritMod.Instance.GetTexture("Effects/Masks/Extra_49"), new Vector2(i * 16 - (int)Main.screenPosition.X + 8, j * 16 - (int)Main.screenPosition.Y + 16) + zero, null, new Color((int)(2.5f * sineAdd), (int)(5f * sineAdd), (int)(6f * sineAdd), 0), 0f, new Vector2(50, 50), 0.2f * (sineAdd + 1), SpriteEffects.None, 0f);
		}

		public override bool CanKillTile(int i, int j, ref bool blockDamaged) => MyWorld.downedRaider;
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height) => offsetY = 2;
		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 64, 48, ModContent.ItemType<StarBeaconItem>());
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
		}

		public override void MouseOver(int i, int j)
		{
			//shows the Cryptic Crystal icon while mousing over this tile
			Main.player[Main.myPlayer].showItemIcon = true;
			Main.player[Main.myPlayer].showItemIcon2 = ModContent.ItemType<StarWormSummon>();
		}

		public override bool NewRightClick(int i, int j)
		{
			for (int k = 0; k < Main.npc.Length; k++)
				if (Main.npc[k].active && Main.npc[k].type == ModContent.NPCType<SteamRaiderHead>()) return false;

			if (Mechanics.EventSystem.EventManager.IsPlaying<Mechanics.EventSystem.Events.StarplateBeaconIntroEvent>())
				return false;

			Player player = Main.player[Main.myPlayer];
			if (player.HasItem(ModContent.ItemType<StarWormSummon>()))
			{
				int x = i;
				int y = j;
				while (Main.tile[x, y].type == Type) x--;
				x++;
				while (Main.tile[x, y].type == Type) y--;
				y++;

				Mechanics.EventSystem.EventManager.PlayEvent(new Mechanics.EventSystem.Events.StarplateBeaconIntroEvent(new Vector2(x * 16f + 16f, y * 16f + 12f)));
			}
			return false;
		}
	}
}