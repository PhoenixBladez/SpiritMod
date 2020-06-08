using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
    public class ZephyrSpearProj : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Breath of the Zephyr");
        }

        public override void SetDefaults() {
            projectile.CloneDefaults(ProjectileID.Trident);
            aiType = ProjectileID.Trident;
        }
    }
}
