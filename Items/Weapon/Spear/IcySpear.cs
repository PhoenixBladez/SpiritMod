using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Spear {
public class IcySpear : ModItem
{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Spear");
			Tooltip.SetDefault("Hit foes are occasionally frostburned");
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
        item.useAnimation = 35;
        item.useTime = 35;
        item.shootSpeed = 4f;
        item.knockBack = 8f;
        item.damage = 14;
        item.value = Item.sellPrice(0, 0, 60, 0);
        item.rare = 1;
        item.shoot = mod.ProjectileType("IcySpear");
    }
    
    public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FrigidFragment", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
