using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
    public class Blood : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Blood Clusters");
        }

        public override void SetDefaults() {
            projectile.hostile = false;
            projectile.magic = true;
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = 4;
            projectile.alpha = 255;
            projectile.extraUpdates = 2;
            projectile.timeLeft = 60;
        }
        int num2475;
        int counter;
        public override void AI() {

            projectile.ai[0] += 0.5f * projectile.direction;
            if(projectile.ai[0] > 20f || projectile.ai[0] < -20f) {
                projectile.Kill();
            }
            for(int num1438 = 0; num1438 < 2; num1438 = num2475 + 1) {
                Vector2 center22 = projectile.Center;
                projectile.scale = 1f - projectile.localAI[0];
                projectile.width = (int)(20f * projectile.scale);
                projectile.height = projectile.width;
                projectile.position.X = center22.X - (float)(projectile.width / 2);
                projectile.position.Y = center22.Y - (float)(projectile.height / 2);
                if((double)projectile.localAI[0] < 0.1) {
                    projectile.localAI[0] += 0.01f;
                } else {
                    projectile.localAI[0] += 0.025f;
                }
                if(projectile.localAI[0] >= 0.95f) {
                    projectile.Kill();
                }
                projectile.velocity.X = projectile.velocity.X + projectile.ai[0] * 1.5f;
                projectile.velocity.Y = projectile.velocity.Y + projectile.ai[1] * 1.5f * projectile.direction;
                if(projectile.velocity.Length() > 16f) {
                    projectile.velocity.Normalize();
                    projectile.velocity *= 16f;
                }
                projectile.ai[0] *= 1.05f;
                if(projectile.scale < 1f) {
                    int num1448 = 0;
                    while((float)num1448 < projectile.scale * 10f) {
                        Vector2 position177 = new Vector2(projectile.position.X, projectile.position.Y);
                        int width138 = projectile.width;
                        int height138 = projectile.height;
                        float x38 = projectile.velocity.X;
                        float y36 = projectile.velocity.Y;
                        Color newColor5 = default(Color);
                        int num1447 = Dust.NewDust(position177, width138, height138, 5, x38, y36, 100, newColor5, 1.1f);
                        Main.dust[num1447].position = (Main.dust[num1447].position + projectile.Center) / 2f;
                        Main.dust[num1447].noGravity = true;
                        Dust dust81 = Main.dust[num1447];
                        dust81.velocity *= 0.1f;
                        dust81 = Main.dust[num1447];
                        dust81.velocity -= projectile.velocity * (1.3f - projectile.scale);
                        Main.dust[num1447].fadeIn = (float)(100 + projectile.owner);
                        dust81 = Main.dust[num1447];
                        dust81.scale += projectile.scale * 0.45f;
                        num2475 = num1448;
                        num1448 = num2475 + 1;
                    }
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            Player player = Main.player[projectile.owner];
            if(crit && player.statLife != player.statLifeMax2) {
                int lifeToHeal = 0;

                if(player.statLife + 3 <= player.statLifeMax2)
                    lifeToHeal = 3;
                else
                    lifeToHeal = player.statLifeMax2 - player.statLife;

                player.statLife += lifeToHeal;
                player.HealEffect(lifeToHeal);
            }
        }
        public override void Kill(int timeLeft) {
            for(int i = 0; i < 10; i++) {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 5);
            }
        }
    }
}