using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod
{
	public static class ExtraUtils
	{
		public static void Bounce(this Projectile projectile, Vector2 oldVelocity, float VelocityKeptRatio = 1f) => projectile.velocity = new Vector2((projectile.velocity.X == oldVelocity.X) ? projectile.velocity.X : -oldVelocity.X * VelocityKeptRatio, (projectile.velocity.Y == oldVelocity.Y) ? projectile.velocity.Y : -oldVelocity.Y * VelocityKeptRatio);

		public static Rectangle DrawFrame(this Projectile projectile)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			return new Rectangle(0, projectile.frame * texture.Height / Main.projFrames[projectile.type], texture.Width, texture.Height / Main.projFrames[projectile.type]);
		}

		public static void QuickDraw(this Projectile projectile, SpriteBatch spriteBatch, float? rotation = null, SpriteEffects? spriteEffects = null, Color? drawColor = null)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Color color = drawColor ?? Lighting.GetColor((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16);
			if(spriteEffects == null)
				spriteEffects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, projectile.DrawFrame(), projectile.GetAlpha(color), rotation ?? projectile.rotation, projectile.DrawFrame().Size() / 2, projectile.scale, spriteEffects ?? (projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0);
		}

		public static void QuickDrawGlow(this Projectile projectile, SpriteBatch spriteBatch, Color? color = null, float? rotation = null, SpriteEffects? spriteEffects = null)
		{
			if (!ModContent.TextureExists(projectile.modProjectile.Texture + "_glow"))
				return;

			Texture2D tex = ModContent.GetTexture(projectile.modProjectile.Texture + "_glow");
			if (spriteEffects == null)
				spriteEffects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, projectile.DrawFrame(),color ?? Color.White, rotation ?? projectile.rotation, projectile.DrawFrame().Size() / 2, projectile.scale, spriteEffects ?? (projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0);
		}

		public static void QuickDrawTrail(this Projectile projectile, SpriteBatch spriteBatch, float Opacity = 0.5f, float? rotation = null, SpriteEffects? spriteEffects = null, Color? drawColor = null)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Color color = drawColor ?? Lighting.GetColor((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16);
			if (spriteEffects == null)
				spriteEffects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i++)
			{
				float opacity = (ProjectileID.Sets.TrailCacheLength[projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[projectile.type];
				opacity *= Opacity;
				spriteBatch.Draw(tex, projectile.oldPos[i] + projectile.Size/2 - Main.screenPosition, projectile.DrawFrame(), projectile.GetAlpha(color) * opacity, rotation ?? projectile.oldRot[i], projectile.DrawFrame().Size() / 2, projectile.scale, spriteEffects ?? (projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0);
			}
		}

		public static void QuickDrawGlowTrail(this Projectile projectile, SpriteBatch spriteBatch, float Opacity = 0.5f, Color? color = null, float? rotation = null, SpriteEffects? spriteEffects = null)
		{
			if (!ModContent.TextureExists(projectile.modProjectile.Texture + "_glow"))
				return;

			Texture2D tex = ModContent.GetTexture(projectile.modProjectile.Texture + "_glow");
			if (spriteEffects == null)
				spriteEffects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i++)
			{
				float opacity = (ProjectileID.Sets.TrailCacheLength[projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[projectile.type];
				opacity *= Opacity;
				spriteBatch.Draw(tex, projectile.oldPos[i] + projectile.Size / 2 - Main.screenPosition, projectile.DrawFrame(), (color ?? Color.White) * opacity, rotation ?? projectile.oldRot[i], projectile.DrawFrame().Size() / 2, projectile.scale, spriteEffects ?? (projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0);
			}
		}
	}
}
