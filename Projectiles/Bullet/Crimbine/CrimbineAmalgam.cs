using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet.Crimbine
{
    public class CrimbineAmalgam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloody Amalgam");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.penetrate = 2;
            projectile.timeLeft = 300;
            projectile.height = 40;
            projectile.width = 40;
            aiType = ProjectileID.Bullet;

        }

        int timer = 1;
        public override void AI()
        {
            projectile.velocity *= .9994f;
            var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
            foreach (var proj in list)
            {
                if (projectile != proj && proj.type == mod.ProjectileType("CrimbineBone"))
                {
                    Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 95);
                    proj.Kill();
                    projectile.Kill();
                    {
                        int n = Main.rand.Next(14, 17);
                        for (int i = 0; i < n; i++)
                        {
                            float rotation = MathHelper.ToRadians(270 / n * i);
                            Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(rotation);
                            perturbedSpeed.Normalize();
                            perturbedSpeed.X *= Main.rand.NextFloat(5.5f, 7.5f);
                            perturbedSpeed.Y *= Main.rand.NextFloat(8.5f, 10.5f);
                            if (Main.rand.Next(10) == 0)
                            {
                                projType = mod.ProjectileType("CrimbineSpine");

                            }
                            else if (Main.rand.Next(8) == 0)
                            {
                                projType = mod.ProjectileType("CrimbineHeart");

                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                projType = mod.ProjectileType("CrimbineBlob");

                            }
                            int newProj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, projType, projectile.damage/5 * 6, 2, projectile.owner);
                        }
                    }
                }
            }
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            projectile.ai[1] += 1f;
            if (projectile.ai[1] >= 7200f)
            {
                projectile.alpha += 5;
                if (projectile.alpha > 255)
                {
                    projectile.alpha = 255;
                    projectile.Kill();
                }
            }

            projectile.localAI[0] += 1f;
            if (projectile.localAI[0] >= 10f)
            {
                projectile.localAI[0] = 0f;
                int num416 = 0;
                int num417 = 0;
                float num418 = 0f;
                int num419 = projectile.type;
                for (int num420 = 0; num420 < 1000; num420++)
                {
                    if (Main.projectile[num420].active && Main.projectile[num420].owner == projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f)
                    {
                        num416++;
                        if (Main.projectile[num420].ai[1] > num418)
                        {
                            num417 = num420;
                            num418 = Main.projectile[num420].ai[1];
                        }
                    }
                }
                if (num416 > 1)
                {
                    Main.projectile[num417].netUpdate = true;
                    Main.projectile[num417].ai[1] = 36000f;
                    return;
                }
            }
            int num = 5;
            for (int k = 0; k < Main.rand.Next(6, 11); k++)
            {
                int index2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 5, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index2].scale = Main.rand.NextFloat(.85f, 1.1f);
                Main.dust[index2].velocity *= 0f;
                Main.dust[index2].alpha = 100;
                Main.dust[index2].noGravity = false;
                Main.dust[index2].noLight = false;
            }

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            {
                int d = 0;
                for (int k = 0; k < 6; k++)
                {
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
                }

                Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
                Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
                Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
                Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
            }
            return true;
        }
        int projType;
        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 26; k++)
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 5, projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);
            }

        }
    }
}
