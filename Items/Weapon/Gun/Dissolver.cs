using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
    public class Dissolver : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dissolver");
			Tooltip.SetDefault("Fires a burst of acid globs");
		}


		private Vector2 newVect;
        public override void SetDefaults()
        {
            item.damage = 66;  
            item.ranged = true;   
            item.width = 65;     
            item.height = 21;    
            item.useTime = 14;  
            item.useAnimation = 14;
            item.useStyle = 5;    
            item.noMelee = true; 
            item.knockBack = 4;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 20, 0, 0);
            item.rare = 10;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shootSpeed = 1f;
			item.shoot = mod.ProjectileType("AcidGlob");
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//create velocity vectors for the two angled projectiles (outwards at PI/15 radians)
			Vector2 origVect = new Vector2(speedX, speedY);
			for (int X = 0; X <= 5; X++)
			{
				if (Main.rand.Next(2) == 1)
				{
					newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(72, 1800) / 10));
				}
				else
				{
					newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(72, 1800) / 10));
				}
				newVect *= (Main.rand.Next(7,9));
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