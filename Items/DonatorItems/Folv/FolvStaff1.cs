
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.Folv
{
	[AutoloadEquip(EquipType.Balloon)]
	public class FolvStaff1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Folv's Worn Staff");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.rare = ItemRarityID.Blue;
			item.value = 5000;
			item.accessory = true;
            item.vanity = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 40);
			recipe.AddRecipeGroup("SpiritMod:GoldBars", 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
