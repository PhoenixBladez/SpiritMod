using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Projectiles
{
	public class Cannonbubble : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 26;
			projectile.height = 26;
			projectile.ranged = true;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 300;
			projectile.aiStyle = 0;
		}

		public override void AI()
		{
			projectile.velocity.X *= 0.99f;
			projectile.velocity.Y += 0.034f;

			if (projectile.wet)
				projectile.velocity.Y -= 0.08f;

			projectile.scale = 1f + (float)Math.Sin(projectile.timeLeft * 0.02f) * 0.02f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}

		public override void Kill(int timeLeft)
		{
			int dustCount = Main.rand.Next(7, 12);

			for (int i = 0; i < dustCount; ++i)
			{
				Vector2 speed = new Vector2(Main.rand.NextFloat(4, 8) * projectile.velocity.Length() * 0.08f, 0).RotatedByRandom(MathHelper.TwoPi);
				Dust.NewDust(projectile.Center, 0, 0, DustID.BubbleBurst_Blue, speed.X, speed.Y, 0, default, Main.rand.NextFloat(0.75f, 1.5f));
			}
		}
	}
}
