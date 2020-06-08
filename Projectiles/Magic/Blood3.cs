using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
    public class Blood3 : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Blood Cluster");
        }

        public override void SetDefaults() {
            projectile.hostile = false;
            projectile.magic = true;
            projectile.CloneDefaults(ProjectileID.Shuriken);
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = 2;
            projectile.alpha = 255;
            projectile.timeLeft = 150;
            projectile.tileCollide = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity) {
            projectile.penetrate--;
            if(projectile.penetrate <= 0)
                projectile.Kill();
            else {
                aiType = ProjectileID.Shuriken;
                if(projectile.velocity.X != oldVelocity.X) {
                    projectile.velocity.X = -oldVelocity.X;
                }
                if(projectile.velocity.Y != oldVelocity.Y) {
                    projectile.velocity.Y = -oldVelocity.Y;
                }
                projectile.velocity *= 0.75f;
            }
            return false;
        }
        public override void AI() {
            int num = 5;
            for(int k = 0; k < 6; k++) {
                int index2 = Dust.NewDust(projectile.position, 4, projectile.height, 5, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
                Main.dust[index2].scale = .8f;
                Main.dust[index2].velocity *= 0f;
                Main.dust[index2].noGravity = true;
                Main.dust[index2].noLight = false;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            Player player = Main.player[projectile.owner];
            if(Main.rand.Next(18) <= 9 && player.statLife != player.statLifeMax2) {
                int lifeToHeal = 0;

                if(player.statLife + 3 <= player.statLifeMax2)
                    lifeToHeal = 5;
                else
                    lifeToHeal = player.statLifeMax2 - player.statLife;

                player.statLife += lifeToHeal;
                player.HealEffect(lifeToHeal);
            }
        }
    }
}