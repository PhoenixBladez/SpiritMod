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
			item.width = 30;
			item.height = 24;
			item.value = Item.buyPrice(0, 2, 0, 0);
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.rare = ItemRarityID.Pink;
			item.maxStack = 999;
			item.createTile = ModContent.TileType<SpiritBarTile>();
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<SpiritOre>(), 4);
			recipe.AddTile(TileID.AdamantiteForge);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}