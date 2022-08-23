using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Tiles.Ambient;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	internal class Stargrass : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<SpiritDirt>()] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;

			Main.tileMerge[Type][TileID.Dirt] = true;
			Main.tileMerge[TileID.Dirt][Type] = true;

			TileID.Sets.Grass[Type] = true;
			TileID.Sets.Conversion.Grass[Type] = true;

			AddMapEntry(new Color(0, 191, 255));
			DustType = DustID.Flare_Blue;
			ItemDrop = ItemID.DirtBlock;
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
			if (!Framing.GetTileSafely(i, j - 1).HasTile && Main.rand.NextBool(4))
			{
				int style = Main.rand.Next(12);
				PlaceObject(i, j - 1, ModContent.TileType<StargrassFlowers>(), true, style);
				NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<StargrassFlowers>(), style, 0, -1, -1);
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			var tex = ModContent.Request<Texture2D>(Texture + "_Glow", ReLogic.Content.AssetRequestMode.AsyncLoad).Value;
			Color colour = Color.White * MathHelper.Lerp(0.2f, 1f, (float)((Math.Sin(SpiritMod.GlobalNoise.Noise(i * 0.2f, j * 0.2f) * 3f + Main.GlobalTimeWrappedHourly * 1.3f) + 1f) * 0.5f));
			GTile.DrawSlopedGlowMask(i, j, tex, colour, Vector2.Zero, false);
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) => (r, g, b) = (0.05f, 0.2f, 0.5f);
	}
}
