
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.Briar
{
	public class Blaststone_Grave : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
			16,
			16
			};
			TileObjectData.addTile(Type);
            dustType = DustID.Stone;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Gravestone");
			AddMapEntry(new Color(107, 90, 64), name);
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
		}
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            Tile tileBelow = Framing.GetTileSafely(i, j + 2);
            if (!tileBelow.active() || tileBelow.halfBrick() || tileBelow.topSlope())
            {
                WorldGen.KillTile(i, j);
            }

            return true;
        }
    }
}