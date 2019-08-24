using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
    public class Swordsplosion : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Swordsplosion");
            Tooltip.SetDefault("Shoots out a barrage of swords\nProjectiles fired count both as melee and ranged projectiles");

        }


        public override void SetDefaults()
        {
            item.damage = 76;
            item.ranged = true;
            item.width = 60;
            item.height = 26;    
            item.useTime = 19;  
            item.useAnimation = 19;
            item.useStyle = 5;    
            item.noMelee = true; 
            item.knockBack = 6;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item36;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("SwordBarrage"); 
            item.shootSpeed = 10f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < 3; i++)
            {
                float spread = 35f * 0.0174f;//45 degrees converted to radians
                float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                double baseAngle = Math.Atan2(speedX, speedY);
                double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
                speedX = baseSpeed * (float)Math.Sin(randomAngle);
                speedY = baseSpeed * (float)Math.Cos(randomAngle);
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("SwordBarrage"), item.damage, knockBack, item.owner, 0, 0);
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }
}