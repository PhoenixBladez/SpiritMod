using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Spear
{
    public class Talonginus : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Talonginus");
			Tooltip.SetDefault("Extremely quick, but innacurate");
		}


        int currentHit;
        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.value = Item.sellPrice(0, 1, 30, 0);
            item.rare = 2;
            item.crit = 6;
            item.damage = 20;
            item.knockBack = 6f;
            item.useStyle = 5;
            item.useTime = 7;
            item.useAnimation = 7;
            item.melee = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.noUseGraphic = true;
            item.shoot = mod.ProjectileType("TalonginusProj");
            item.shootSpeed = 9f;
            item.UseSound = SoundID.Item1;
            this.currentHit = 0;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[item.shoot] > 0)
                return false;
            return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//create velocity vectors for the two angled projectiles (outwards at PI/15 radians)
			Vector2 origVect = new Vector2(speedX, speedY);
            Vector2 newVect = Vector2.Zero;
            if (Main.rand.Next(2) == 1)
				{
					newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(82, 1800) / 10));
				}
				else
				{
					newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(82, 1800) / 10));
				}
			speedX = newVect.X;
			speedY = newVect.Y;
            this.currentHit++;
            return true;
		}    
    }
}