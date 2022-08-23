using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.BriarDrops;
using SpiritMod.Tiles.Furniture.Hanging;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class HangingSunPot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hanging Sun in a Pot");
			Tooltip.SetDefault("Increases life regeneration during the day");
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 28;
			Item.value = Item.value = Item.sellPrice(0, 0, 3, 0);
			Item.rare = ItemRarityID.White;
			Item.maxStack = 99;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<HangingSunPotTile>(); 
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.BrazierSuspended);
            recipe.AddIngredient(ItemID.Daybloom);
            recipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 2);
			recipe.Register();
		}
	}
}