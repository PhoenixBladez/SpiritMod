using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Material
{
	public class SturdyFur : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Durable Furs");
			Tooltip.SetDefault("'Tough, yet comfortable'");
        }


        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 30;
            item.value = 100;
            item.rare = 2;

            item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "OldLeather", 1);
            recipe.AddIngredient(null, "Carapace", 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 2);
            recipe.AddRecipe();
        }
    }
}