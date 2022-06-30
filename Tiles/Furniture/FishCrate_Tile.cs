using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture
{
	public class FishCrate_Tile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileTable[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Crate");
			AddMapEntry(new Color(150, 150, 150), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16, 64, 32, ModContent.ItemType<Items.Placeable.FishCrate>());
		}
	}
}