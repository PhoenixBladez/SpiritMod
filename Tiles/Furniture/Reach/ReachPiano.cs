using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture.Reach
{
	public class ReachPiano : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Elderbark Piano");
			AddMapEntry(new Color(60, 244, 55), name);
			disableSmartCursor = true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            offsetY = 2;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Terraria.Item.NewItem(i * 16, j * 16, 32, 16, ModContent.ItemType<ReachPiano>());
		}
	}
}