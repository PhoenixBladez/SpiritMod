using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
namespace SpiritMod.Tiles.Piles
{
	public class CopperPile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Copper Pile");
			AddMapEntry(new Color(70, 70, 70), name);
			DustType = DustID.CopperCoin;
			TileID.Sets.DisableSmartCursor[Type] = true;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 36, 18, ItemID.CopperOre, Main.rand.Next(5, 6) + 1);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 10, 18, 36, ItemID.CopperOre);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 10, j * 16, 36, 36, ItemID.CopperOre);
		}
	}
	public class TinPile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 1;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Tin Pile");
			AddMapEntry(new Color(70, 70, 70), name);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			DustType = DustID.SilverCoin;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 18, 36, ItemID.TinOre, Main.rand.Next(5, 6) + 1);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 10, 36, 18, ItemID.TinOre);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 36, 36, ItemID.TinOre);
		}
	}
	public class IronPile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 2;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Iron Pile");
			AddMapEntry(new Color(70, 70, 70), name);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			DustType = DustID.SilverCoin;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 18, 36, ItemID.IronOre, Main.rand.Next(5, 7) + 3);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 10, 36, 18, ItemID.IronOre);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 36, 36, ItemID.IronOre);
		}
	}
	public class LeadPile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 2;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Lead Pile");
			AddMapEntry(new Color(70, 70, 70), name);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			DustType = DustID.SilverCoin;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 18, 36, ItemID.LeadOre, Main.rand.Next(5, 7) + 3);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 10, 36, 18, ItemID.LeadOre);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 36, 36, ItemID.LeadOre);
		}
	}
	public class SilverPile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 2;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Silver Pile");
			AddMapEntry(new Color(70, 70, 70), name);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			DustType = DustID.SilverCoin;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 18, 36, ItemID.SilverOre, Main.rand.Next(6, 8) + 4);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 10, 36, 18, ItemID.SilverOre);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 36, 36, ItemID.SilverOre);
		}
	}

	public class TungstenPile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 2;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Tungsten Pile");
			AddMapEntry(new Color(70, 70, 70), name);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			DustType = DustID.SilverCoin;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 18, 36, ItemID.TungstenOre, Main.rand.Next(6, 8) + 4);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 10, 36, 18, ItemID.TungstenOre);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 36, 36, ItemID.TungstenOre);
		}
	}
	public class GoldPile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 2;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Gold Pile");
			AddMapEntry(new Color(70, 70, 70), name);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			DustType = DustID.GoldCoin;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 18, 36, ItemID.GoldOre, Main.rand.Next(10, 12) + 4);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 10, 36, 18, ItemID.GoldOre);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 36, 36, ItemID.GoldOre);
		}
	}
	public class PlatinumPile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 2;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Platinum Pile");
			AddMapEntry(new Color(70, 70, 70), name);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			DustType = DustID.SilverCoin;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 18, 36, ItemID.PlatinumOre, Main.rand.Next(10, 12) + 4);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 10, 36, 18, ItemID.PlatinumOre);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 36, 36, ItemID.PlatinumOre);
		}
	}
}