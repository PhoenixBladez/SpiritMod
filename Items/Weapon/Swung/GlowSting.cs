using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class GlowSting : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glowsting");
			Tooltip.SetDefault("Launches a returning scythe");
		}


        public override void SetDefaults()
        {
            item.damage = 50;            
            item.melee = true;            
            item.width = 34;              
            item.height = 40;  
item.autoReuse = true;			
            item.useTime = 25;           
            item.useAnimation = 25;     
            item.useStyle = 1;        
            item.knockBack = 6;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item1;          
            item.shoot = mod.ProjectileType("GlowStingScythe");
                       item.shootSpeed = 10f;         
            item.crit = 8;                     
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MoonStone", 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}