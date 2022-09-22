using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture.Reach
{
	public class TreemanStatue : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileMergeDirt[Type] = true;

			TileObjectData.newTile.Width = 6;
			TileObjectData.newTile.Height = 9;
			TileObjectData.newTile.Origin = new Point16(3, 6);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.addTile(Type);

			AdjTiles = new int[] { TileID.WorkBenches, TileID.Bookcases, TileID.Bottles };
			DustType = DustID.Asphalt;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Statue of the Old Gods");
			AddMapEntry(new Color(179, 146, 107), name);
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new EntitySource_TileBreak(i, j), (i + 3) * 16, (j + 5) * 16, 16, 32, ModContent.ItemType<Items.Placeable.Furniture.Reach.TreemanStatue>());
	}
}