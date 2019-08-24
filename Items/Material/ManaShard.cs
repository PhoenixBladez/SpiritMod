using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Material
{
	public class ManaShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mana Shard");
			Tooltip.SetDefault("A conduit for mana");
        }


        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 30;
            item.value = 100;
            item.rare = 1;

            item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FallenStar, 2);
            recipe.AddIngredient(ItemID.TissueSample, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
			
			  ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemID.FallenStar, 2);
            recipe2.AddIngredient(ItemID.ShadowScale, 2);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this, 1);
            recipe2.AddRecipe();


            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(null, "ManaShard", 3);
            recipe1.AddTile(TileID.Anvils);
            recipe1.SetResult(ItemID.BandofStarpower, 1);
            recipe1.AddRecipe();
        }
    }
}