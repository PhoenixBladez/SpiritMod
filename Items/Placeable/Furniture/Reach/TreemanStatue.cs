using SpiritMod.Items.Sets.HuskstalkSet;
using SpiritMod.Items.Sets.BriarDrops;
using SpiritMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;
using TreemanStatueTile = SpiritMod.Tiles.Furniture.Reach.TreemanStatue;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
	public class TreemanStatue : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Statue of the Old Gods");
			Tooltip.SetDefault("Provides the effects of a Workbench, Potion Crafting Station, and Bookcase\n'The Old Ones will protect you'");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 28;
			item.value = 25500;
			item.maxStack = 99;
			item.rare = ItemRarityID.Orange;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;
			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = ModContent.TileType<TreemanStatueTile>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 50);
			recipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 20);
			recipe.AddIngredient(ItemID.GoldCoin, 10);
			recipe.AddIngredient(ItemID.Book, 5);
			recipe.AddIngredient(ItemID.Bone, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}