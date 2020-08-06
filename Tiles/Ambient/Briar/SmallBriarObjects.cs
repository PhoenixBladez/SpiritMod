using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.Briar
{
	public class SmallBriarObjects : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileMergeDirt[Type] = true;
            TileObjectData.newTile.RandomStyleRange = 10;
            dustType = 1;
			soundType = SoundID.Grass;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
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

            AddMapEntry(new Color(105, 89, 74));
		}
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            Tile tileBelow = Framing.GetTileSafely(i, j + 1);
            if (!tileBelow.active() || tileBelow.halfBrick() || tileBelow.topSlope())
            {
                WorldGen.KillTile(i, j);
            }

            return true;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 2;
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
		}
	}
}