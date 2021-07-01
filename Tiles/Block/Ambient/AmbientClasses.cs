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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(this, 1);
			recipe1.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe1.SetResult(ItemID.StoneBlock, 1);
			recipe1.AddRecipe();
		}
	}
	public abstract class AmbientDirtItem : ModItem
	{
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DirtBlock, 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(this, 1);
			recipe1.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe1.SetResult(ItemID.DirtBlock, 1);
			recipe1.AddRecipe();
		}
	}
	public abstract class AmbientCorruptItem : ModItem
	{
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 25);
			recipe.AddIngredient(ItemID.RottenChunk, 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this, 25);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(this, 25);
			recipe1.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe1.SetResult(ItemID.StoneBlock, 25);
			recipe1.AddRecipe();
		}
	}
	public abstract class AmbientCrimsonItem : ModItem
	{
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 25);
			recipe.AddIngredient(ItemID.Vertebrae, 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this, 25);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(this, 25);
			recipe1.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe1.SetResult(ItemID.StoneBlock, 25);
			recipe1.AddRecipe();
		}
	}
	public abstract class AmbientHallowItem : ModItem
	{
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 25);
			recipe.AddIngredient(ItemID.CrystalShard, 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this, 25);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(this, 25);
			recipe1.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe1.SetResult(ItemID.StoneBlock, 25);
			recipe1.AddRecipe();
		}
	}
	public abstract class AmbientLeafItem : ModItem
	{
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DirtBlock, 25);
			recipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this, 25);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(this, 25);
			recipe1.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe1.SetResult(ItemID.DirtBlock, 25);
			recipe1.AddRecipe();
		}
	}
	public abstract class AzureGemItem : ModItem
	{
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddIngredient(ModContent.ItemType<Items.Sets.SeraphSet.MoonStone>(), 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this, 50);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(this, 50);
			recipe1.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe1.SetResult(ModContent.ItemType<Items.Sets.SeraphSet.MoonStone>(),1);
			recipe1.AddRecipe();
		}
	}
}