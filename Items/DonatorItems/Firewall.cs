using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	[AutoloadEquip(EquipType.Shield)]
    public class Firewall : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Firewall");
			Tooltip.SetDefault("Negates knockback \nDouble tap right to dash\n~Donator Item~");
		}
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 28;
            item.rare = 5;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.accessory = true;
			item.defense = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>(mod).firewall = true;
            player.noKnockback = true;
        }

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(397, 1);
			recipe.AddIngredient(3097, 1);
			recipe.AddIngredient(null, "InfernalShield", 1);
            recipe.AddTile(114);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
