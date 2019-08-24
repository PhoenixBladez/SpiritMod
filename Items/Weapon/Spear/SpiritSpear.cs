using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Spear {
public class SpiritSpear : ModItem
{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Spear");
		}


    public override void SetDefaults()
    {
        item.useStyle = 5;
        item.width = 56;
        item.height = 56;
        item.noUseGraphic = true;
        item.UseSound = SoundID.Item1;
        item.melee = true;
        item.noMelee = true;
        item.useAnimation = 25;
        item.useTime = 25;
        item.shootSpeed = 6f;
        item.knockBack = 7f;
        item.damage = 60;
        item.value = Item.sellPrice(0, 1, 15, 0);
        item.rare = 5;
        item.autoReuse = false;
        item.shoot = mod.ProjectileType("SpiritSpearProjectile");
    }
    
    public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SpiritBar", 12);
            recipe.AddIngredient(null, "SoulShred", 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
}}
