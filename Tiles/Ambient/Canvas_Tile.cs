using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient
{
	public class Canvas_Tile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16
			};
			TileObjectData.newTile.AnchorBottom = default(AnchorData);
			TileObjectData.newTile.AnchorTop = default(AnchorData);
			TileObjectData.newTile.AnchorWall = true;
			TileObjectData.addTile(Type);
			dustType = -1;
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Canvas");
			AddMapEntry(new Color(0, 80, 252), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int item = ModContent.ItemType<Canvas>();
			Item.NewItem(i * 16, j * 16, 48, 48, item);
		}
	}
}