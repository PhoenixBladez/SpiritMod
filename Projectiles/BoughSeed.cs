using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
    public class BoughSeed : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Seed");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;

        }
        public override void SetDefaults()
        {
            projectile.width = 14;
                projectile.height = 20;
            projectile.thrown = true;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.light = 0;
			projectile.timeLeft = 300;
            projectile.extraUpdates = 1;
        }

        public override bool PreAI()
        {
            if (projectile.ai[0] == 0)
            {
                projectile.rotation = projectile.velocity.ToRotation() + 1.57F;
            }
            else
            {
                projectile.ignoreWater = true;
                projectile.tileCollide = false;
                int num996 = 1;
                bool flag52 = false;
                bool flag53 = false;
                projectile.localAI[0] += 1f;
                if (projectile.localAI[0] % 30f == 0f)
                {
                    flag53 = true;
                }
                int num997 = (int)projectile.ai[1];
                if (projectile.localAI[0] >= (float)(60 * num996))
                {
                    flag52 = true;
                }
                else if (num997 < 0 || num997 >= 300)
                {
                    flag52 = true;
                }
                else if (Main.npc[num997].active && !Main.npc[num997].dontTakeDamage)
                {
                    projectile.Center = Main.npc[num997].Center - projectile.velocity * 2f;
                    projectile.gfxOffY = Main.npc[num997].gfxOffY;
                    if (flag53)
                    {
                        Main.npc[num997].HitEffect(0, 1.0);
                    }
                }
                else
                {
                    flag52 = true;
                }
                if (flag52)
                {
                    projectile.Kill();
                }
            }

            return false;
        }
        public override void AI()
        {
            Vector2 targetPos = projectile.Center;
            float targetDist = 350f;
            bool targetAcquired = false;

            //loop through first 200 NPCs in Main.npc
            //this loop finds the closest valid target NPC within the range of targetDist pixels
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].CanBeChasedBy(projectile) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[i].Center, 1, 1))
                {
                    float dist = projectile.Distance(Main.npc[i].Center);
                    if (dist < targetDist)
                    {
                        targetDist = dist;
                        targetPos = Main.npc[i].Center;
                        targetAcquired = true;
                    }
                }
            }

            //change trajectory to home in on target
            if (targetAcquired)
            {
                float homingSpeedFactor = 6f;
                Vector2 homingVect = targetPos - projectile.Center;
                float dist = projectile.Distance(targetPos);
                dist = homingSpeedFactor / dist;
                homingVect *= dist;

                projectile.velocity = (projectile.velocity * 20 + homingVect) / 21f;
            }

            //Spawn the dust
            if (Main.rand.Next(11) == 0)
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 60, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            }
            projectile.rotation = projectile.velocity.ToRotation() + (float)(Math.PI / 2);
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
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.ai[0] = 1f;
            projectile.ai[1] = (float)target.whoAmI;
            projectile.velocity = (target.Center - projectile.Center) * 0.75f;
            projectile.netUpdate = true;
            {
                projectile.damage = 0;
            }
            knockback = 0;
            int num31 = 6;
            Point[] array2 = new Point[num31];
            int num32 = 0;

            for (int n = 0; n < 1000; n++)
            {
                if (n != projectile.whoAmI && Main.projectile[n].active && Main.projectile[n].owner == Main.myPlayer && Main.projectile[n].type == projectile.type && Main.projectile[n].ai[0] == 1f && Main.projectile[n].ai[1] == target.whoAmI)
                {
                    array2[num32++] = new Point(n, Main.projectile[n].timeLeft);
                    if (num32 >= array2.Length)
                    {
                        break;
                    }
                }
            }
            if (num32 >= array2.Length)
            {
                int num33 = 0;
                for (int num34 = 1; num34 < array2.Length; num34++)
                {
                    if (array2[num34].Y < array2[num33].Y)
                    {
                        num33 = num34;
                    }
                }
                Main.projectile[array2[num33].X].Kill();
            
           
            }
        }
        public override void Kill(int timeLeft)
        {
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
            {
				int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("Wrath"), 55, 7, Main.myPlayer);
         
          
                for (int num621 = 0; num621 < 40; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 60, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[num622].velocity *= 1.2f;
					Main.dust[num622].noGravity = true;
                    if (Main.rand.Next(2) == 0)
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
            }
            Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
        }
    }
}
