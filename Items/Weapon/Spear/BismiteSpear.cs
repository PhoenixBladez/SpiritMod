using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Spear {
public class BismiteSpear : ModItem
{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Pike");
			Tooltip.SetDefault("Occasionally poisons foes");
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
        item.useAnimation = 32;
        item.useTime = 32;
        item.shootSpeed = 4f;
        item.knockBack = 4f;
        item.damage = 11;
        item.value = Item.sellPrice(0, 0, 60, 0);
        item.rare = 2;
        item.shoot = mod.ProjectileType("BismiteSpearProj");
    }
    
    public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BismiteCrystal", 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
}
}
