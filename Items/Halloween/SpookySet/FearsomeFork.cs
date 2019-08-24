using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween.SpookySet
{
    public class FearsomeFork : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fearsome Fork");
			Tooltip.SetDefault("Launches pumpkins");
		}


         public override void SetDefaults()
        {
            item.useStyle = 5;
            item.width = 24;
            item.height = 24;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.melee = true;
            item.noMelee = true;
            item.useAnimation = 27;
            item.useTime = 27;
            item.shootSpeed = 5f;
            item.knockBack = 8f;
            item.damage = 67;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 8;
            item.shoot = mod.ProjectileType("FearsomeFork");
        }
		
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(1729, 12);
            recipe.AddTile(18);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
		}
    }
}