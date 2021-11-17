using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable;
using SpiritMod.Particles;
using SpiritMod.Tiles.Block;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles
{
	public class SuperSunFlower : ModTile
	{
		public static readonly int Range = 5; // Used in the detours

		public override void SetDefaults()
		{
			Main.tileSolid[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLavaDeath[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileFrameImportant[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.Height = 6;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 6 };
			TileObjectData.newTile.DrawYOffset = 12;
			TileObjectData.newTile.Origin = new Point16(1, 5);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorValidTiles = new int[] { TileID.Grass, TileID.Dirt, TileID.JungleGrass, TileID.Mud, ModContent.TileType<BriarGrass>() };
			TileObjectData.addTile(Type);

			AddMapEntry(Color.LightGoldenrodYellow);

			dustType = DustID.Grass;
		}

		public override void PlaceInWorld(int i, int j, Item item)
		{
			MyWorld.superSunFlowerPositions.Add(new Point16(i, j));

			if (Main.netMode == NetmodeID.MultiplayerClient)
				SpiritMod.WriteToPacket(SpiritMod.Instance.GetPacket(4), (byte)MessageType.PlaceSuperSunFlower, (ushort)i, (ushort)j).Send();
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 0, 0, ModContent.ItemType<SuperSunFlowerItem>());

			// no need to sync this back to clients
			MyWorld.superSunFlowerPositions.Remove(new Point16(i + 1, j + 5));
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Framing.GetTileSafely(i, j);

			b = 0.3f;
			r = g = (float)Math.Sin(Main.GlobalTime) * 0.2f + 0.8f;

			if (tile.frameY > 54 || Main.dayTime)
				return;

			if (Main.rand.NextBool(180) && !Main.gamePaused) {
				GlowParticle particle = new GlowParticle(
					new Vector2(i, j).ToWorldCoordinates() + new Vector2(40f * (Main.rand.NextBool() ? 1 : -1), 30f * (Main.rand.NextBool() ? 1 : -1)),
					Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.1f, 0.4f),
					new Color(0.7f, 0.7f, 0.15f),
					Main.rand.NextFloat(0.03f, 0.06f),
					Main.rand.Next(180, 400));

				ParticleHandler.SpawnParticle(particle);
			}
		}
	}
}
