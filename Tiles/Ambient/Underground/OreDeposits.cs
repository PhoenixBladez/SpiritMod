using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.Underground
{
	public class OreDeposits : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Origin = new Terraria.DataStructures.Point16(1, 1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.RandomStyleRange = 4;
			TileObjectData.addTile(Type);

			DustType = DustID.BrownMoss;

			AddMapEntry(Color.Pink);
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) => offsetY = 2;

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int itemID = ItemID.TungstenOre;
			int frame = frameY / 36;

			if (frame == 1)
				itemID = ItemID.SilverOre;
			else if (frame == 2)
				itemID = ItemID.IronOre;
			else if (frame == 3)
				itemID = ItemID.GoldOre;

			Item.NewItem(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, itemID, Main.rand.Next(22, 31));
		}
	}
}