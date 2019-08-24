using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

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
			DisplayName.SetDefault("Folv's Staff");
			Tooltip.SetDefault("Grants 2% increased magic damage and +10 maximum mana \n~Donator Item~");
		}
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.rare = 1;
            item.value = 5000;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2 += 10;
            player.magicDamage *= 1.02f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 40);
            recipe.AddRecipeGroup("GoldBars", 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
