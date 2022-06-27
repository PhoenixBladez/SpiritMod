using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
namespace SpiritMod.Tiles.Furniture.SpaceJunk
{
	public class ScrapItem1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Salvaged Scrap");
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.value = Item.buyPrice(0, 0, 30, 0);

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 15;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = Mod.Find<ModTile>("ScrapTile1").Type;
		}
	}
	public class ScrapTile1 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileTable[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Space Scrap");
			AddMapEntry(new Color(150, 150, 150), name);
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			offsetY = 2;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
			Terraria.Item.NewItem(i * 16, j * 16, 64, 32, Mod.Find<ModItem>("ScrapItem1").Type);
		}
	}
	public class ScrapItem2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Salvaged Scrap");
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.value = Item.buyPrice(0, 0, 30, 0);

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 15;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = Mod.Find<ModTile>("ScrapTile2").Type;
		}
	}
	public class ScrapTile2 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileTable[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Space Scrap");
			AddMapEntry(new Color(150, 150, 150), name);
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			offsetY = 2;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
			Terraria.Item.NewItem(i * 16, j * 16, 64, 32, Mod.Find<ModItem>("ScrapItem2").Type);
		}
	}

	public class ScrapItem3 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Salvaged Scrap");
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.value = Item.buyPrice(0, 0, 30, 0);

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 15;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = Mod.Find<ModTile>("ScrapTile3").Type;
		}
	}
	public class ScrapTile3 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16
			};
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Space Scrap");
			AddMapEntry(new Color(150, 150, 150), name);
			TileObjectData.newTile.AnchorBottom = default(AnchorData);
			TileObjectData.newTile.AnchorTop = default(AnchorData);
			TileObjectData.newTile.AnchorWall = true;
			TileObjectData.addTile(Type);
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			offsetY = 2;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
			Item.NewItem(i * 16, j * 16, 32, 48, Mod.Find<ModItem>("ScrapItem3").Type);
		}
	}
	public class ScrapItem4 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Salvaged Scrap");
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.value = Item.buyPrice(0, 0, 30, 0);

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 15;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = Mod.Find<ModTile>("ScrapTile4").Type;
		}
	}
	public class ScrapTile4 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Space Scrap");
			AddMapEntry(new Color(150, 150, 150), name);
			TileObjectData.newTile.AnchorBottom = default(AnchorData);
			TileObjectData.newTile.AnchorTop = default(AnchorData);
			TileObjectData.newTile.AnchorWall = true;
			TileObjectData.addTile(Type);
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			offsetY = 2;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
			Item.NewItem(i * 16, j * 16, 32, 48, Mod.Find<ModItem>("ScrapItem4").Type);
		}
	}
	public class ScrapItem5 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Salvaged Scrap");
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.value = Item.buyPrice(0, 0, 30, 0);

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 15;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = Mod.Find<ModTile>("ScrapTile5").Type;
		}
	}
	public class ScrapTile5 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileTable[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Space Scrap");
			AddMapEntry(new Color(150, 150, 150), name);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.addTile(Type);
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			offsetY = 2;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
			Terraria.Item.NewItem(i * 16, j * 16, 64, 32, Mod.Find<ModItem>("ScrapItem5").Type);
		}
	}
	public class ScrapItem6 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Salvaged Scrap");
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.value = Item.buyPrice(0, 0, 30, 0);

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 15;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = Mod.Find<ModTile>("ScrapTile6").Type;
		}
	}
	public class ScrapTile6 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileTable[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Width = 1;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Space Scrap");
			AddMapEntry(new Color(150, 150, 150), name);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16
			};
			TileObjectData.addTile(Type);
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			offsetY = 2;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
			Terraria.Item.NewItem(i * 16, j * 16, 64, 32, Mod.Find<ModItem>("ScrapItem6").Type);
		}
	}
}