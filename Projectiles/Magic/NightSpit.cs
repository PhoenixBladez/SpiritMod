using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
    public class NightSpit : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Night Grasp");
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults() {
            projectile.friendly = true;
            projectile.magic = true;
            projectile.width = 10;
            projectile.height = 10;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.timeLeft = 180;
        }

        public override void AI() {
            for(int i = 0; i < 10; i++) {
                float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
                int num = Dust.NewDust(new Vector2(x, y), 26, 26, 70, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num].alpha = projectile.alpha;
                Main.dust[num].position.X = x;
                Main.dust[num].position.Y = y;
                Main.dust[num].velocity *= 0f;
                Main.dust[num].noGravity = true;
            }

            bool flag25 = false;
            int jim = 1;
            for(int index1 = 0; index1 < 200; index1++) {
                if(Main.npc[index1].CanBeChasedBy(projectile, false) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[index1].Center, 1, 1)) {
                    float num23 = Main.npc[index1].position.X + (float)(Main.npc[index1].width / 2);
                    float num24 = Main.npc[index1].position.Y + (float)(Main.npc[index1].height / 2);
                    float num25 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num23) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num24);
                    if(num25 < 500f) {
                        flag25 = true;
                        jim = index1;
                    }

                }
            }
            if(flag25) {


                float num1 = 10f;
                Vector2 vector2 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                float num2 = Main.npc[jim].Center.X - vector2.X;
                float num3 = Main.npc[jim].Center.Y - vector2.Y;
                float num4 = (float)Math.Sqrt((double)num2 * (double)num2 + (double)num3 * (double)num3);
                float num5 = num1 / num4;
                float num6 = num2 * num5;
                float num7 = num3 * num5;
                int num8 = 10;
                projectile.velocity.X = (projectile.velocity.X * (float)(num8 - 1) + num6) / (float)num8;
                projectile.velocity.Y = (projectile.velocity.Y * (float)(num8 - 1) + num7) / (float)num8;
            } else {
                projectile.velocity *= 0.95f;
            }
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

        private void AdjustMagnitude(ref Vector2 vector) {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if(magnitude > 6f)
                vector *= 6f / magnitude;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            projectile.Kill();
        }
    }
}

