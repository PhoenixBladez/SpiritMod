using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
    public class QuicksilverGun : ModItem

    {
        private Vector2 newVect;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quicksilver Purge");
			Tooltip.SetDefault("Shoots out three powerful quicksilver pins that latch onto enemies");
		}
        public override void SetDefaults()
        {
            item.damage = 74;  
            item.ranged = true;   
            item.width = 82;     
            item.height = 26;    
            item.useTime = 12;
            item.useAnimation = 36;
            item.reuseDelay = 24;
            item.useStyle = 5;    
            item.noMelee = true;
            item.knockBack = 7;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 8;
            item.crit = 4;
			//item.UseSound = SoundID.Item94;
            item.autoReuse = true;
            item.shoot = 89; 
            item.shootSpeed = 14f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("QuicksilverPin"), damage, knockBack, player.whoAmI);
			return false;
        }
		
    }
}
