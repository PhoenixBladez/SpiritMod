using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
    public class ShadowShot : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Shot");
			Tooltip.SetDefault("Shoots out a spread of Vile Bullets");
		}


        public override void SetDefaults()
        {
            item.damage = 26;
            item.ranged = true;
            item.width = 50;
            item.height = 38;    
            item.useTime = 35;  
            item.useAnimation = 35;
            item.useStyle = 5;    
            item.noMelee = true; 
            item.knockBack = 6;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 4, 0, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item36;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("VileBullet"); 
            item.shootSpeed = 50f;
            item.useAmmo = AmmoID.Bullet;
            item.crit = 6;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float spread = 30f * 0.0174f;//45 degrees converted to radians
            float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
            double baseAngle = Math.Atan2(speedX, speedY);
            for (int i = 0; i < 4; i++)
            {
                double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
                speedX = baseSpeed * (float)Math.Sin(randomAngle);
                speedY = baseSpeed * (float)Math.Cos(randomAngle);
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("VileBullet"), item.damage, knockBack, item.owner, 0, 0);
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Boomstick);
            recipe.AddIngredient(ItemID.Musket);
            recipe.AddIngredient(ItemID.PhoenixBlaster, 1);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }
}