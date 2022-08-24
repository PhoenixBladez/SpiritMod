using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ModLoader;
using System;
using TileID = Terraria.ID.TileID;
using Terraria.ID;

namespace SpiritMod.Tiles.Block
{
	public class SpiritGrass : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<SpiritDirt>()] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;

			Main.tileMerge[Type][TileID.Grass] = true;
			Main.tileMerge[TileID.Grass][Type] = true;

			TileID.Sets.Grass[Type] = true;
			TileID.Sets.Conversion.Grass[Type] = true;

			AddMapEntry(new Color(0, 191, 255));
			DustType = DustID.Flare_Blue;
			ItemDrop = ModContent.ItemType<SpiritDirtItem>();
		}

		public static bool PlaceObject(int x, int y, int type, bool mute = false, int style = 0, int alternate = 0, int random = -1, int direction = -1)
		{
			if (!TileObject.CanPlace(x, y, type, style, direction, out TileObject toBePlaced, false))
				return false;
			toBePlaced.random = random;
			if (TileObject.Place(toBePlaced) && !mute)
				WorldGen.SquareTileFrame(x, y, true);
			return false;
		}

		public override void RandomUpdate(int i, int j)
		{
			if (!Framing.GetTileSafely(i, j - 1).HasTile && Main.rand.NextBool(40))
			{
				int rand = Main.rand.Next(1, 6);
				PlaceObject(i, j - 1, Mod.Find<ModTile>("SpiritGrassA" + rand).Type);
				NetMessage.SendObjectPlacment(-1, i, j - 1, Mod.Find<ModTile>("SpiritGrassA" + rand).Type, 0, 0, -1, -1);
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile2 = Framing.GetTileSafely(i, j - 1);
			if (!Main.tileSolid[tile2.TileType] || !tile2.HasTile)
			{
				Color colour = Color.White * MathHelper.Lerp(0.2f, 1f, (float)((Math.Sin(SpiritMod.GlobalNoise.Noise(i * 0.2f, j * 0.2f) * 3f + Main.GlobalTimeWrappedHourly * 1.3f) + 1f) * 0.5f));
				Texture2D glow = ModContent.Request<Texture2D>("SpiritMod/Tiles/Block/SpiritGrass_Glow", ReLogic.Content.AssetRequestMode.AsyncLoad).Value;

				GTile.DrawSlopedGlowMask(i, j, glow, colour, Vector2.Zero);
			}
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile2 = Framing.GetTileSafely(i, j - 1);
			if (!Main.tileSolid[tile2.TileType] || !tile2.HasTile)
			{
				r = 0.3f;
				g = 0.45f;
				b = 1.05f;
			}
		}
	}
}