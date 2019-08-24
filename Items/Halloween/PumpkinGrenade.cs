using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween
{
    public class PumpkinGrenade : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pumpkin Grenade");
		}


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Grenade);
            item.shoot = mod.ProjectileType("PumpkinGrenade");
            item.useAnimation = 30;
            item.rare = 3;
            item.useTime = 60;
            item.damage = 66;
			item.value = 500;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(1725, 1);
			 recipe.AddIngredient(168, 5);
            recipe.AddTile(18);
            recipe.SetResult(this, 5);
            recipe.AddRecipe();
		}
    }
}