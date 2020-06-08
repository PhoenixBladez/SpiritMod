using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
    class MagicBlaze : ModProjectile
    {
        public static int _type;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Fiery Blaze");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
            ProjectileID.Sets.TrailingMode[projectile.type] = 1;
        }

        public override void SetDefaults() {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.penetrate = 4;
            projectile.timeLeft = 500;
            projectile.height = 18;
            projectile.width = 10;
            projectile.alpha = 50;
            projectile.CloneDefaults(ProjectileID.Bullet);
            projectile.extraUpdates = 1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            projectile.penetrate--;
            if(projectile.penetrate <= 0) {
                projectile.Kill();
            } else {
                projectile.ai[0] += 0.1f;
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
        int counter;
        public override void AI() {
            Lighting.AddLight(projectile.position, 0.4f, .12f, .036f);
            counter++;
            if(counter >= 1440) {
                counter = -1440;
            }
            for(int i = 0; i < 4; i++) {
                float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;

                int num2121 = Dust.NewDust(projectile.Center + new Vector2(0, (float)Math.Cos(counter / 4.2f) * 9.2f).RotatedBy(projectile.rotation), 6, 6, 6, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num2121].velocity *= 0f;
                Main.dust[num2121].scale *= .75f;
                Main.dust[num2121].noGravity = true;

            }

            projectile.localAI[0] += 1f;
            if(projectile.localAI[0] == 12f) {
                projectile.localAI[0] = 0f;
                for(int j = 0; j < 12; j++) {
                    Vector2 vector2 = Vector2.UnitX * -projectile.width / 2f;
                    vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default(Vector2)) * new Vector2(8f, 16f);
                    vector2 = Utils.RotatedBy(vector2, (projectile.rotation - 1.57079637f), default(Vector2));
                    int num8 = Dust.NewDust(projectile.Center, 0, 0, 6, 0f, 0f, 160, default(Color), 1f);
                    Main.dust[num8].scale = 1.1f;
                    Main.dust[num8].noGravity = true;
                    Main.dust[num8].position = projectile.Center + vector2;
                    Main.dust[num8].velocity = projectile.velocity * 0.1f;
                    Main.dust[num8].velocity = Vector2.Normalize(projectile.Center - projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(6) == 2)
                target.AddBuff(BuffID.OnFire, 180);
            if(Main.rand.Next(4) == 0) {
                int n = 4;
                int deviation = Main.rand.Next(0, 300);
                for(int i = 0; i < n; i++) {
                    float rotation = MathHelper.ToRadians(270 / n * i + deviation);
                    Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(rotation);
                    perturbedSpeed.Normalize();
                    perturbedSpeed.X *= 2.5f;
                    perturbedSpeed.Y *= 2.5f;
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, 504, projectile.damage / 2, 2, projectile.owner);
                }
            }
        }
        public override void Kill(int timeLeft) {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 74);
            ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
                delegate {
                    for(int i = 0; i < 40; i++) {
                        int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, -2f, 0, default(Color), 1.2f);
                        Main.dust[num].noGravity = true;
                        Dust expr_62_cp_0 = Main.dust[num];
                        expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
                        Dust expr_92_cp_0 = Main.dust[num];
                        expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
                        if(Main.dust[num].position != projectile.Center) {
                            Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
                        }
                    }
                });
            for(int num625 = 0; num625 < 2; num625++) {
                float scaleFactor10 = 0.33f;
                if(num625 == 1)
                    scaleFactor10 = 0.66f;

                if(num625 == 2)
                    scaleFactor10 = 1f;

                int num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Gore expr_13AB6_cp_0 = Main.gore[num626];
                expr_13AB6_cp_0.velocity.X = expr_13AB6_cp_0.velocity.X + 1f;
                Gore expr_13AD6_cp_0 = Main.gore[num626];
                expr_13AD6_cp_0.velocity.Y = expr_13AD6_cp_0.velocity.Y + 1f;
                num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Gore expr_13B79_cp_0 = Main.gore[num626];
                expr_13B79_cp_0.velocity.X = expr_13B79_cp_0.velocity.X - 1f;
                Gore expr_13B99_cp_0 = Main.gore[num626];
                expr_13B99_cp_0.velocity.Y = expr_13B99_cp_0.velocity.Y + 1f;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for(int k = 0; k < projectile.oldPos.Length; k++) {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}
