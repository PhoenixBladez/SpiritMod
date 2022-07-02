using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.AmbientObjects
{
	//This file corresponds to all ambient objects in TileID.LargePiles and TileID.LargePiles2, Tile 186 and 187
	public abstract class DefaultLargePile1 : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 24;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.White;
			Item.maxStack = 999;
			Item.createTile = TileID.LargePiles;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			SafeSetDefaults();

		}
		public virtual void SafeSetDefaults() { }
	}

	public abstract class DefaultLargePile2 : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 24;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.White;
			Item.maxStack = 999;
			Item.createTile = TileID.LargePiles2;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			SafeSetDefaults();

		}
		public virtual void SafeSetDefaults() { }
	}

	public class SkeletonPile : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Skeleton Pile");

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Bone, 4);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}

		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			Item.placeStyle = Main.rand.Next(0, 7);
			return base.UseItem(player);
		}
	}

	public class StonePile : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Stone Rubble");
		public override void SafeSetDefaults() => Item.placeStyle = 7;

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.StoneBlock, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}

		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			Item.placeStyle = Main.rand.Next(7, 13);
			return base.UseItem(player);
		}
	}

	public class StonePileHelmet : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Helmet Rubble");
		public override void SafeSetDefaults() => Item.placeStyle = 13;

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.StoneBlock, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
	}

	public class StonePilePickaxe : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Mining Rubble");
		public override void SafeSetDefaults() => Item.placeStyle = 14;

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.StoneBlock, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
	}
	public class StonePileSword : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Abandoned Sword Rubble");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 15;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.StoneBlock, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
	}

	public class WoodRuinPile : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Ruined Furniture");
		public override void SafeSetDefaults() => Item.placeStyle = 22;

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 5);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}

		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			Item.placeStyle = Main.rand.Next(22, 24);
			return base.UseItem(player);
		}
	}
	public class ChestPile : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Abandoned Chest");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 24;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 8);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
	}
	public class ChandelierPile : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Broken Chandelier");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 25;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Chain, 3);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
	}
	public class IcePile : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Frozen Rubble");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 26;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IceBlock, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			Item.placeStyle = Main.rand.Next(26, 32);
			return true;
		}
	}
	public class MushroomPile : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Glowing Mushroom Rubble");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 32;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.StoneBlock, 10);
			recipe.AddIngredient(ItemID.GlowingMushroom, 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			Item.placeStyle = Main.rand.Next(32, 35);
			return base.UseItem(player);
		}
	}
	public class MossStonePile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Mossy Stone Rubble");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 1;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.StoneBlock, 5);
			recipe.AddIngredient(ItemID.MudBlock, 5);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			Item.placeStyle = Main.rand.Next(0, 3);
			return base.UseItem(player);
		}
	}
	public class MossMudPile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Mossy Mud Rubble");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 3;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MudBlock, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			Item.placeStyle = Main.rand.Next(3, 6);
			return base.UseItem(player);
		}
	}
	public class HellstonePile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Hellstone Rubble");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 6;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.AshBlock, 10);
			recipe.AddIngredient(ItemID.Hellstone, 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			Item.placeStyle = Main.rand.Next(6, 9);
			return base.UseItem(player);
		}
	}
	public class WebPile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Webbed Rubble");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 9;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Cobweb, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			Item.placeStyle = Main.rand.Next(9, 14);
			return base.UseItem(player);
		}
	}
	public class GrassStonePile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Grassy Stone Rubble");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 14;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.StoneBlock, 5);
			recipe.AddIngredient(ItemID.DirtBlock, 5);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			Item.placeStyle = Main.rand.Next(14, 17);
			return base.UseItem(player);
		}
	}
	public class LihzahrdPile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Lihzahrd Brick Rubble");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 18;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LihzahrdBrick, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			Item.placeStyle = Main.rand.Next(18, 21);
			return base.UseItem(player);
		}
	}
	public class MahoganyCage : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Mahogany Cage");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 21;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.RichMahogany, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
	}
	public class MahoganyCageFull : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Mahogany Cage (Full)");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 22;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.RichMahogany, 10);
			recipe.AddIngredient(ItemID.Bird, 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
	}
	public class RustyMinecart : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Rusty Minecart");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 23;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 3);
			recipe.AddIngredient(ItemID.DirtBlock, 5);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
	}
	public class OldWell : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Old Well");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 24;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.StoneBlock, 14);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
	}
	public class DirtPile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Pile of Dirt");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 25;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 15);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
	}
	public class OldTent : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Old Tent");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 26;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Silk, 5);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
	}
	public class OldWheelbarrow : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Old Wheelbarrow");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 27;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 10);
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
	}
	public class SandPile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Sandstone Rubble");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 29;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SandBlock, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			Item.placeStyle = Main.rand.Next(29, 35);
			return base.UseItem(player);
		}
	}
	public class GranitePile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Granite Rubble");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 35;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Granite, 6);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			Item.placeStyle = Main.rand.Next(35, 41);
			return base.UseItem(player);
		}
	}
	public class MarblePile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Marble Rubble");

		public override void SafeSetDefaults()
		{
			Item.placeStyle = 41;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Marble, 6);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			Item.placeStyle = Main.rand.Next(41, 47);
			return base.UseItem(player);
		}
	}
}