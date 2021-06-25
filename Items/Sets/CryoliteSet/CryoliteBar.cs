using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CryoliteSet
{
	public class CryoliteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Bar");
		}


		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 24;
            item.value = Terraria.Item.sellPrice(0, 0, 25, 0);
            item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.rare = ItemRarityID.Orange;
			item.maxStack = 999;
			item.createTile = ModContent.TileType<CryoliteBarTile>();
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CryoliteOre>(), 3);
			recipe.AddTile(TileID.Hellforge);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}