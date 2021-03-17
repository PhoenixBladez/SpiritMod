using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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
			recipe.AddIngredient(ModContent.ItemType<Items.Material.EnchantedLeaf>(), 1);
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
}