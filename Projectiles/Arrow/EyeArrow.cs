using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
    public class EyeArrow : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Eye Arrow");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults() {
            projectile.CloneDefaults(ProjectileID.BoneArrow);
            projectile.damage = 14;
            projectile.extraUpdates = 1;
            aiType = ProjectileID.BoneArrow;
        }
        public override void AI() {
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("EyeDust1"), 0 - projectile.velocity.X, 0 - projectile.velocity.Y);
        }
        public override void Kill(int timeLeft) {
            for(int i = 0; i < 5; i++) {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 0);
            }
            Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
        }

    }
}
