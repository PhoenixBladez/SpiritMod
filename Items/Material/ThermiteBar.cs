using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
	public class ThermiteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thermal Bar");
		}


		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 24;
			item.value = 10000;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.rare = 8;
			item.consumable = true;
			item.maxStack = 99;
			item.createTile = ModContent.TileType<Tiles.Furniture.ThermiteBar>();
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ThermiteOre>(), 4);
			recipe.AddTile(TileID.AdamantiteForge);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}