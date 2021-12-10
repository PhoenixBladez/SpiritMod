using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Utilities;
using SpiritMod.Prim;
using SpiritMod.Particles;

namespace SpiritMod.Mechanics.BoonSystem.NemesisBoon
{
	public class NemesisBoonSword : ModProjectile
	{

		private const int SWINGDURATION = 30;

		private const float SWINGROTATION = 0.9f;

		private const int SWINGDISTANCE = 110;

		private const int NUMBEROFSWINGS = 3;
		private NPC parent => Main.npc[(int)projectile.ai[0]];

		private float swingSpeed = (SWINGROTATION * 2) / SWINGDURATION;

		private bool activated => projectile.ai[1] == 1;

		private float swingTimer = 0;

		private List<float> oldRotation = new List<float>();

		private float hoverCounter;

		private bool initialized = false;

		private bool swinging = false;

		private float swingWindup = 0;

		Vector2 swingDirection = Vector2.Zero;

		Player player;

		Vector2 swingBase = Vector2.Zero;

		int flashCounter = 0;

		float slashProgress = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sword of Nemesis");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}


		public override void SetDefaults()
		{
			projectile.width = projectile.height = 26;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = false;
		}

		public override void AI()
		{
			if (!initialized)
			{
				for (int i = 0; i < projectile.oldPos.Length; i++)
					oldRotation.Add(projectile.rotation);
			}
			Vector2 posToBe = Vector2.Zero;

			oldRotation.Add(projectile.rotation);
			while (oldRotation.Count > projectile.oldPos.Length)
			{
				oldRotation.RemoveAt(0);
			}
			if (activated)
			{
				if (!swinging)
				{
					swingWindup++;
					projectile.timeLeft = (int)(SWINGDURATION * NUMBEROFSWINGS / 2f) + 5;
					float newRot = swingDirection.ToRotation() + SWINGROTATION + 1.57f;

					float rotDif = ((((newRot - projectile.rotation) % 6.28f) + 9.42f) % 6.28f) - 3.14f;
					if (swingWindup > 60)
						swinging = true;

					if (Math.Abs(rotDif) > 0.1f)
						projectile.rotation += rotDif / 15f;
					projectile.rotation = MathHelper.Lerp(projectile.rotation, newRot, 0.1f);

					Vector2 dist1 = player.Center - swingBase;
					float velLength = dist1.Length();

					float offset = SWINGDISTANCE * Math.Min(swingWindup / 30f, 1);
					offset *= 2;
					projectile.velocity = swingDirection * (float)Math.Pow(Math.Abs(velLength - offset), 0.3f) * Math.Sign(velLength - offset);

					if (swingWindup > 30)
					{
						flashCounter++;
					}
					if (swingWindup == 30)
						Main.PlaySound(SoundID.NPCDeath7, projectile.Center);
				}
				else
				{
					projectile.hostile = true;

					swingTimer += swingSpeed;
					if (swingTimer > 1 || swingTimer < 0)
						swingSpeed *= -1;

					float progress = swingTimer;
					float oldProgress = EaseFunction.EaseCircularInOut.Ease(progress - swingSpeed);
					slashProgress = progress = EaseFunction.EaseCircularInOut.Ease(progress);

					projectile.velocity = swingDirection * 50 * (Math.Abs(oldProgress - progress));

					projectile.rotation = swingDirection.ToRotation() + MathHelper.Lerp(SWINGROTATION, -SWINGROTATION, progress) + 1.57f;

					if (Main.rand.Next(2) == 0)
					{
						int particleTimeleft = Main.rand.Next(30, 60);
						StarParticle particle = new StarParticle(
							projectile.Center + ((projectile.rotation - 1.57f).ToRotationVector2() * Main.rand.Next(70)) + Main.rand.NextVector2Circular(10, 10),
							Vector2.Zero,
							Color.SkyBlue,
							Main.rand.NextFloat(0.1f, 0.2f),
							particleTimeleft);

						particle.TimeActive = (uint)particleTimeleft / 2;
						ParticleHandler.SpawnParticle(particle);
					}
				}

				projectile.Center = swingBase + ((projectile.rotation - 1.57f).ToRotationVector2() * SWINGDISTANCE * Math.Min(swingWindup / 30f, 1));

				swingBase += projectile.velocity;
			}
			else
			{
				swingBase = projectile.Center;
				player = Main.player[parent.target];
				swingDirection = projectile.DirectionTo(player.Center);
				projectile.velocity = Vector2.Zero;
				hoverCounter += 0.05f;

				posToBe = parent.Center + new Vector2(parent.direction * (parent.width + (projectile.width / 2)) * -1, (float)Math.Sin(hoverCounter) * 12).RotatedBy(parent.rotation);
				if (!parent.active)
					projectile.active = false;


				Vector2 newPos = Vector2.Lerp(projectile.Center, posToBe, 0.1f);
				Vector2 tiltDirection = projectile.DirectionTo(newPos);
				projectile.rotation = tiltDirection.X * ((projectile.Center - newPos).Length() / 10f);
				projectile.Center = newPos;

			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				int particleTimeleft = Main.rand.Next(30, 60);
				StarParticle particle = new StarParticle(
					projectile.Center + ((projectile.rotation - 1.57f).ToRotationVector2() * Main.rand.Next(70)) + Main.rand.NextVector2Circular(10, 10),
					Vector2.Zero,
					Color.SkyBlue,
					Main.rand.NextFloat(0.1f, 0.2f),
					particleTimeleft);

				particle.TimeActive = (uint)particleTimeleft / 2;
				ParticleHandler.SpawnParticle(particle);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];

			Texture2D tex2 = ModContent.GetTexture(Texture + "_White");

			if (swinging)
			{
				spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				Effect effect = mod.GetEffect("Effects/NemesisBoonShader");

				List<PrimitiveSlashArc> slashArcs = new List<PrimitiveSlashArc>();
				PrimitiveSlashArc slash = new PrimitiveSlashArc
				{
					BasePosition = swingBase - Main.screenPosition,
					StartDistance = SWINGDISTANCE,
					EndDistance = SWINGDISTANCE + tex.Height,
					AngleRange = new Vector2(SWINGROTATION * Math.Sign(swingSpeed), -SWINGROTATION * Math.Sign(swingSpeed)),
					DirectionUnit = swingDirection,
					Color = Color.Cyan * projectile.velocity.Length() * 0.2f,
					SlashProgress = Math.Sign(swingSpeed) == 1 ? slashProgress: 1 - slashProgress
				};
				slashArcs.Add(slash);
				PrimitiveRenderer.DrawPrimitiveShapeBatched(slashArcs.ToArray(), effect);

				spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			}
			DrawSword(spriteBatch, tex, 1, 1);

			if (swingWindup < 60 && swingWindup > 30)
			{
				float progress = (swingWindup - 30) / 30f;
				float transparency = (float)Math.Pow(1 - progress, 2);
				float scale = 1 + progress;
				DrawSword(spriteBatch, tex2, transparency, scale);
			}

			return false;
		}

		private void DrawSword(SpriteBatch spriteBatch, Texture2D tex, float transparency, float scale)
		{
			Vector2 origin = new Vector2(tex.Width / 2, tex.Height);

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * 0.8f * transparency, projectile.rotation, origin, projectile.scale * scale, SpriteEffects.None, 0f);
			for (int k = projectile.oldPos.Length - 1; k > 0; k--)
			{
				Vector2 drawPos = projectile.oldPos[k] + (new Vector2(projectile.width, projectile.height) / 2);
				Color color = Color.White * (float)(((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length));
				float num108 = 4;
				float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
				float num106 = 0f;
				Color color29 = new Color(110 - projectile.alpha, 94 - projectile.alpha, 25 - projectile.alpha, 0).MultiplyRGBA(color);
				for (int num103 = 0; num103 < 4; num103++)
				{
					Color color28 = color29;
					color28 = projectile.GetAlpha(color28);
					color28 *= 1.5f - num107;
					color28 *= (float)Math.Pow((((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) / 2), 1.5f);
					Vector2 vector29 = drawPos + ((float)num103 / (float)num108 * 6.28318548f + projectile.rotation + num106).ToRotationVector2() * (1.5f * num107 + 2f) - Main.screenPosition + new Vector2(0, projectile.gfxOffY) - projectile.velocity * (float)num103;
					spriteBatch.Draw(tex, vector29, null, color28 * .6f * transparency, oldRotation[k], origin, projectile.scale * scale, SpriteEffects.None, 0f);
				}
			}
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}
