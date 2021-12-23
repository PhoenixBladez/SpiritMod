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
			item.width = 30;
			item.height = 24;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.rare = 0;
			item.maxStack = 999;
			item.createTile = TileID.LargePiles;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			SafeSetDefaults();

		}
		public virtual void SafeSetDefaults() { }
	}
	public abstract class DefaultLargePile2 : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 24;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.rare = 0;
			item.maxStack = 999;
			item.createTile = TileID.LargePiles2;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			SafeSetDefaults();

		}
		public virtual void SafeSetDefaults() { }
	}
	public class SkeletonPile : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Skeleton Pile");


		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Bone, 4);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool UseItem(Player player)
		{
			item.placeStyle = Main.rand.Next(0, 7);
			return base.UseItem(player);

		}
	}
	public class StonePile : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Stone Rubble");

        public override void SafeSetDefaults()
        {
			item.placeStyle = 7;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool UseItem(Player player)
		{
			item.placeStyle = Main.rand.Next(7, 13);
			return base.UseItem(player);
		}
	}
	public class StonePileHelmet : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Helmet Rubble");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 13;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class StonePilePickaxe : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Mining Rubble");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 14;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class StonePileSword : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Abandoned Sword Rubble");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 15;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class WoodRuinPile : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Ruined Furniture");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 22;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 5);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool UseItem(Player player)
		{
			item.placeStyle = Main.rand.Next(22, 24);
			return base.UseItem(player);
		}
	}
	public class ChestPile : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Abandoned Chest");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 24;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 8);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class ChandelierPile : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Broken Chandelier");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 25;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Chain, 3);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class IcePile : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Frozen Rubble");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 26;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IceBlock, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool UseItem(Player player)
		{
			item.placeStyle = Main.rand.Next(26, 32);
			return true;
		}
	}
	public class MushroomPile : DefaultLargePile1
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Glowing Mushroom Rubble");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 32;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 10);
			recipe.AddIngredient(ItemID.GlowingMushroom, 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool UseItem(Player player)
		{
			item.placeStyle = Main.rand.Next(32, 35);
			return base.UseItem(player);
		}
	}
	public class MossStonePile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Mossy Stone Rubble");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 5);
			recipe.AddIngredient(ItemID.MudBlock, 5);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool UseItem(Player player)
		{
			item.placeStyle = Main.rand.Next(0, 3);
			return base.UseItem(player);
		}
	}
	public class MossMudPile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Mossy Mud Rubble");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 3;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MudBlock, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool UseItem(Player player)
		{
			item.placeStyle = Main.rand.Next(3, 6);
			return base.UseItem(player);
		}
	}
	public class HellstonePile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Hellstone Rubble");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 6;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.AshBlock, 10);
			recipe.AddIngredient(ItemID.Hellstone, 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool UseItem(Player player)
		{
			item.placeStyle = Main.rand.Next(6, 9);
			return base.UseItem(player);
		}
	}
	public class WebPile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Webbed Rubble");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 9;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Cobweb, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool UseItem(Player player)
		{
			item.placeStyle = Main.rand.Next(9, 14);
			return base.UseItem(player);
		}
	}
	public class GrassStonePile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Grassy Stone Rubble");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 14;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 5);
			recipe.AddIngredient(ItemID.DirtBlock, 5);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool UseItem(Player player)
		{
			item.placeStyle = Main.rand.Next(14, 17);
			return base.UseItem(player);
		}
	}
	public class LihzahrdPile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Lihzahrd Brick Rubble");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 18;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LihzahrdBrick, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool UseItem(Player player)
		{
			item.placeStyle = Main.rand.Next(18, 21);
			return base.UseItem(player);
		}
	}
	public class MahoganyCage : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Mahogany Cage");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 21;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RichMahogany, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class MahoganyCageFull : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Mahogany Cage (Full)");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 22;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RichMahogany, 10);
			recipe.AddIngredient(ItemID.Bird, 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class RustyMinecart : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Rusty Minecart");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 23;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 3);
			recipe.AddIngredient(ItemID.DirtBlock, 5);
			recipe.anyIronBar = true;
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class OldWell : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Old Well");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 24;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 14);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class DirtPile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Pile of Dirt");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 25;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DirtBlock, 15);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class OldTent : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Old Tent");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 26;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 5);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class OldWheelbarrow : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Old Wheelbarrow");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 27;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 2);
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.anyIronBar = true;
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class SandPile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Sandstone Rubble");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 29;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SandBlock, 10);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool UseItem(Player player)
		{
			item.placeStyle = Main.rand.Next(29, 35);
			return base.UseItem(player);
		}
	}
	public class GranitePile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Granite Rubble");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 35;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Granite, 6);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool UseItem(Player player)
		{
			item.placeStyle = Main.rand.Next(35, 41);
			return base.UseItem(player);
		}
	}
	public class MarblePile : DefaultLargePile2
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Marble Rubble");

		public override void SafeSetDefaults()
		{
			item.placeStyle = 41;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Marble, 6);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool UseItem(Player player)
		{
			item.placeStyle = Main.rand.Next(41, 47);
			return base.UseItem(player);
		}
	}
}