
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class ScattergunPellet : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scattergun Pellet");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 13;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 120;
			projectile.height = 6;
			projectile.width = 6;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}

		int timer = 0;
        bool chosen;
        int dustVer;
        int dustType;
		public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Vector2 mouse = Main.MouseWorld;
			if (!chosen)
            {
                dustVer = Main.rand.Next(0, 2);
                chosen = true;
            }
			if (dustVer == 0)
            {
                dustType = 226;
            }
			else
            {
                dustType = 272;
            }
			timer++;
			if (timer == 48)
            {
                projectile.velocity.Y *= -1;
				if (dustVer == 0)
                {
                    dustVer = 1;
                }
                else
                {
                    dustVer = 0;
                }
            }
            for (int i = 0; i < 3; i++) {
                float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
                int num = Dust.NewDust(new Vector2(x, y), 2, 2, dustType);
				Main.dust[num].alpha = projectile.alpha;
				Main.dust[num].velocity *= 0f;
				Main.dust[num].noGravity = true;
                Main.dust[num].fadeIn = 0.4684f;
                Main.dust[num].scale *= .1235f;
			}
            if (Main.rand.NextBool(3))
            {
                for (int i = 0; i < 1; i++)
                {
                    float x1 = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                    float y1 = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
                    int num1 = Dust.NewDust(new Vector2(x1, y1), 2, 2, dustType);
                    Main.dust[num1].alpha = projectile.alpha;
                    Main.dust[num1].velocity = projectile.velocity;
                    Main.dust[num1].fadeIn = 0.4684f;
                    Main.dust[num1].noGravity = true;
                    Main.dust[num1].scale *= .1235f;
                }
            }
        }
        Color color;
        public void AdditiveCall(SpriteBatch spriteBatch)
        {
            {
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    if (dustVer == 0)
                    {
                        color = new Color(179, 237, 255) * 0.65f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                    }
					else
                    {
                        color = new Color(240, 199, 255) * 0.65f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                    }

                    float scale = projectile.scale;
                    Texture2D tex = ModContent.GetTexture("SpiritMod/Projectiles/Bullet/ScattergunPellet");

                    spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
                   
                }
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
            return false;
        }
        public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.NPCHit, projectile.position, 3);
		}
	}
}
