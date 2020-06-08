using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Returning
{
    public class CoralBoomerang : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Hightide Boomerang");
        }

        public override void SetDefaults() {
            projectile.width = 28;
            projectile.height = 28;
            projectile.aiStyle = 3;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.magic = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 700;
            projectile.extraUpdates = 1;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            target.StrikeNPC(projectile.damage / 4, 0f, 0, crit);
            for(int k = 0; k < 20; k++) {
                Dust.NewDust(target.position, target.width, target.height, 225, 2.5f, -2.5f, 0, Color.White, 0.7f);
                Dust.NewDust(target.position, target.width, target.height, 225, 2.5f, -2.5f, 0, default(Color), .34f);
            }
        }
    }
}
