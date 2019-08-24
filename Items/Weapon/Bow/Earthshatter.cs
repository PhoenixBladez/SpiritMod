using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class Earthshatter : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Earthshatter");
			Tooltip.SetDefault("Shoots out Earthen rocks along with arrows");
		}


        private Vector2 newVect;
        public override void SetDefaults()
        {
            item.damage = 51;
            item.noMelee = true;
            item.ranged = true;
            item.width = 20;
            item.height = 40;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = 5;
            item.shoot = 9;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 6;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 9;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shootSpeed = 18f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("Earthrock"), damage, knockBack, player.whoAmI);
            Projectile newProj = Main.projectile[proj];
            newProj.friendly = true;
            newProj.hostile = false;
            Vector2 origVect = new Vector2(speedX, speedY);
            for (int X = 0; X <= 2; X++)
            {
                if (Main.rand.Next(2) == 1)
                {
                    newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(82, 1800) / 10));
                }
                else
                {
                    newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(82, 1800) / 10));
                }
                int proj2 = Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, type, damage, knockBack, player.whoAmI);
                Projectile newProj2 = Main.projectile[proj2];
            }
            return false;
        }
    }
}