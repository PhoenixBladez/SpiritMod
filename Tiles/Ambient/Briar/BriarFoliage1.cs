using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.Briar
{
	public class BriarFoliage1 : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileMergeDirt[Type] = true;

			dustType = 5;
			soundType = SoundID.Grass;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.addTile(Type);
            /*
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.WaterDeath = false;

			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16
			};
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.Style = 0;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.UsesCustomCanPlace = true;

			for (int i = 0; i < 8; i++)
			{
				TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
				TileObjectData.addSubTile(i);
			}

			TileObjectData.addTile(Type);*/

            AddMapEntry(new Color(100, 150, 66));
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 2;
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			Tile tileBelow = Framing.GetTileSafely(i, j + 2);
			if (!tileBelow.active() || tileBelow.halfBrick() || tileBelow.topSlope()) {
				WorldGen.KillTile(i, j);
			}

			return true;
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Framing.GetTileSafely(i, j);

			if (tile.frameX >= 108) {
				Color colour = Color.White * MathHelper.Lerp(0.2f, 1f, (float)((Math.Sin(SpiritMod.GlobalNoise.Noise(i * 0.2f, j * 0.2f) * 3f + Main.GlobalTime * 1.3f) + 1f) * 0.5f));

				Texture2D glow = ModContent.GetTexture("SpiritMod/Tiles/Ambient/Briar/BriarFoliage1Glow");
				Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
				spriteBatch.Draw(glow, new Vector2(i * 16, j * 16 + 2) - Main.screenPosition + zero, new Rectangle(tile.frameX - 108, 0, 16, 16), colour);
			}
		}
	}
}