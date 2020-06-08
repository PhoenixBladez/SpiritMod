
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
    public class ShadowSingeProj : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Shadow Singe");
            Main.projFrames[projectile.type] = 7;
        }

        public override void SetDefaults() {
            projectile.width = 54;
            projectile.height = 54;
            projectile.friendly = true;
            projectile.timeLeft = 28;
            projectile.penetrate = 5;
        }

        public override bool PreAI() {
            projectile.frameCounter++;
            if(projectile.frameCounter > 4) {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if(projectile.frame > 6) {
                projectile.frame = 0;
            }
            projectile.alpha += 3;
            return false;
        }
    }
}
