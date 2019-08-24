using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class TitaniumRing : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titanium Band");
			Tooltip.SetDefault("Every hit has a chance to give you shadow dodge");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
            item.value = Item.buyPrice(0, 10, 0, 0);
			item.rare = 4;

			item.accessory = true;

			item.defense = 2;
		}

		public override void UpdateEquip(Player player)
        {
            if (Main.rand.Next(6) == 0)            
            {

                player.GetModPlayer<MyPlayer>(mod).TiteRing = true;
            }
		}
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TitaniumBar, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
