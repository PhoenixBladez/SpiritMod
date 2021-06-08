using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizardTwo.Projectiles
{
	public class MysticWizardBallSmall_Projectile : ModProjectile, IDrawAdditive
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Star");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 7;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
            projectile.width = 8;
            projectile.height = 8;
            projectile.hostile = true;
			projectile.friendly = false;
			projectile.penetrate = 2;
			projectile.timeLeft = 60;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.aiStyle = 1;
            projectile.scale = 1f;
			projectile.extraUpdates = 1;
			aiType = ProjectileID.Bullet;
		}
		public override bool PreAI()
        {
            float num = 1f - (float)projectile.alpha / 255f;
            num *= projectile.scale;
            Lighting.AddLight(projectile.Center, 0.1f * num, 0.2f * num, 0.4f * num);

            int num623 = Dust.NewDust(projectile.Center, 4, 4, 180, 0f, 0f, 0, default(Color), 1.8f);
            Main.dust[num623].velocity = projectile.velocity;
            Main.dust[num623].noGravity = true;

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
                Texture2D tex = ModContent.GetTexture("SpiritMod/NPCs/Boss/MoonWizardTwo/Projectiles/MysticWizardBall_Projectile");

                spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
            }
        }
    }
}
