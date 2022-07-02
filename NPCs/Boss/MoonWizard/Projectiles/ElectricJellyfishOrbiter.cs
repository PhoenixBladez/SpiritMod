using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
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
            Main.projFrames[Projectile.type] = 5;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 18;
			Projectile.height = 34;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.penetrate = 2;
            Projectile.hide = false;
			Projectile.timeLeft = 30000;
		}
        float alphaCounter;
        int num1;
		public override void AI()
        {
            var list = Main.projectile.Where(x1 => x1.Hitbox.Intersects(Projectile.Hitbox));
            foreach (var proj in list)
            {
                if (Projectile != proj && proj.friendly)
                {
                    Projectile.Kill();
                }
            }
            alphaCounter += .04f;
            Lighting.AddLight(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);
            Projectile.frameCounter++;
            Projectile.spriteDirection = -Projectile.direction;
            if (Projectile.timeLeft % 4 == 0)
            {
               
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
                Projectile.frameCounter = 0;
            }
            num1 = ModContent.NPCType<MoonjellyGiant>();
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
            float num2 = 60f;
            float x = 0.8f * Projectile.scale;
            float y = 0.5f * Projectile.scale;
            bool flag2 = false;

            if ((double)Projectile.ai[0] < (double)num2)
            {
                bool flag4 = true;
                int index1 = (int)Projectile.ai[1];
                if (Main.npc[index1].active && Main.npc[index1].type == num1)
                {
                    if (!flag2 && Main.npc[index1].oldPos[1] != Vector2.Zero)
                        Projectile.position = Projectile.position + Main.npc[index1].position - Main.npc[index1].oldPos[1];
                    if (Projectile.timeLeft % 3 == 0)
                    {
                        DustHelper.DrawElectricity(Projectile.Center + (Projectile.velocity * 4), Main.npc[index1].Center + (Main.npc[index1].velocity * 4), 226, 0.35f, 30, default, 0.12f);
                    }
                    if (Projectile.Distance(Main.npc[index1].Center) > 600)
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            Dust d = Dust.NewDustPerfect(Projectile.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(3), 0, default, 0.65f);
                            d.noGravity = true;
                        }
                        Projectile.position = Main.npc[index1].position + new Vector2(Main.rand.Next(-125, 126), Main.rand.Next(-125, 126));

                    }
                }
                else
                {
                    Projectile.ai[0] = num2;
                    Projectile.timeLeft = 300;
                    flag4 = false;
                }
                if (flag4 && !flag2)
                {
                    Projectile.velocity = Projectile.velocity + new Vector2((float)Math.Sign(Main.npc[index1].Center.X - Projectile.Center.X), (float)Math.Sign(Main.npc[index1].Center.Y - Projectile.Center.Y)) * new Vector2(x *.5f, y);
                }
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 lineStart = Projectile.Center;
            int rightValue = (int)Projectile.ai[0];
            float collisionpoint = 0f;
            if (rightValue < (double)Main.npc.Length && rightValue != -1)
            {
                NPC other = Main.npc[rightValue];
                Vector2 lineEnd = other.Center;
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), lineStart, lineEnd, Projectile.scale / 2, ref collisionpoint))
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
            SoundEngine.PlaySound(SoundID.NPCDeath28, Projectile.Center);
            for (int i = 0; i < 12; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(3), 0, default, 0.65f);
                d.noGravity = true;
            }
        }
	}
}
