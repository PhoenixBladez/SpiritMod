using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BowsMisc.GemBows.Topaz_Bow
{
	public class Topaz_Arrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Topaz Arrow");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 14; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.arrow = true;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.extraUpdates = 2;
			projectile.arrow = true;
		}
		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57F;
			projectile.velocity.Y += .02f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SpriteEffects effect = projectile.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Color col = Lighting.GetColor((int)(projectile.Center.Y) / 16, (int)(projectile.Center.Y) / 16);
			var basePos = projectile.Center - Main.screenPosition + new Vector2(0.0f, projectile.gfxOffY);

			Texture2D texture = Main.projectileTexture[projectile.type];

			int height = texture.Height / Main.projFrames[projectile.type];
			var frame = new Rectangle(0, height * projectile.frame, texture.Width, height);
			Vector2 origin = frame.Size() / 2f;
			int reps = 1;
			while (reps < 5)
			{
				col = projectile.GetAlpha(Color.Lerp(col, Color.White, 2.5f));
				float num7 = 5 - reps;
				Color drawCol = col * (num7 / (ProjectileID.Sets.TrailCacheLength[projectile.type] * 1.5f));
				Vector2 oldPo = projectile.oldPos[reps];
				float rotation = projectile.rotation;
				SpriteEffects effects2 = effect;
				if (ProjectileID.Sets.TrailingMode[projectile.type] == 2)
				{
					rotation = projectile.oldRot[reps];
					effects2 = projectile.oldSpriteDirection[reps] == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				}
				Vector2 drawPos = oldPo + projectile.Size / 2f - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
				Main.spriteBatch.Draw(texture, drawPos, frame, drawCol, rotation + projectile.rotation * (reps - 1) * -effect.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt(), origin, MathHelper.Lerp(projectile.scale, 3f, reps / 15f), effects2, 0.0f);
				reps++;
			}

			Main.spriteBatch.Draw(texture, basePos, frame, new Color(255 - projectile.alpha, 255 - projectile.alpha, 255 - projectile.alpha, 175), projectile.rotation, origin, projectile.scale, effect, 0.0f);

			height = texture.Height / Main.projFrames[projectile.type];
			frame = new Rectangle(0, height * projectile.frame, texture.Width, height);
			origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float num99 = (float)(Math.Cos(Main.GlobalTime % 2.40000009536743 / 2.40000009536743 * MathHelper.TwoPi) / 4.0f + 0.5f);

			Color color2 = new Color(sbyte.MaxValue - projectile.alpha, sbyte.MaxValue - projectile.alpha, sbyte.MaxValue - projectile.alpha, 0).MultiplyRGBA(Color.White);
			for (int i = 0; i < 4; ++i)
			{
				Color drawCol = projectile.GetAlpha(color2) * (1f - num99);
				Vector2 offset = ((i / 4 * MathHelper.TwoPi) + projectile.rotation).ToRotationVector2();
				Vector2 position2 = projectile.Center + offset * (8.0f * num99 + 2.0f) - Main.screenPosition - texture.Size() * projectile.scale / 2f + origin * projectile.scale + new Vector2(0.0f, projectile.gfxOffY);
				Main.spriteBatch.Draw(texture, position2, frame, drawCol, projectile.rotation, origin, projectile.scale, effect, 0.0f);
			}

			Lighting.AddLight(projectile.Center, Color.Purple.ToVector3() / 2f);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int index = 0; index < 5; ++index)
			{
				int i = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.AmethystBolt, 0.0f, 0.0f, 0, Color.Purple, 1f);
				Main.dust[i].noGravity = true;
			}
			Main.PlaySound(SoundID.Trackable, (int)projectile.position.X, (int)projectile.position.Y, 193, 1f, -0.2f);
		}
	}
}
