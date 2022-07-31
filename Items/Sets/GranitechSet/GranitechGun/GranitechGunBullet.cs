using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using SpiritMod.Utilities;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpiritMod.Particles;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader.Config;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechGun
{
	public class GranitechGunBullet : ModProjectile, IDrawAdditive //, ITrailProjectile
	{
		public bool spawnRings = false;
		private float aberrationOffset = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granitech Bullet");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Main.projFrames[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.alpha = 255;
			Projectile.timeLeft = 900;
			Projectile.aiStyle = -1;
			Projectile.extraUpdates = 1;
			Projectile.scale = Main.rand.NextFloat(1f, 1.3f);
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.Pi;
			Lighting.AddLight(Projectile.Center, Color.Yellow.ToVector3() / 2);
			if (spawnRings && Projectile.ai[0]++ == 0 && !Main.dedServ)
			{
				int maxRings = 3;
				DrawAberration.DrawChromaticAberration(Vector2.Normalize(Projectile.velocity), 1, delegate (Vector2 offset, Color colorMod)
				{
					for (int i = 0; i < maxRings; i++) //multiple rings
					{
						float progress = i / (float)maxRings;
						Color color = Color.Lerp(new Color(222, 111, 127), new Color(239, 241, 80), progress).MultiplyRGBA(colorMod);

						float scale = (i == 0) ? 0.5f : //small
							(i == 1) ? 0.75f : //med
							1f; //big

						float speed = (i == 0) ? 3.5f :
							(i == 1) ? 1.8f :
							0.5f;

						Vector2 velNormal = Vector2.Normalize(Projectile.velocity);
						Vector2 spawnPos = Vector2.Lerp(Projectile.Center, Projectile.Center + Vector2.Normalize(Projectile.velocity) * 90, progress) + offset;
						ParticleHandler.SpawnParticle(new PulseCircle(spawnPos, color * 0.4f, scale * 100, 20, PulseCircle.MovementType.OutwardsSquareRooted)
						{
							Angle = Projectile.velocity.ToRotation(),
							ZRotation = 0.6f,
							RingColor = color,
							Velocity = velNormal * speed
						});
					}
				});
			}

			//accelerate over time, with a cap
			const float maxSpeed = 18f;
			if (Projectile.velocity.Length() < maxSpeed)
				Projectile.velocity *= 1.02f;
			else
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * maxSpeed;

			//randomize the aberration offset, at random intervals
			if (Main.rand.NextBool(10))
				aberrationOffset = Main.rand.NextFloat(1, 2f);

			//fade in quickly
			Projectile.alpha = (int)MathHelper.Max(Projectile.alpha - 10, 0);
		}

		public override void Kill(int timeLeft)
		{
			//Main.PlaySound(SoundID.Item10, projectile.Center);

			if (!Main.dedServ)
			{
				SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/EnergyImpact") with { PitchVariance = 0.1f, Volume = 0.66f}, Projectile.Center);

				ParticleHandler.SpawnParticle(new GranitechGunBurst(Projectile.Center, Main.rand.NextFloat(0.9f, 1.1f), Projectile.oldVelocity.ToRotation()));

				for (int i = 0; i < 4; ++i)
				{
					Vector2 vel = Vector2.Normalize(Projectile.oldVelocity).RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(6f, 10f) + (Projectile.velocity * 0.8f);
					ParticleHandler.SpawnParticle(new GranitechParticle(Projectile.Center, vel, Main.rand.NextBool(2) ? new Color(255, 56, 85) : new Color(239, 241, 80), Main.rand.NextFloat(3f, 3.5f), 22));
				}

				for (int i = 0; i < 4; ++i)
				{
					Vector2 vel = Vector2.Normalize(Projectile.oldVelocity).RotatedByRandom(MathHelper.ToRadians(180)) * Main.rand.NextFloat(6f, 10f) + (Projectile.velocity * 0.4f);
					ParticleHandler.SpawnParticle(new GranitechParticle(Projectile.Center, vel, Main.rand.NextBool(2) ? new Color(255, 56, 85) : new Color(239, 241, 80), Main.rand.NextFloat(3f, 3.5f), 22));
				}
			}

			/*for (int i = 0; i < 4; ++i)
			{
				float speed = Main.rand.NextFloat(3, 5.5f);
				var d = Dust.NewDustPerfect(projectile.Center, ModContent.DustType<GranitechGunDust>(), new Vector2(speed, 0).RotatedByRandom(MathHelper.Pi) + (projectile.velocity * 0.4f), 0, default);
				GranitechGunDust.RandomizeFrame(d);
				d.scale = speed / 4f;
				d.fadeIn = speed / 4f;
			}*/

			spawnRings = false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D glow = Mod.Assets.Request<Texture2D>("Items/Sets/GranitechSet/GranitechGun/GranitechGunBullet_Glow").Value;

			DrawAberration.DrawChromaticAberration(Vector2.Normalize(Projectile.velocity), aberrationOffset, delegate (Vector2 offset, Color colorMod)
			{
				Vector2 drawPosition = Projectile.Center + offset - Main.screenPosition;
				for (int i = 1; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
				{
					float progress = (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
					Color trailColor = Color.Lerp(new Color(222, 111, 127), new Color(178, 105, 140), progress).MultiplyRGBA(colorMod) * progress;
					Vector2 trailPosition = Projectile.oldPos[i] + offset + Projectile.Size / 2 - Main.screenPosition;
					Main.spriteBatch.Draw(glow, trailPosition, null, Projectile.GetAlpha(trailColor), Projectile.rotation, glow.Size() / 2, Projectile.scale, SpriteEffects.None, 0f);
				}

				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPosition, null, Projectile.GetAlpha(colorMod), Projectile.rotation, glow.Size() / 2, Projectile.scale, SpriteEffects.None, 0f);
			});

			return false;
		}

		public void AdditiveCall(SpriteBatch sb, Vector2 screenPos)
		{
			Texture2D bloomTex = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value;

			//trail of blooms at the positions of each bullet texture
			for (int i = 1; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
			{
				float progress = (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
				Color trailColor = Color.Lerp(new Color(222, 111, 127), new Color(178, 105, 140), progress) * progress;
				Vector2 trailPosition = Projectile.oldPos[i] + Projectile.Size / 2 - screenPos;
				sb.Draw(bloomTex, trailPosition, null, Projectile.GetAlpha(trailColor) * 0.66f, Projectile.rotation, bloomTex.Size() / 2f, new Vector2(0.22f, 0.17f) * Projectile.scale, SpriteEffects.None, 0);
			}

			sb.Draw(bloomTex, Projectile.Center - screenPos, null, Projectile.GetAlpha(new Color(239, 241, 80)) * 0.33f, Projectile.rotation, bloomTex.Size() / 2f,
				new Vector2(0.22f, 0.17f) * Projectile.scale, SpriteEffects.None, 0);
		}
	}
}