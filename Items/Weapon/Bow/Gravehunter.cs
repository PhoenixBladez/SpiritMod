using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class Gravehunter : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravehunter");
			Tooltip.SetDefault("'You can't tell if you guide the trigger or it guides you'");
		}


        Vector2 newVect = Vector2.Zero;
        public override void SetDefaults()
        {
            item.damage = 50;
            item.noMelee = true;
            item.ranged = true;
            item.width = 24;
            item.height = 46;
            item.useTime = 6;
            item.useAnimation = 18;
            item.useStyle = 5;
            item.reuseDelay = 35;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.crit = 12;
            item.knockBack = 1;
            item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
            item.rare = 6;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shootSpeed = 6f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {

            Vector2 origVect = new Vector2(speedX, speedY);
            if (Main.rand.Next(2) == 1)
            {
                newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(140, 1800) / 10));
            }
            else
            {
                newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(140, 1800) / 10));
            }
            int proj2 = Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, type, damage, knockBack, player.whoAmI);
            return false;
        }

    }
}