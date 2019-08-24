using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;


namespace SpiritMod.Items.Weapon.Gun
{
    public class CthulhuGun : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("R'lyehian Sidearm");
			Tooltip.SetDefault("Converts bullets into Mystic Bullets\nThe second shot fired will cause Mystic Bullets to explode\nThe third shot fired increases the potency of the bullet and its explosion\nResets every three shots");
		}


        int charger;
        private int memes;
        public override void SetDefaults()
        {
            item.damage = 29;
            item.ranged = true;   
            item.width = 34;     
            item.height = 28;    
            item.useTime = 21;
            item.useAnimation = 21;
            item.useStyle = 5;    
            item.noMelee = true; 
            item.knockBack = 3;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 3, 38, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item95;
            item.autoReuse = true;
            item.shoot = 10; 
            item.shootSpeed = 8f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            charger++;
            if (charger == 1)
            {
                for (int I = 0; I < 1; I++)
                {
                    Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("PurpleBullet1"), damage, knockBack, player.whoAmI, 0f, 0f);
                    return false;
                }
            }
            if (charger == 2)
            {
                for (int I = 0; I < 1; I++)
                {
                    Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("PurpleBullet2"), damage, knockBack, player.whoAmI, 0f, 0f);
                    return false;
                }
            }
            if (charger == 3)
            {
                for (int I = 0; I < 1; I++)
                {
                    Projectile.NewProjectile(position.X, position.Y, speedX , speedY, mod.ProjectileType("PurpleBullet3"), damage / 5 * 6, 7, player.whoAmI, 0f, 0f);
                    charger = 0;
                }
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }
}
