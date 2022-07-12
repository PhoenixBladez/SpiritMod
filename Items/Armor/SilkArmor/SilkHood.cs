using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.SilkArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class SilkHood : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Manasilk Hood");
			Tooltip.SetDefault("Increases your max number of minions");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.value = 7500;
			Item.rare = ItemRarityID.Blue;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.Silk, 3);
			recipe.AddRecipeGroup("SpiritMod:GoldBars");
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}