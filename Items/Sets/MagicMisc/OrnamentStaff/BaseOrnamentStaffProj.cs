using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.OrnamentStaff
{
	public abstract class BaseOrnamentStaffProj : ModProjectile
	{
		internal bool homing = false;
		internal bool speedCheck = false;
		internal float speed = 0f;
		public int DustType;
		public Color color;

		public BaseOrnamentStaffProj(int DustType, Color color)
		{
			this.DustType = DustType;
			this.color = color;
		}

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 18;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 170;
			Projectile.netUpdate = true;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.570796f;
			Lighting.AddLight(Projectile.Center, color.ToVector3() / 2);
			if (Projectile.timeLeft < 120)
			{
				if (!homing && Projectile.owner == Main.myPlayer)
				{
					Projectile.ai[0] = Main.MouseWorld.X;
					Projectile.ai[1] = Main.MouseWorld.Y;
					Vector2 vector2_1 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
					Vector2 vector2_2 = Vector2.Normalize(vector2_1 - Projectile.Center) * 14f;
					Projectile.velocity = vector2_2;
					homing = true;
					Projectile.netUpdate = true;
				}

				if (Vector2.Distance(Projectile.Center, new Vector2(Projectile.ai[0], Projectile.ai[1])) <= 12.0)
					Projectile.timeLeft = Math.Min(Projectile.timeLeft, 5);
			}
			else
			{
				if (!speedCheck)
				{
					speed = Main.rand.NextFloat(0.9f, 0.95f);
					speedCheck = true;
				}
				Projectile.velocity *= speed;
			}
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => damage += Math.Min(target.defense / 2, 4);

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = height = 8;
			return true;
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
			Vector2 spinPoint = new Vector2(0f, -3f).RotatedByRandom(MathHelper.Pi);
			float maxRepeats = 18 * Projectile.scale;
			var vector2 = new Vector2(1.1f, 1f);
			for (float i = 0; i < maxRepeats; ++i)
			{
				int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, DustType, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[dustIndex].position = Projectile.Center;
				Main.dust[dustIndex].velocity = spinPoint.RotatedBy(MathHelper.TwoPi * i / maxRepeats, new Vector2()) * vector2 * (0.8f + Main.rand.NextFloat() * 0.4f);
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].scale = 2f;
				Main.dust[dustIndex].fadeIn = Main.rand.NextFloat() * 2f;
				var dust = Dust.CloneDust(dustIndex);
				dust.scale /= 2f;
				dust.fadeIn /= 2f;
			}

			SoundEngine.PlaySound(SoundID.Shatter, Projectile.Center);
		}
	}
}
