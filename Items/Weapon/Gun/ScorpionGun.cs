using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
    public class ScorpionGun : ModItem
    {
        private Vector2 newVect;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Poacher");
			Tooltip.SetDefault("Converts bullets into venomous bullets");
		}
        public override void SetDefaults()
        {
            item.damage = 32;  
            item.ranged = true;   
            item.width = 54;     
            item.height = 18;    
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = 5;    
            item.noMelee = true; 
            item.knockBack = 7;
            item.useTurn = false;
            item.value = Terraria.Item.buyPrice(0, 20, 0, 0);
            item.rare = 5;
            item.crit = 10;
			item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = 283; 
            item.shootSpeed = 13f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        { 
        {
            int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, 283, damage, knockBack, player.whoAmI);
        }
			return false;
        }
		
    }
}
