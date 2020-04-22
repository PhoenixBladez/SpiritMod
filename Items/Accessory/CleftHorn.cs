using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Neck)]
    public class CleftHorn : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cleft Horn");
			Tooltip.SetDefault("Increases armor penetration by 3\nMelee attacks occasionally strike enemies twice");
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
            player.armorPenetration += 3;
            player.GetSpiritPlayer().cleftHorn = true;
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
