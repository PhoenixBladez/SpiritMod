using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Leather
{
    [AutoloadEquip(EquipType.Shoes)]
    public class LeatherBoots : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leather Striders");
			Tooltip.SetDefault("Slightly increases movement speed and acceleration");
		}
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 20;
            item.value = Item.buyPrice(0, 0, 4, 0);
            item.rare = 1;

            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed += 0.09f;
            player.runAcceleration += .06f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("OldLeather"), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
