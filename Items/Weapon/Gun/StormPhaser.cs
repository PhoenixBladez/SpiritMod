using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
    public class StormPhaser : ModItem
    {private Vector2 newVect;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Storm Phaser");
			Tooltip.SetDefault("Celestially powerful");
		}


        public override void SetDefaults()
        {
            item.damage = 36;
            item.ranged = true;   
            item.width = 65;     
            item.height = 21;    
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 5;    
            item.noMelee = true; 
            item.knockBack = 6;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 20, 0, 0);
            item.rare = 9;
            item.UseSound = SoundID.Item36;
            item.autoReuse = true;
            item.shoot = 10; 
            item.shootSpeed = 8f;
            item.useAmmo = AmmoID.Bullet;
        }
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Vector2 origVect = new Vector2(speedX, speedY);
			for (int X = 0; X <= 7; X++)
			{
				if (Main.rand.Next(2) == 1)
				{
					newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(72, 1800) / 10));
				}
				else
				{
					newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(72, 1800) / 10));
				}
			Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

    }
}