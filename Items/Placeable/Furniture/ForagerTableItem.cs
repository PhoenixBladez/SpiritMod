using SpiritMod.Tiles.Furniture;
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
			item.width = 36;
			item.height = 28;
			item.value = item.value = Terraria.Item.sellPrice(0, 0, 3, 0);
			item.rare = ItemRarityID.White;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<ForagerTableTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 20);
			recipe.anyWood = true;
			recipe.AddIngredient(ItemID.IronBar, 2);
			recipe.anyIronBar = true;
            recipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 6);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}