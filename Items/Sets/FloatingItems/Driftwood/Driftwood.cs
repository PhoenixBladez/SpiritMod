using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;

namespace SpiritMod.Items.Sets.FloatingItems.Driftwood
{
	public class Driftwood1Item : FloatingItem
	{
		public override float SpawnWeight => 0.9f;
		public override float Weight => base.Weight * 0.9f;
		public override float Bouyancy => base.Bouyancy * 1.05f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Small Driftwood");
			Tooltip.SetDefault("'Aesthetically pleasing'");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 24;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = 0;
			item.rare = ItemRarityID.White;
			item.createTile = ModContent.TileType<Driftwood1Tile>();
			item.maxStack = 999;
			item.autoReuse = true;
			item.consumable = true;
			item.useAnimation = 15;
			item.useTime = 10;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1);
			recipe.SetResult(ModContent.ItemType<DriftwoodTileItem>(), 10);
			recipe.AddRecipe();
		}
	}

	public class Driftwood1Tile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileTable[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleWrapLimit = 2;
			TileObjectData.newTile.StyleMultiplier = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1); //facing right will use the second texture style
			TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Driftwood");
			AddMapEntry(new Color(69, 54, 43), name);
			dustType = DustID.Stone;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(i * 16, j * 16, 48, 48, ModContent.ItemType<Driftwood1Item>());
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height) => offsetY = 2;
	}

	public class Driftwood2Item : FloatingItem
	{
		public override float Weight => base.Weight * 0.9f;
		public override float Bouyancy => base.Bouyancy * 1.05f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Medium Driftwood");
			Tooltip.SetDefault("'Aesthetically pleasing'");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 24;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = 0;
			item.rare = ItemRarityID.White;
			item.createTile = ModContent.TileType<Driftwood2Tile>();
			item.maxStack = 999;
			item.autoReuse = true;
			item.consumable = true;
			item.useAnimation = 15;
			item.useTime = 10;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1);
			recipe.SetResult(ModContent.ItemType<DriftwoodTileItem>(), 20);
			recipe.AddRecipe();
		}
	}

	public class Driftwood2Tile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileTable[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleWrapLimit = 2;
			TileObjectData.newTile.StyleMultiplier = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1); //facing right will use the second texture style
			TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Driftwood");
			AddMapEntry(new Color(69, 54, 43), name);
			dustType = DustID.Stone;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(i * 16, j * 16, 48, 48, ModContent.ItemType<Driftwood2Item>());
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height) => offsetY = 2;
	}

	public class Driftwood3Item : FloatingItem
	{
		public override float Weight => base.Weight * 0.9f;
		public override float Bouyancy => base.Bouyancy * 1.05f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Large Driftwood");
			Tooltip.SetDefault("'Aesthetically pleasing'");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 24;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = 0;
			item.rare = ItemRarityID.White;
			item.createTile = ModContent.TileType<Driftwood3Tile>();
			item.maxStack = 999;
			item.autoReuse = true;
			item.consumable = true;
			item.useAnimation = 15;
			item.useTime = 10;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1);
			recipe.SetResult(ModContent.ItemType<DriftwoodTileItem>(), 25);
			recipe.AddRecipe();
		}
	}

	public class Driftwood3Tile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileTable[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleWrapLimit = 2;
			TileObjectData.newTile.StyleMultiplier = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1); //facing right will use the second texture style
			TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Driftwood");
			AddMapEntry(new Color(69, 54, 43), name);
			dustType = DustID.Stone;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(i * 16, j * 16, 48, 48, ModContent.ItemType<Driftwood3Item>());
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height) => offsetY = 2;
	}
}