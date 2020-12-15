using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs.MoonjellyEvent;
using System.Linq;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class ElectricJellyfishOrbiter : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arcane Energy");
            Main.projFrames[projectile.type] = 5;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.width = 18;
			projectile.height = 34;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.penetrate = 2;
            projectile.hide = false;
			projectile.timeLeft = 30000;
		}
        float alphaCounter;
        int num1;
		public override void AI()
        {
            var list = Main.projectile.Where(x1 => x1.Hitbox.Intersects(projectile.Hitbox));
            foreach (var proj in list)
            {
                if (projectile != proj && proj.friendly)
                {
                    projectile.Kill();
                }
            }
            alphaCounter += .04f;
            Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);
            projectile.frameCounter++;
            projectile.spriteDirection = -projectile.direction;
            if (projectile.timeLeft % 4 == 0)
            {
               
                projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
                projectile.frameCounter = 0;
            }
            num1 = ModContent.NPCType<MoonjellyGiant>();
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            float num2 = 60f;
            float x = 0.8f * projectile.scale;
            float y = 0.5f * projectile.scale;
            bool flag2 = false;

            if ((double)projectile.ai[0] < (double)num2)
            {
                bool flag4 = true;
                int index1 = (int)projectile.ai[1];
                if (Main.npc[index1].active && Main.npc[index1].type == num1)
                {
                    if (!flag2 && Main.npc[index1].oldPos[1] != Vector2.Zero)
                        projectile.position = projectile.position + Main.npc[index1].position - Main.npc[index1].oldPos[1];
                    if (projectile.timeLeft % 3 == 0)
                    {
                        DustHelper.DrawElectricity(projectile.Center + (projectile.velocity * 4), Main.npc[index1].Center + (Main.npc[index1].velocity * 4), 226, 0.35f, 30, default, 0.12f);
                    }
                    if (projectile.Distance(Main.npc[index1].Center) > 600)
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            Dust d = Dust.NewDustPerfect(projectile.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(3), 0, default, 0.65f);
                            d.noGravity = true;
                        }
                        projectile.position = Main.npc[index1].position + new Vector2(Main.rand.Next(-125, 126), Main.rand.Next(-125, 126));

                    }
                }
                else
                {
                    projectile.ai[0] = num2;
                    projectile.timeLeft = 300;
                    flag4 = false;
                }
                if (flag4 && !flag2)
                {
                    projectile.velocity = projectile.velocity + new Vector2((float)Math.Sign(Main.npc[index1].Center.X - projectile.Center.X), (float)Math.Sign(Main.npc[index1].Center.Y - projectile.Center.Y)) * new Vector2(x *.5f, y);
                }
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 lineStart = projectile.Center;
            int rightValue = (int)projectile.ai[0];
            float collisionpoint = 0f;
            if (rightValue < (double)Main.npc.Length && rightValue != -1)
            {
                NPC other = Main.npc[rightValue];
                Vector2 lineEnd = other.Center;
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), lineStart, lineEnd, projectile.scale / 2, ref collisionpoint))
                {
                    return true;
                }
            }
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(4, projectile.Center, 28);
            for (int i = 0; i < 12; i++)
            {
                Dust d = Dust.NewDustPerfect(projectile.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(3), 0, default, 0.65f);
                d.noGravity = true;
            }
        }
	}
}
