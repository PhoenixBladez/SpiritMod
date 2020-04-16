using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Projectiles.Thrown
{
	public class BrainProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Creeping Shruken");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 150;
			projectile.height = 36;
			projectile.width = 36;
            projectile.aiStyle = -1;
            projectile.thrown = true;
            projectile.ignoreWater = true;
			projectile.tileCollide = false;
		}

		public override void AI()
		{
            projectile.alpha += 5;
            if (projectile.alpha >= 200)
            {
                projectile.alpha = 200;
            }
            projectile.velocity = projectile.velocity.RotatedBy(System.Math.PI / 40);
            projectile.rotation += .3f;
            projectile.velocity *= 0.96f;
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
