using Terraria.ModLoader;
using Terraria;
using System;

namespace SpiritMod.Projectiles.Returning
{
    public class ReachBoomerang : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Briarheart Boomerang");
        }

        public override void SetDefaults() {
            projectile.width = 22;
            projectile.height = 22;
            projectile.aiStyle = 3;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.magic = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 700;
        }
        public override void AI()
        {
            projectile.rotation += 0.1f;
            {
                int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 167, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
                Main.dust[dust2].velocity *= 0f;
                Main.dust[dust2].scale = .62f;
                Main.dust[dust2].noGravity = true;
            }
        }
    }
}
