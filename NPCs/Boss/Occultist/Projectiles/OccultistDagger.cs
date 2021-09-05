using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod;
using SpiritMod.Particles;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Occultist.Projectiles
{
	public class OccultistDagger : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sacrificial Dagger");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.timeLeft = 240;
			projectile.hostile = true;
			projectile.height = 12;
			projectile.width = 12;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.scale = Main.rand.NextFloat(0.6f, 0.8f);
			projectile.penetrate = -1;
		}

		private ref float Angle => ref projectile.ai[0];
		private ref float AiState => ref projectile.ai[1];
		private const float STATE_FADEIN = 0;
		private const float STATE_DRAWBACK = 1;
		private const float STATE_FLYING = 2;
		private const float STATE_FADEOUT = 3;

		private const int FADEINTIME = 15;
		private const int DRAWBACKTIME = 20;
		private const int FLYINGTIME = 70;
		private const int FADEOUTTIME = 15;

		private ref float AiTimer => ref projectile.localAI[0];

		private float alphaCounter;
		public override void AI()
		{
			alphaCounter += 0.08f;
			projectile.rotation = Angle + MathHelper.PiOver2;
			switch (AiState)
			{
				case STATE_FADEIN:
					projectile.alpha = Math.Max(projectile.alpha - 255 / FADEINTIME, 0);
					if (AiTimer > FADEINTIME)
					{
						AiState = STATE_DRAWBACK;
						AiTimer = 0;
						projectile.netUpdate = true;
					}
					break;

				case STATE_DRAWBACK:
					projectile.velocity = Vector2.Lerp(projectile.velocity, -Angle.ToRotationVector2() * 3, 0.1f);
					if (AiTimer > DRAWBACKTIME)
					{
						AiState = STATE_FLYING;
						projectile.velocity = (projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * 10;
						if (!Main.dedServ)
						{
							for (int i = -1; i <= 1; i += 2)
							{
								Vector2 velocity = projectile.velocity.RotatedBy(MathHelper.Pi + i * MathHelper.Pi / 8).RotatedByRandom(MathHelper.Pi / 16) * 0.5f;
								ParticleHandler.SpawnParticle(new ImpactLine(projectile.Center + projectile.velocity * 2, velocity, Color.LightPink, new Vector2(0.33f, 0.75f), 10));
							}
						}
						AiTimer = 0;
						projectile.netUpdate = true;
					}
					break;

				case STATE_FLYING:
					projectile.tileCollide = true;
					if (projectile.velocity.Length() < 20)
						projectile.velocity *= 1.03f;

					if (AiTimer > FLYINGTIME)
					{
						AiTimer = 0;
						AiState = STATE_FADEOUT;
						projectile.netUpdate = true;
					}
					break;

				case STATE_FADEOUT:
					projectile.tileCollide = false;
					projectile.alpha += 255 / FADEOUTTIME;
					projectile.velocity *= 0.95f;
					if (projectile.alpha >= 255)
						projectile.Kill();
					break;
			}

			++AiTimer;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.oldPos[ProjectileID.Sets.TrailCacheLength[projectile.type] - 1] + projectile.Size / 2))
				return true;
			return base.Colliding(projHitbox, targetHitbox);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.rotation);
			writer.Write(AiTimer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.rotation = reader.ReadSingle();
			AiTimer = reader.ReadSingle();
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (AiState == STATE_FLYING)
				AiState = STATE_FADEOUT;

			projectile.velocity = oldVelocity;
			return false;
		}

		public override bool CanDamage() => AiState == STATE_FLYING;

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			float sineAdd = (float)Math.Sin(alphaCounter) / 4f + 0.75f;
			{
				for (int k = 0; k < projectile.oldPos.Length; k++)
				{
					Color color = new Color(255, 179, 246) * 0.75f * projectile.Opacity * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

					float scale = projectile.scale;
					Texture2D glowtex = ModContent.GetTexture(Texture + "_trail");
					spriteBatch.Draw(glowtex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color * sineAdd, projectile.rotation, glowtex.Size() / 2, scale, default, default);
				}
			}

			projectile.QuickDraw(spriteBatch, drawColor: Color.White);

			Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");
			float bloomOpacity = Math.Max((1 - projectile.Opacity) * 3, 0.5f) * projectile.Opacity;
			spriteBatch.Draw(bloom, projectile.Center - Main.screenPosition, null, Color.Pink * bloomOpacity, projectile.rotation, bloom.Size() / 2, new Vector2(projectile.scale / 4f, projectile.scale / 2f), SpriteEffects.None, 0f);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;
	}
}