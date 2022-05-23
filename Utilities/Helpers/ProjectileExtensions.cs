using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod
{
	public static class ProjectileExtensions
	{
		public static void Bounce(this Projectile projectile, Vector2 oldVelocity, float VelocityKeptRatio = 1f) => projectile.velocity = new Vector2((projectile.velocity.X == oldVelocity.X) ? projectile.velocity.X : -oldVelocity.X * VelocityKeptRatio, (projectile.velocity.Y == oldVelocity.Y) ? projectile.velocity.Y : -oldVelocity.Y * VelocityKeptRatio);

		public static Rectangle DrawFrame(this Projectile projectile)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			return new Rectangle(0, projectile.frame * texture.Height / Main.projFrames[projectile.type], texture.Width, texture.Height / Main.projFrames[projectile.type]);
		}

		public static void QuickDraw(this Projectile projectile, SpriteBatch spriteBatch, float? rotation = null, SpriteEffects? spriteEffects = null, Color? drawColor = null, Vector2? drawOrigin = null)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Color color = drawColor ?? Lighting.GetColor((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16);
			if(spriteEffects == null)
				spriteEffects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, projectile.DrawFrame(), projectile.GetAlpha(color), rotation ?? projectile.rotation, 
				drawOrigin ?? projectile.DrawFrame().Size() / 2, projectile.scale, spriteEffects ?? (projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0);
		}

		public static void QuickDrawGlow(this Projectile projectile, SpriteBatch spriteBatch, Color? color = null, float? rotation = null, SpriteEffects? spriteEffects = null, Vector2? drawOrigin = null)
		{
			if (!ModContent.TextureExists(projectile.modProjectile.Texture + "_glow"))
				return;

			Texture2D tex = ModContent.GetTexture(projectile.modProjectile.Texture + "_glow");
			Rectangle frame = new Rectangle(0, projectile.frame * tex.Height / Main.projFrames[projectile.type], tex.Width, tex.Height / Main.projFrames[projectile.type]);
			if (spriteEffects == null)
				spriteEffects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, frame, color ?? Color.White, rotation ?? projectile.rotation, drawOrigin ?? frame.Size() / 2, projectile.scale, 
				spriteEffects ?? (projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0);
		}

		public static void QuickDrawTrail(this Projectile projectile, SpriteBatch spriteBatch, float baseOpacity = 0.5f, float? rotation = null, SpriteEffects? spriteEffects = null, Color? drawColor = null, Vector2? drawOrigin = null)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Color color = drawColor ?? Lighting.GetColor((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16);
			if (spriteEffects == null)
				spriteEffects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i++)
			{
				float opacityMod = (ProjectileID.Sets.TrailCacheLength[projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[projectile.type];
				opacityMod *= baseOpacity;
				Vector2 drawPosition = projectile.oldPos[i] + (projectile.Size / 2) - Main.screenPosition;
				spriteBatch.Draw(tex, drawPosition, projectile.DrawFrame(), projectile.GetAlpha(color) * opacityMod, 
					rotation ?? projectile.oldRot[i], drawOrigin ?? projectile.DrawFrame().Size() / 2, projectile.scale, 
					spriteEffects ?? (projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0);
			}
		}

		public static void QuickDrawGlowTrail(this Projectile projectile, SpriteBatch spriteBatch, float Opacity = 0.5f, Color? color = null, float? rotation = null, SpriteEffects? spriteEffects = null, Vector2? drawOrigin = null)
		{
			if (!ModContent.TextureExists(projectile.modProjectile.Texture + "_glow"))
				return;

			Texture2D tex = ModContent.GetTexture(projectile.modProjectile.Texture + "_glow");
			Rectangle frame = new Rectangle(0, projectile.frame * tex.Height / Main.projFrames[projectile.type], tex.Width, tex.Height / Main.projFrames[projectile.type]);
			if (spriteEffects == null)
				spriteEffects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i++)
			{
				float opacity = (ProjectileID.Sets.TrailCacheLength[projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[projectile.type];
				opacity *= Opacity;
				spriteBatch.Draw(tex, projectile.oldPos[i] + projectile.Size / 2 - Main.screenPosition, frame, (color ?? Color.White) * opacity, rotation ?? projectile.oldRot[i], 
					drawOrigin ?? frame.Size() / 2, projectile.scale, spriteEffects ?? (projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0);
			}
		}

		public static void UpdateFrame(this Projectile projectile, int framespersecond, int loopFrame = 0)
		{
			if (framespersecond == 0)
				return;

			projectile.frameCounter++;
			if (projectile.frameCounter > (60 / framespersecond))
			{
				projectile.frameCounter = 0;
				projectile.frame++;
				if (projectile.frame >= Main.projFrames[projectile.type])
					projectile.frame = loopFrame;

			}
		}

		public static bool CheckSolidTilesAndPlatforms(Rectangle range)
		{
			int startX = range.X;
			int endX = range.X + range.Width;

			int startY = range.Y;
			int endY = range.Y + range.Height;

			if (Collision.SolidTiles(startX, endX, startY, endY))
				return true;

			for(int x = startX; x <= endX; x++)
			{
				for(int y = startY; y <= endY; y++)
				{
					if (TileID.Sets.Platforms[Framing.GetTileSafely(new Point(x, y)).type])
						return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Tries to move an entity to a given spot, using linear acceleration, and multiplicative inverse decceleration.
		/// </summary>
		/// <param name="ent"></param>
		/// <param name="desiredPosition"></param>
		/// <param name="accelSpeed"></param>
		/// <param name="deccelSpeed"></param>
		/// <param name="maxSpeed"></param>
		public static void AccelFlyingMovement(this Entity ent, Vector2 desiredPosition, Vector2 accelSpeed, Vector2 deccelSpeed, Vector2? maxSpeed = null)
		{
			if(ent.Center.X != desiredPosition.X)
				ent.velocity.X = (ent.Center.X < desiredPosition.X) ? 
					((ent.velocity.X < 0) ? ent.velocity.X / (deccelSpeed.X + 1) : ent.velocity.X) + accelSpeed.X :
					((ent.velocity.X > 0) ? ent.velocity.X / (deccelSpeed.X + 1) : ent.velocity.X) - accelSpeed.X;

			if(ent.Center.Y != desiredPosition.Y)
				ent.velocity.Y = (ent.Center.Y < desiredPosition.Y) ?
					((ent.velocity.Y < 0) ? ent.velocity.Y / (deccelSpeed.Y + 1) : ent.velocity.Y) + accelSpeed.Y :
					((ent.velocity.Y > 0) ? ent.velocity.Y / (deccelSpeed.Y + 1) : ent.velocity.Y) - accelSpeed.Y;

			if (maxSpeed != null)
				ent.velocity = new Vector2(MathHelper.Clamp(ent.velocity.X, -maxSpeed.Value.X, maxSpeed.Value.X), MathHelper.Clamp(ent.velocity.Y, -maxSpeed.Value.Y, maxSpeed.Value.Y));
		}

		public static void AccelFlyingMovement(this Entity ent, Vector2 desiredPosition, float accelSpeed, float deccelSpeed, float maxSpeed = -1)
		{
			if(maxSpeed >= 0)
				AccelFlyingMovement(ent, desiredPosition, new Vector2(accelSpeed), new Vector2(deccelSpeed), new Vector2(maxSpeed));

			else
				AccelFlyingMovement(ent, desiredPosition, new Vector2(accelSpeed), new Vector2(deccelSpeed));
		}
	}
}
