using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
	public class HeartScale : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heart Scale");
			Tooltip.SetDefault("'A lovely scale. It is coveted by collectors.'");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 18;
			item.rare = ItemRarityID.Green;
			item.maxStack = 99;
			item.noUseGraphic = true;
			item.value = Terraria.Item.sellPrice(0, 0, 10, 0);
			item.noMelee = true;
			item.autoReuse = false;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(ItemID.BottledWater, 1);
			recipe1.AddIngredient(ModContent.ItemType<HeartScale>(), 3);
			recipe1.AddIngredient(ItemID.Waterleaf, 1);
			recipe1.AddIngredient(ItemID.Moonglow, 1);
			recipe1.AddIngredient(ItemID.Shiverthorn, 1);
			recipe1.AddTile(TileID.Bottles);
			recipe1.SetResult(ItemID.LifeforcePotion, 1);
			recipe1.AddRecipe();

			ModRecipe recipe2 = new ModRecipe(mod);
			recipe2.AddIngredient(ModContent.ItemType<HeartScale>(), 3);
			recipe2.AddIngredient(ModContent.ItemType<Items.Sets.Bismite.BismiteCrystal>(), 6);
			recipe2.AddTile(TileID.Anvils);
			recipe2.SetResult(ItemID.LifeCrystal, 1);
			recipe2.AddRecipe();
		}
	}
}
