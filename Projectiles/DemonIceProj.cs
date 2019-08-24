using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Projectiles
{
    public class DemonIceProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Wave");

        }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 40;
			projectile.friendly = true;
			projectile.melee = true;
            projectile.timeLeft = 45;
            projectile.penetrate = 5;
            projectile.alpha = 255;

        }

        public override bool PreAI()
        {
                      int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 206, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            int dust1 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 68, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 172, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust1].noGravity = true;
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].scale = 2f;
            Main.dust[dust1].scale = 1.5f;
            Main.dust[dust].scale = 1.5f;
			            projectile.velocity.Y += 0.4F;
            projectile.velocity.X *= 1.005F;
            projectile.velocity.X = MathHelper.Clamp(projectile.velocity.X, -10, 10);
						return true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(2) == 0)
            {
                target.AddBuff(BuffID.Frostburn, 120, true);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}
