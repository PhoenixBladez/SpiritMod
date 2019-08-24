using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Spear {
public class DuskLance : ModItem
{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dusk Lance");
			Tooltip.SetDefault("Occasionally shoots out an apparition that inflicts Shadowflame");
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
        item.useAnimation = 25;
        item.useTime = 25;
        item.shootSpeed = 6f;
        item.knockBack = 6f;
        item.damage = 38;
        item.value = Item.sellPrice(0, 3, 60, 0);
        item.rare = 6;
        item.shoot = mod.ProjectileType("DuskLanceProj");
    }
    
    public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DarkLance, 1);
            recipe.AddIngredient(null, "DuskStone", 4);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    } 
}
