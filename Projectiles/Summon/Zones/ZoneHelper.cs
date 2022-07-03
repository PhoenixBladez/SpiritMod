using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.Zones
{
	public static class ZoneHelper
	{
		public static bool ZonePreDraw(Projectile projectile, Texture2D texture)
		{
			var drawOrigin = new Vector2(TextureAssets.Projectile[projectile.type].Value.Width * 0.5f, (projectile.height / Main.projFrames[projectile.type]) * 0.5f);

			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				const float Repeats = 4;

				float sine = (float)Math.Cos(Main.GlobalTimeWrappedHourly % 2.4f / 2.4f * MathHelper.TwoPi) / 2f + 0.5f;
				SpriteEffects effects = (projectile.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				Color drawCol = new Color(53 - projectile.alpha, 26 - projectile.alpha, 120 - projectile.alpha, 0).MultiplyRGBA(Color.LightBlue);
				Rectangle frame = TextureAssets.Projectile[projectile.type].Value.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame);

				for (int i = 0; i < Repeats; i++)
				{
					Color col = projectile.GetAlpha(drawCol);
					col *= 1f - sine;
					Vector2 rot = (i / Repeats * MathHelper.TwoPi + projectile.rotation).ToRotationVector2();
					Vector2 drawPos = projectile.Center - new Vector2(60) + drawOrigin + rot * (4f * sine + 2f) - Main.screenPosition + new Vector2(0, projectile.gfxOffY) - projectile.velocity * i;
					Main.spriteBatch.Draw(texture, drawPos, frame, col, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);
				}
			}
			return false;
		}

		public static void ZoneAdditiveDraw(SpriteBatch spriteBatch, Projectile projectile, Color col, string path)
		{
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color = col * 0.75f * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				float scale = projectile.scale;
				Texture2D tex = ModContent.Request<Texture2D>(path, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
			}
		}
	}
}
