using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.BriarDrops;

namespace SpiritMod.Tiles.Block.Ambient
{
	public abstract class AmbientBlockItem<T> : ModItem where T : ModTile
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;
			Item.maxStack = 999;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<T>();
		}
	}

	public abstract class AmbientStoneItem<T> : AmbientBlockItem<T> where T : ModTile
	{
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.StoneBlock, 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Recipe.Create(ItemID.StoneBlock, 1);
			recipe1.AddIngredient(this, 1);
			recipe1.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe1.Register();
		}
	}

	public abstract class AmbientDirtItem<T> : AmbientBlockItem<T> where T : ModTile
	{
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.DirtBlock, 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Recipe.Create(ItemID.DirtBlock, 1);
			recipe1.AddIngredient(this, 1);
			recipe1.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe1.Register();
		}
	}

	public abstract class AmbientCorruptItem<T> : AmbientBlockItem<T> where T : ModTile
	{
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(25);
			recipe.AddIngredient(ItemID.StoneBlock, 25);
			recipe.AddIngredient(ItemID.RottenChunk, 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Recipe.Create(ItemID.StoneBlock, 25);
			recipe1.AddIngredient(this, 25);
			recipe1.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe1.Register();
		}
	}

	public abstract class AmbientCrimsonItem<T> : AmbientBlockItem<T> where T : ModTile
	{
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(25);
			recipe.AddIngredient(ItemID.StoneBlock, 25);
			recipe.AddIngredient(ItemID.Vertebrae, 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Recipe.Create(ItemID.StoneBlock, 25);
			recipe1.AddIngredient(this, 25);
			recipe1.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe1.Register();
		}
	}
	public abstract class AmbientHallowItem<T> : AmbientBlockItem<T> where T : ModTile
	{
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(25);
			recipe.AddIngredient(ItemID.StoneBlock, 25);
			recipe.AddIngredient(ItemID.CrystalShard, 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Recipe.Create(ItemID.StoneBlock, 25);
			recipe1.AddIngredient(this, 25);
			recipe1.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe1.Register();
		}
	}

	public abstract class AmbientLeafItem<T> : AmbientBlockItem<T> where T : ModTile
	{
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(25);
			recipe.AddIngredient(ItemID.DirtBlock, 25);
			recipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Recipe.Create(ItemID.DirtBlock, 25);
			recipe1.AddIngredient(this, 25);
			recipe1.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe1.Register();
		}
	}

	public abstract class AzureGemItem<T> : AmbientBlockItem<T> where T : ModTile
	{
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(50);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddIngredient(ModContent.ItemType<Items.Sets.SeraphSet.MoonStone>(), 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Recipe.Create(ModContent.ItemType<Items.Sets.SeraphSet.MoonStone>(), 1);
			recipe1.AddIngredient(this, 50);
			recipe1.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe1.Register();
		}
	}
}