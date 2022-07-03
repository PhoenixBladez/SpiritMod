using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BowsMisc.GemBows.Ruby_Bow
{
	public class Ruby_Arrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ruby Arrow");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.arrow = true;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.arrow = true;
		}

		int bounces = 2;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			bounces--;
			if (bounces <= 0)
				Projectile.Kill();
			else {
				SoundEngine.PlaySound(SoundID.Shatter with { Volume = 0.4f });
				for (int index = 0; index < 5; ++index)
				{
					int i = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemRuby, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[i].noGravity = true;
				}				
				if (Projectile.velocity.X != oldVelocity.X)
					Projectile.velocity.X = -oldVelocity.X;

				if (Projectile.velocity.Y != oldVelocity.Y)
					Projectile.velocity.Y = -oldVelocity.Y;

				Projectile.velocity *= 0.825f;
			}
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects effect = Projectile.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Color col = Lighting.GetColor((int)(Projectile.Center.Y) / 16, (int)(Projectile.Center.Y) / 16);
			var basePos = Projectile.Center - Main.screenPosition + new Vector2(0.0f, Projectile.gfxOffY);

			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			int height = texture.Height / Main.projFrames[Projectile.type];
			var frame = new Rectangle(0, height * Projectile.frame, texture.Width, height);
			Vector2 origin = frame.Size() / 2f;
			int reps = 1;
			while (reps < 5)
			{
				col = Projectile.GetAlpha(Color.Lerp(col, Color.White, 2.5f));
				float num7 = 5 - reps;
				Color drawCol = col * (num7 / (ProjectileID.Sets.TrailCacheLength[Projectile.type] * 1.5f));
				Vector2 oldPo = Projectile.oldPos[reps];
				float rotation = Projectile.rotation;
				SpriteEffects effects2 = effect;
				if (ProjectileID.Sets.TrailingMode[Projectile.type] == 2)
				{
					rotation = Projectile.oldRot[reps];
					effects2 = Projectile.oldSpriteDirection[reps] == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				}
				Vector2 drawPos = oldPo + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
				Main.spriteBatch.Draw(texture, drawPos, frame, drawCol, rotation + Projectile.rotation * (reps - 1) * -effect.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt(), origin, MathHelper.Lerp(Projectile.scale, 3f, reps / 15f), effects2, 0.0f);
				reps++;
			}

			Main.spriteBatch.Draw(texture, basePos, frame, new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 175), Projectile.rotation, origin, Projectile.scale, effect, 0.0f);

			height = texture.Height / Main.projFrames[Projectile.type];
			frame = new Rectangle(0, height * Projectile.frame, texture.Width, height);
			origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float num99 = (float)(Math.Cos(Main.GlobalTimeWrappedHourly % 2.40000009536743 / 2.40000009536743 * MathHelper.TwoPi) / 4.0f + 0.5f);

			Color color2 = new Color(sbyte.MaxValue - Projectile.alpha, sbyte.MaxValue - Projectile.alpha, sbyte.MaxValue - Projectile.alpha, 0).MultiplyRGBA(Color.White);
			for (int i = 0; i < 4; ++i)
			{
				Color drawCol = Projectile.GetAlpha(color2) * (1f - num99);
				Vector2 offset = ((i / 4 * MathHelper.TwoPi) + Projectile.rotation).ToRotationVector2();
				Vector2 position2 = Projectile.Center + offset * (8.0f * num99 + 2.0f) - Main.screenPosition - texture.Size() * Projectile.scale / 2f + origin * Projectile.scale + new Vector2(0.0f, Projectile.gfxOffY);
				Main.spriteBatch.Draw(texture, position2, frame, drawCol, Projectile.rotation, origin, Projectile.scale, effect, 0.0f);
			}

			Lighting.AddLight(Projectile.Center, Color.Purple.ToVector3() / 2f);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int index = 0; index < 5; ++index)
			{
				int i = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemAmethyst, 0.0f, 0.0f, 0, Color.Purple, 1f);
				Main.dust[i].noGravity = true;
			}
			SoundEngine.PlaySound(SoundID.Shatter with { Volume = 0.4f });
		}
	}
}
