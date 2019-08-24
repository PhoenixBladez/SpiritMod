using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class CleftHorn : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cleft Horn");
			Tooltip.SetDefault("Increases melee damage and critical strike chance by 4%");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
            item.value = Item.buyPrice(0, 0, 50, 0);
			item.rare = 2;

			item.accessory = true;

			item.defense = 1;
		}

		public override void UpdateEquip(Player player)
		{
			player.meleeCrit += 4;
			player.meleeDamage += 0.04f;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Carapace", 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
