using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class CobaltRing : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Ring");
			Tooltip.SetDefault("Increases melee and movement speed by 10%");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
            item.value = Item.buyPrice(0, 10, 0, 0);
			item.rare = 4;
			item.accessory = true;

			item.defense = 1;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			 player.moveSpeed *= 1.10f;
			 player.meleeSpeed *= 1.10f;
		}

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CobaltBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
