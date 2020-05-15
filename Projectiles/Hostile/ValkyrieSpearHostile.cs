using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
	public class ValkyrieSpearHostile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Valkyrie Spear");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 300;
			projectile.height = 40;
			projectile.width = 10;
			aiType = ProjectileID.DeathLaser;
		}
        float num;
        public override void AI()
        {
            num += .4f;
            projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
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
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255 - (int)num*5, 255 - (int)num*5, 255 - (int)num*5, 100 - (int)num*3);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 1);
            Vector2 vector9 = projectile.position;
            Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
            vector9 += value19 * 16f;
            for (int num257 = 0; num257 < 20; num257++)
            {
                int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, 263, 0f, 0f, 0, default(Color), 1f);
                Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
                Main.dust[newDust].velocity += value19 * 2f;
                Main.dust[newDust].velocity *= 0.5f;
                Main.dust[newDust].noGravity = true;
                vector9 -= value19 * 8f;
            }
        }
    }
}
