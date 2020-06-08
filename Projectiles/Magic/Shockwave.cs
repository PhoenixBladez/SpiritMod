using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
    public class Shockwave : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Shockwave");
        }

        public override void SetDefaults() {
            projectile.hostile = false;
            projectile.magic = true;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = false;
            projectile.alpha = 255;
            projectile.penetrate = -1;
            projectile.timeLeft = 35;
            projectile.tileCollide = false;
        }

        int counter = -720;
        bool boom = false;
        private float distortStrength = 300f;
        public override bool PreAI() {
            if(!boom) {
                if(Main.netMode != NetmodeID.Server && !Filters.Scene["Shockwave"].IsActive()) {
                    Filters.Scene.Activate("Shockwave", projectile.Center).GetShader().UseColor(10, 5, 15).UseTargetPosition(projectile.Center);
                }
                boom = true;
            }
            if(Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive()) {
                float progress = (35f - projectile.timeLeft) / 60f; // Will range from -3 to 3, 0 being the point where the bomb explodes.
                Filters.Scene["Shockwave"].GetShader().UseProgress(progress).UseOpacity(distortStrength * (1 - progress / 3f));
            }
            return false;
        }
        public override void Kill(int timeLeft) {
            if(Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive()) {
                Filters.Scene["Shockwave"].Deactivate();
            }
        }
    }
}
