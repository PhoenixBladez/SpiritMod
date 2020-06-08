using Terraria.ModLoader;

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
            projectile.penetrate = 1;
            projectile.timeLeft = 700;
            projectile.extraUpdates = 2;
        }

    }
}
