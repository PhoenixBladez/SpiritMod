using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss
{
    public class TornadoTrace : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Tornado");
        }

        public override void SetDefaults() {
            projectile.hostile = false;
            projectile.width = 50;
            projectile.height = 2;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.penetrate = 8;
            projectile.alpha = 255;
            projectile.timeLeft = 60;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.extraUpdates = 1;
        }
    }
}