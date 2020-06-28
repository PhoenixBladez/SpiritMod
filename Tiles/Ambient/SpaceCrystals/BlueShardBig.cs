using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.SpaceCrystals
{
	public class BlueShardBig : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Width = 2;
			Main.tileLighted[Type] = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
			16,
			16
			};
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Giant Crystal");
			dustType = 180;
			AddMapEntry(new Color(200, 200, 200), name);
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.3f / 4;
			g = 0.3f / 4;
			b = 0.9f / 4;
		}
	}
}
