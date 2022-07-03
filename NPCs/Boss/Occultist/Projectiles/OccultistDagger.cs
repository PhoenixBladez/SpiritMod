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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.timeLeft = 240;
			Projectile.hostile = true;
			Projectile.height = 12;
			Projectile.width = 12;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.scale = Main.rand.NextFloat(0.6f, 0.8f);
			Projectile.penetrate = -1;
		}

		private ref float Angle => ref Projectile.ai[0];
		private ref float AiState => ref Projectile.ai[1];
		private const float STATE_FADEIN = 0;
		private const float STATE_DRAWBACK = 1;
		private const float STATE_FLYING = 2;
		private const float STATE_FADEOUT = 3;

		private const int FADEINTIME = 15;
		private const int DRAWBACKTIME = 20;
		private const int FLYINGTIME = 70;
		private const int FADEOUTTIME = 15;

		private ref float AiTimer => ref Projectile.localAI[0];

		private float alphaCounter;
		public override void AI()
		{
			alphaCounter += 0.08f;
			Projectile.rotation = Angle + MathHelper.PiOver2;
			switch (AiState)
			{
				case STATE_FADEIN:
					Projectile.alpha = Math.Max(Projectile.alpha - 255 / FADEINTIME, 0);
					if (AiTimer > FADEINTIME)
					{
						AiState = STATE_DRAWBACK;
						AiTimer = 0;
						Projectile.netUpdate = true;
					}
					break;

				case STATE_DRAWBACK:
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, -Angle.ToRotationVector2() * 3, 0.1f);
					if (AiTimer > DRAWBACKTIME)
					{
						AiState = STATE_FLYING;
						Projectile.velocity = (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * 10;
						if (!Main.dedServ)
						{
							for (int i = -1; i <= 1; i += 2)
							{
								Vector2 velocity = Projectile.velocity.RotatedBy(MathHelper.Pi + i * MathHelper.Pi / 8).RotatedByRandom(MathHelper.Pi / 16) * 0.5f;
								ParticleHandler.SpawnParticle(new ImpactLine(Projectile.Center + Projectile.velocity * 2, velocity, Color.LightPink, new Vector2(0.33f, 0.75f), 10));
							}
						}
						AiTimer = 0;
						Projectile.netUpdate = true;
					}
					break;

				case STATE_FLYING:
					Projectile.tileCollide = true;
					if (Projectile.velocity.Length() < 20)
						Projectile.velocity *= 1.03f;

					if (AiTimer > FLYINGTIME)
					{
						AiTimer = 0;
						AiState = STATE_FADEOUT;
						Projectile.netUpdate = true;
					}
					break;

				case STATE_FADEOUT:
					Projectile.tileCollide = false;
					Projectile.alpha += 255 / FADEOUTTIME;
					Projectile.velocity *= 0.95f;
					if (Projectile.alpha >= 255)
						Projectile.Kill();
					break;
			}

			++AiTimer;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.oldPos[ProjectileID.Sets.TrailCacheLength[Projectile.type] - 1] + Projectile.Size / 2))
				return true;
			return base.Colliding(projHitbox, targetHitbox);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.rotation);
			writer.Write(AiTimer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.rotation = reader.ReadSingle();
			AiTimer = reader.ReadSingle();
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (AiState == STATE_FLYING)
				AiState = STATE_FADEOUT;

			Projectile.velocity = oldVelocity;
			return false;
		}

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => AiState == STATE_FLYING;

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			float sineAdd = (float)Math.Sin(alphaCounter) / 4f + 0.75f;
			{
				for (int k = 0; k < Projectile.oldPos.Length; k++)
				{
					Color color = new Color(255, 179, 246) * 0.75f * Projectile.Opacity * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

					float scale = Projectile.scale;
					Texture2D glowtex = ModContent.Request<Texture2D>(Texture + "_trail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
					spriteBatch.Draw(glowtex, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, null, color * sineAdd, Projectile.rotation, glowtex.Size() / 2, scale, default, default);
				}
			}

			Projectile.QuickDraw(spriteBatch, drawColor: Color.White);

			Texture2D bloom = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			float bloomOpacity = Math.Max((1 - Projectile.Opacity) * 3, 0.5f) * Projectile.Opacity;
			spriteBatch.Draw(bloom, Projectile.Center - Main.screenPosition, null, Color.Pink * bloomOpacity, Projectile.rotation, bloom.Size() / 2, new Vector2(Projectile.scale / 4f, Projectile.scale / 2f), SpriteEffects.None, 0f);
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}
}