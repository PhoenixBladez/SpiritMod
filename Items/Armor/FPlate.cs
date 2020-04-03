using System.Collections.Generic;
using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class FPlate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Plate");
            Tooltip.SetDefault("Increases melee speed and movement speed by 6%");

        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 18;
            item.value = Terraria.Item.sellPrice(0, 0, 11, 0);
            item.rare = 2;
            item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeSpeed += .06f;
            player.moveSpeed += .06f;
            player.maxRunSpeed += .03f;
        }
        public override void AddRecipes()  //How to craft this item
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FloranBar", 14);   //you need 10 Wood
            recipe.AddTile(TileID.Anvils);   //at work bench
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}