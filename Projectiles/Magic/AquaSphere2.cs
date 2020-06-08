using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
    public class AquaSphere2 : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Aqua Sphere");
        }

        public override void SetDefaults() {
            projectile.width = 12;
            projectile.height = 12;
            projectile.hide = true;
            projectile.friendly = true;
            projectile.penetrate = 2;
            projectile.timeLeft = 90;
            projectile.tileCollide = false;
        }

        public override void Kill(int timeLeft) {
            for(int i = 0; i < 2; i++) {
                int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 187);
                Main.dust[d].noGravity = true;
            }
        }

        public override void AI() {
            projectile.velocity *= 0.93f;

            for(int i = 1; i <= 3; i++) {
                int num1 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
                    187, projectile.velocity.X, projectile.velocity.Y, 0, default(Color), 1f);
                Main.dust[num1].noGravity = true;
                Main.dust[num1].velocity *= 0.1f;
            }
            Lighting.AddLight(projectile.position, 0.1f, 0.2f, 0.3f);
        }

    }
}