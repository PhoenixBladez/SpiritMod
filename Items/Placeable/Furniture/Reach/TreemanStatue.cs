using SpiritMod.Items.Sets.HuskstalkSet;
using SpiritMod.Items.Sets.BriarDrops;
using Terraria;
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
			Item.width = 32;
			Item.height = 28;
			Item.value = 25500;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Orange;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<TreemanStatueTile>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 50);
			recipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 20);
			recipe.AddIngredient(ItemID.GoldCoin, 10);
			recipe.AddIngredient(ItemID.Book, 5);
			recipe.AddIngredient(ItemID.Bone, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}