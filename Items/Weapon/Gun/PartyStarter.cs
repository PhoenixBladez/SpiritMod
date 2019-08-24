using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
    public class PartyStarter : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Partystarter");
			Tooltip.SetDefault("'Let's get this party started! Number 999, baby!'\nConverts bullets into Party Bullets'");
		}


        public override void SetDefaults()
        {
            item.damage = 66;
            item.ranged = true;   
            item.width = 65;     
            item.height = 32;    
            item.useTime = 32;
            item.useAnimation = 32;
            item.useStyle = 5;    
            item.noMelee = true; 
            item.knockBack = 12;
            item.useTurn = false;
            item.value = Terraria.Item.buyPrice(0, 19, 99, 0);
            item.rare = 5;
            item.crit = 10;
            item.UseSound = SoundID.Item36;
            item.autoReuse = false;
            item.shoot = 11; 
            item.shootSpeed = 17f;
            item.useAmmo = AmmoID.Bullet;
        }
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Terraria.Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ProjectileID.PartyBullet, damage, knockBack, player.whoAmI, 0f, 0f);
            return false;

        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }
}