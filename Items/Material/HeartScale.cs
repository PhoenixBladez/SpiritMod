using Terraria;
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
			Item.width = Item.height = 18;
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 99;
			Item.noUseGraphic = true;
			Item.value = Terraria.Item.sellPrice(0, 0, 10, 0);
			Item.noMelee = true;
			Item.autoReuse = false;
		}

		public override void AddRecipes()
		{
			Recipe recipe1 = Recipe.Create(ItemID.LifeforcePotion, 1);
			recipe1.AddIngredient(ItemID.BottledWater, 1);
			recipe1.AddIngredient(ModContent.ItemType<HeartScale>(), 3);
			recipe1.AddIngredient(ItemID.Waterleaf, 1);
			recipe1.AddIngredient(ItemID.Moonglow, 1);
			recipe1.AddIngredient(ItemID.Shiverthorn, 1);
			recipe1.AddTile(TileID.Bottles);
			recipe1.Register();
		}
	}
}
