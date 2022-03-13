
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class SpectreRing : AccessoryItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectre Ring");
			Tooltip.SetDefault("When hurt, you shoot a bolt of Spectre Energy to protect yourself.");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(0, 15, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.accessory = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SpectreBar, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
