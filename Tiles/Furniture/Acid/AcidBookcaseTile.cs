using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture.Acid
{
	public class AcidBookcaseTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16,
				16
			};
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
            name.SetDefault("Corrosive Bookcase");
            AddMapEntry(new Color(100, 122, 111), name);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            dustType = -1;
            disableSmartCursor = true;
			adjTiles = new int[] { TileID.Bookcases };
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Terraria.Item.NewItem(i * 16, j * 16, 16, 32, ModContent.ItemType<Items.Placeable.Furniture.Acid.AcidBookcase>());
		}
	}
}