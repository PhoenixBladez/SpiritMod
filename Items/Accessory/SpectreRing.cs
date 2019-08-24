using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class SpectreRing : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectre Ring");
			Tooltip.SetDefault("When hurt, you shoot a bolt of Spectre Energy to protect yourself!");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
            item.value = Item.buyPrice(0, 15, 0, 0);
			item.rare = 8;

			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<MyPlayer>(mod).SRingOn = true;
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
