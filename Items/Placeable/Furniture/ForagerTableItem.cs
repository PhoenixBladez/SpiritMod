using SpiritMod.Tiles.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.BriarDrops;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class ForagerTableItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Naturalist's Workshop");
			Tooltip.SetDefault("Allows for the crafting of ambient objects and tiles");
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 28;
			Item.value = Item.value = Terraria.Item.sellPrice(0, 0, 3, 0);
			Item.rare = ItemRarityID.White;
			Item.maxStack = 99;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<ForagerTableTile>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 20);
			recipe.anyWood = true;
			recipe.AddIngredient(ItemID.IronBar, 2);
			recipe.anyIronBar = true;
            recipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 6);
			recipe.Register();
		}
	}
}