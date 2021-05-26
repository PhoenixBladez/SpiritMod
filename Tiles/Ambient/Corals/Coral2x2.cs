using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.Corals
{
	public class Coral2x2 : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorValidTiles = new int[] { TileID.Sand, TileID.Crimsand, TileID.Ebonsand };
			TileObjectData.newTile.RandomStyleRange = 3;
			TileObjectData.addTile(Type);

			disableSmartCursor = true;
			dustType = 225;// DustID.Coralstone;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Coral");
			AddMapEntry(new Color(87, 61, 51), name);
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 48, ItemID.Coral, Main.rand.Next(3, 6));
			//if (frameX < 36) //French fry coral
			//	Item.NewItem(i * 16, j * 16, 32, 48, ModContent.ItemType<Items.Material.Canvas>());
			//else if (frameX < 72) //Blue tabletop coral
			//	Item.NewItem(i * 16, j * 16, 32, 48, ModContent.ItemType<Items.Material.Canvas>());
			//else //Brain coral
			//	Item.NewItem(i * 16, j * 16, 32, 48, ModContent.ItemType<Items.Material.Canvas>());
		}
	}
}
