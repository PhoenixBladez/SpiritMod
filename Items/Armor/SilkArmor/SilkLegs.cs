using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.SilkArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class SilkLegs : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Manasilk Leggings");

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Silk, 5);
			recipe.AddRecipeGroup("SpiritMod:GoldBars");
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}