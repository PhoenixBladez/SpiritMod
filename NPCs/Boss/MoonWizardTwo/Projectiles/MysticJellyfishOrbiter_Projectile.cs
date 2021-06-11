using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.NPCs.Boss.MoonWizardTwo.Projectiles
{
	public class MysticJellyfishOrbiter_Projectile : ModProjectile, IDrawAdditive
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Star");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 7;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
            projectile.width = 12;
            projectile.height = 16;
            projectile.hostile = true;
			projectile.friendly = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 70;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.aiStyle = -1;
            projectile.scale = 1.5f;
			aiType = ProjectileID.Bullet;
		}
		int timer;
		int colortimer;
		public override bool PreAI()
		{
            projectile.velocity *= 1.01f;
            float num = 1f - (float)projectile.alpha / 255f;
            num *= projectile.scale;
            Lighting.AddLight(projectile.Center, 0.1f * num, 0.2f * num, 0.4f * num);
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;

            int num623 = Dust.NewDust(projectile.Center - projectile.velocity / 5, 4, 4, 180, 0f, 0f, 0, default(Color), 1.8f);
            Main.dust[num623].velocity = projectile.velocity;
            Main.dust[num623].noGravity = true;
			if (projectile.timeLeft < 55)
            {
                projectile.tileCollide = true;
            }

			/*if (projectile.timeLeft == 35)
			{
				NPC parent = Main.npc[(int)projectile.ai[0]];
				Vector2 direction = Main.player[parent.target].Center - projectile.Center;
				direction.Normalize();
				direction *= projectile.velocity.Length();
				projectile.velocity = direction;
			}*/

            Player player = Main.player[projectile.owner];
            Vector2 center = projectile.Center;
            float num8 = (float)player.miscCounter / 40f;
            float num7 = 1.0471975512f * 2;
            if (projectile.minion)
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        int num6 = Dust.NewDust(center, 0, 0, 180, 0f, 0f, 100, default(Color), .85f);
                        Main.dust[num6].noGravity = true;
                        Main.dust[num6].velocity = Vector2.Zero;
                        Main.dust[num6].noLight = true;
                        Main.dust[num6].position = center - projectile.velocity / 5 * (float)j + (num8 * 6.28318548f + num7 * (float)i).ToRotationVector2() * 9f;
                    }
                }
            }
            return true;
		}
		public override void Kill(int timeLeft)
		{
			Vector2 vector9 = projectile.position;
			Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 24; num257++) {
				int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, 180, 0f, 0f, 0, default(Color), 1.2f);
				Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
            if (projectile.minion)
            {
                ProjectileExtras.Explode(projectile.whoAmI, 60, 60, delegate
                {
                    Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 3));
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 180, 0f, -2f, 0, default(Color), 2f);
                            Main.dust[num].noGravity = true;
                            Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                            Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                            Main.dust[num].scale *= .25f;
                            if (Main.dust[num].position != projectile.Center)
                                Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
                        }
                    }
                    DustHelper.DrawDustImage(projectile.Center, 226, 0.15f, "SpiritMod/Effects/DustImages/MoonSigil", 1f);
                }, true);
            }
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
        public void AdditiveCall(SpriteBatch spriteBatch)
        {
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Color color = new Color(255, 255, 255) * 0.75f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                float scale = projectile.scale;
                Texture2D tex = ModContent.GetTexture("SpiritMod/NPCs/Boss/MoonWizardTwo/Projectiles/WizardBall_Projectile");

                spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
            }
        }
    }
}
