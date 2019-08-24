using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Material
{
	public class WorshipCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worshipper's Jewel");
			Tooltip.SetDefault("'Carry a silent prayer with you'");
        }


        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 30;
            item.value = 100;
            item.rare = 8;

            item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SunShard", 3);
            recipe.AddIngredient(null, "EnchantedLeaf", 3);
            recipe.AddIngredient(ItemID.SoulofFlight, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 4);
            recipe.AddRecipe();
        }
    }
}
