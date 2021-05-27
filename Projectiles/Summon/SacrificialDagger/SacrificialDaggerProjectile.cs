using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Weapon.Magic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.SacrificialDagger
{
	public class SacrificialDaggerProjectile : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sacrificial Dagger");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			Main.projFrames[projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.minion = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 300;
			projectile.height = 12;
			projectile.width = 12;
			projectile.tileCollide = true;
		}
		public override void Kill(int timeLeft)
		{			
            Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y, 1);
            Vector2 vector9 = projectile.position;
            Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
            vector9 += value19 * 12f;
            for (int num257 = 0; num257 < 18; num257++)
            {
                int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, 173, 0f, 0f, 0, Color.White, 1f);
                Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
                Main.dust[newDust].velocity += value19 * 2f;
                Main.dust[newDust].velocity *= 0.5f;
                Main.dust[newDust].noGravity = true;
                vector9 -= value19 * 8f;
            }
		}
        float alphaCounter;
		public override void AI()
        {
            alphaCounter += 0.08f;
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
            			
			projectile.frameCounter++;
			if (projectile.frameCounter >= 5) {
				projectile.frameCounter = 0;
				projectile.frame = (projectile.frame + 1) % 4;
			}
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * .75f;
		}
		public void AdditiveCall(SpriteBatch spriteBatch)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 2f;
            {
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    Color color = new Color(255, 179, 246) * 0.85f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                    float scale = projectile.scale;
                    Texture2D tex = ModContent.GetTexture("SpiritMod/Projectiles/Summon/SacrificialDagger/SacrificialDagger_Trail");

                    spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color * sineAdd, projectile.rotation, tex.Size() / 2, scale, default, default);
                }
            }
        }
	}
}
