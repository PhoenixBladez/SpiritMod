using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
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
			projectile.width = 14;
			projectile.height = 18;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.tileCollide = true;
			projectile.timeLeft = 170;
			projectile.netUpdate = true;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.570796f;
			Lighting.AddLight(projectile.Center, color.ToVector3() / 2);
			if (projectile.timeLeft < 120)
			{
				if (!homing && projectile.owner == Main.myPlayer)
				{
					projectile.ai[0] = Main.MouseWorld.X;
					projectile.ai[1] = Main.MouseWorld.Y;
					Vector2 vector2_1 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
					Vector2 vector2_2 = Vector2.Normalize(vector2_1 - projectile.Center) * 14f;
					projectile.velocity = vector2_2;
					homing = true;
					projectile.netUpdate = true;
				}

				if (Vector2.Distance(projectile.Center, new Vector2(projectile.ai[0], projectile.ai[1])) <= 12.0)
					projectile.timeLeft = Math.Min(projectile.timeLeft, 5);
			}
			else
			{
				if (!speedCheck)
				{
					speed = Main.rand.NextFloat(0.9f, 0.95f);
					speedCheck = true;
				}
				projectile.velocity *= speed;
			}
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => damage += Math.Min(target.defense / 2, 4);

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = height = 8;
			return true;
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
			Vector2 spinPoint = new Vector2(0f, -3f).RotatedByRandom(MathHelper.Pi);
			float maxRepeats = 18 * projectile.scale;
			var vector2 = new Vector2(1.1f, 1f);
			for (float i = 0; i < maxRepeats; ++i)
			{
				int dustIndex = Dust.NewDust(projectile.Center, 0, 0, DustType, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[dustIndex].position = projectile.Center;
				Main.dust[dustIndex].velocity = spinPoint.RotatedBy(MathHelper.TwoPi * i / maxRepeats, new Vector2()) * vector2 * (0.8f + Main.rand.NextFloat() * 0.4f);
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].scale = 2f;
				Main.dust[dustIndex].fadeIn = Main.rand.NextFloat() * 2f;
				var dust = Dust.CloneDust(dustIndex);
				dust.scale /= 2f;
				dust.fadeIn /= 2f;
			}

			Main.PlaySound(SoundID.Trackable, (int)projectile.position.X, (int)projectile.position.Y, 165, 0.5f, 0.0f);
		}
	}
}
