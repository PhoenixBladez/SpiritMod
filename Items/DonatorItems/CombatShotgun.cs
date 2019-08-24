using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
    public class CombatShotgun : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Combat Shotgun");
			Tooltip.SetDefault("'Rip and Tear'\nShoots a spread of bullets\nRight-click to shoot a grenade\n~Donator Item~");
		}

        
        public override void SetDefaults()
        {
            item.damage = 28;
            item.ranged = true;
            item.width = 65;
            item.height = 21;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 4;
            item.value = 10780;
            item.rare = 5;
            item.UseSound = SoundID.Item36;
            item.autoReuse = false;
            item.shoot = 10;
            item.shootSpeed = 12f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ProjectileID.GrenadeI, (int)(damage * 1.25), knockBack, player.whoAmI);
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
					Vector2 origVect = new Vector2(speedX, speedY);
					Vector2 velocity;
                    if (Main.rand.Next(2) == 1)
                    {
                        velocity = origVect.RotatedBy(Math.PI * (Main.rand.NextDouble() * .09));
                    }
                    else
                    {
                        velocity = origVect.RotatedBy(-Math.PI * (Main.rand.NextDouble() * .09));
                    }
                    velocity *= .75f + Main.rand.NextFloat(.5f);
                    int proj2 = Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockBack, player.whoAmI);
                    Projectile newProj2 = Main.projectile[proj2];
                }
                return false;
            }
            return false;
        }
    }
}