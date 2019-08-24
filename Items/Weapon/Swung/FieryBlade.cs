using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class FieryBlade : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Blade");
			Tooltip.SetDefault("Shoots out a bouncing blaze");
		}


        public override void SetDefaults()
        {
            item.damage = 26;            
            item.melee = true;            
            item.width = 34;              
            item.height = 40;
            item.autoReuse = true;           
            item.useTime = 27;           
            item.useAnimation = 27;     
            item.useStyle = 1;        
            item.knockBack = 0;
            item.value = Item.sellPrice(0, 0, 40, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item1;          
            item.shoot = mod.ProjectileType("Blaze");
            item.shootSpeed = 3f ;                               
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null,"CarvedRock", 13);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(3) == 0)
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 6);
            }
        }

    }
}