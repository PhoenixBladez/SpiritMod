using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritSet
{
	public class SpiritBar : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Spirit Bar");

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 24;
			Item.value = Item.buyPrice(0, 2, 0, 0);
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Pink;
			Item.maxStack = 999;
			Item.createTile = ModContent.TileType<SpiritBarTile>();
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<SpiritOre>(), 4);
			recipe.AddTile(TileID.AdamantiteForge);
			recipe.Register();
		}
	}
}