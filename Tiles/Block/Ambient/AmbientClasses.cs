using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.HuskstalkSet;
using SpiritMod.Items.Sets.BriarDrops;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Tiles.Block.Ambient
{
   public abstract class AmbientStoneItem : ModItem
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
	public abstract class AmbientDirtItem : ModItem
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
	public abstract class AmbientCorruptItem : ModItem
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
	public abstract class AmbientCrimsonItem : ModItem
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
	public abstract class AmbientHallowItem : ModItem
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
	public abstract class AmbientLeafItem : ModItem
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
	public abstract class AzureGemItem : ModItem
	{
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(50);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddIngredient(ModContent.ItemType<Items.Sets.SeraphSet.MoonStone>(), 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Recipe.Create(ModContent.ItemType<Items.Sets.SeraphSet.MoonStone>(),1);
			recipe1.AddIngredient(this, 50);
			recipe1.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe1.Register();
		}
	}
}