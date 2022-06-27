using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Furniture;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient
{
	public class ProbePrint : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			DustType = 7;
			TileID.Sets.DisableSmartCursor[Type] = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Blueprint");
			AddMapEntry(new Color(0, 80, 252), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int item = 0;
			switch (frameX / 54) {
				case 0:
					item = ModContent.ItemType<PrintProbe>();
					break;
				case 1:
					item = ModContent.ItemType<PrintProbe>();
					break;
				case 2:
					item = ModContent.ItemType<PrintProbe>();
					break;
				case 3:
					item = ModContent.ItemType<PrintProbe>();
					break;
			}
			if (item > 0) {
				Item.NewItem(i * 16, j * 16, 48, 48, item);
			}
		}
	}
}