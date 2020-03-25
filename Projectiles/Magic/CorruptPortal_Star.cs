using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class CorruptPortal_Star : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shooting Star");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
            projectile.CloneDefaults(ProjectileID.Shuriken);
            projectile.width = 16;
			projectile.height = 16;
			projectile.friendly = true;
            projectile.scale *= .8f;
			projectile.penetrate = 2;
            projectile.alpha = 60;
			projectile.timeLeft = 150;
			projectile.tileCollide = true;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.penetrate--;
            if (projectile.penetrate <= 0)
                projectile.Kill();
            else
            {
                aiType = ProjectileID.Shuriken;
                if (projectile.velocity.X != oldVelocity.X)
                {
                    projectile.velocity.X = -oldVelocity.X;
                }
                if (projectile.velocity.Y != oldVelocity.Y)
                {
                    projectile.velocity.Y = -oldVelocity.Y;
                }
                projectile.velocity *= 0.75f;
            }
            return false;
        }
        public override void AI()
		{
            int num = 5;
            for (int k = 0; k < 6; k++)
            {
                int index2 = Dust.NewDust(projectile.position, 4, projectile.height, 173, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
                Main.dust[index2].scale = .8f;
                Main.dust[index2].velocity *= 0f;
                Main.dust[index2].noGravity = true;
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
            return false;
        }
    }
}