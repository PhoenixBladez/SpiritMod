using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Spear {
public class ClatterSword : ModItem
{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clatter Sword");
			Tooltip.SetDefault("Attacks occasionally pierce through enemies, lowering their defense");
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
        item.useAnimation = 5;
        item.useTime = 15;
        item.shootSpeed = 7f;
        item.knockBack = 8f;
        item.damage = 22;
        item.value = Item.sellPrice(0, 0, 15, 0);
        item.rare = 2;
        item.autoReuse = false;
        item.shoot = mod.ProjectileType("ClatterSwordProj");
    }
    
    public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Carapace", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
}}
