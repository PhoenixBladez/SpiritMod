
using Microsoft.Xna.Framework;
using SpiritMod.Items.Tool;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture
{
	public class GemsPickaxeSapphire : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileLighted[Type] = true;

			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Lodged Pickaxe");
			AddMapEntry(Color.Blue, name);
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = .048f;
			g = .099f;
			b = .122f;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			if(Main.rand.Next(2) == 0) {
				Item.NewItem(i * 16, j * 16, 64, 32, ModContent.ItemType<GemPickaxe>());
			}
			for(int k = 0; k < 3; k++) {
				Item.NewItem(i * 16, j * 16, 64, 32, ItemID.Sapphire);
			}
		}
	}
}
