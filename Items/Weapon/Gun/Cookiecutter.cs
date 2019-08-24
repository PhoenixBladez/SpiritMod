using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.Items.Weapon.Gun
{
    public class Cookiecutter : ModItem
    { 
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Cookiecutter");
			Tooltip.SetDefault("'Rapidly fires bullets'\nShooting enemies will grant you additional life regen");
		}

       private Vector2 newVect;

        public override void SetDefaults()
        {
            item.damage = 28;  
            item.ranged = true;   
            item.width = 65;     
            item.height = 21;    
            item.useTime = 9;
            item.useAnimation = 9;
            item.useStyle = 5;    
            item.noMelee = true; 
            item.knockBack = 2;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 5;
			item.UseSound = SoundID.Item36;
            item.autoReuse = true;
            item.shoot = 10; 
            item.shootSpeed = 15f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }	
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            {
                int p = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
                Main.projectile[p].GetGlobalProjectile<SpiritGlobalProjectile>(mod).shotFromCookieCutter = true;
                return false;
            }
            
            return false;
        }		
    }
}
