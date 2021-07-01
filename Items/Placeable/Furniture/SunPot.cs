using SpiritMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.BriarDrops;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class SunPot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sun in a Pot");
			Tooltip.SetDefault("Increases life regeneration during the day");
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

			item.createTile = ModContent.TileType<SunPotTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ClayPot);
            recipe.AddIngredient(ItemID.Daybloom);
            recipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 2);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}