using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class BismiteSword : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Sword");
			Tooltip.SetDefault("On use, shoots a Bismite Wrath Shard \n Occasionally poisons foes");
		}


        public override void SetDefaults()
        {
            item.damage = 11;            
            item.melee = true;            
            item.width = 34;              
            item.height = 40;             
            item.useTime = 24;           
            item.useAnimation = 24;     
            item.useStyle = 1;        
            item.knockBack = 6;
            item.value = Item.sellPrice(0, 0, 15, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item1;          
            item.shoot = mod.ProjectileType("BismiteSwordProjectile");
            item.shootSpeed = 7; ;            
            item.crit = 8;                     
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null,"BismiteCrystal", 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}