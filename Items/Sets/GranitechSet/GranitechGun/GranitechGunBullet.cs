using Terraria;
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
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
			Main.projFrames[projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.width = 12;
			projectile.height = 12;
			projectile.alpha = 255;
			projectile.timeLeft = 900;
			projectile.aiStyle = -1;
			projectile.extraUpdates = 1;
			projectile.scale = Main.rand.NextFloat(1f, 1.3f);
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() - MathHelper.Pi;
			Lighting.AddLight(projectile.Center, Color.Yellow.ToVector3() / 2);
			if (spawnRings && projectile.ai[0]++ == 0 && !Main.dedServ)
			{
				int maxRings = 3;
				for (int j = -1; j <= 1; j++) //repeat multiple times with different offset and color, for chromatic aberration effect
				{
					Vector2 posOffset = Vector2.Normalize(projectile.velocity).RotatedBy(j * MathHelper.PiOver2) * 1f;
					Color colorMod = (j == -1) ? new Color(255, 0, 0, 100) : ((j == 0) ? new Color(0, 255, 0, 100) : new Color(0, 0, 255, 100));

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

						Vector2 velNormal = Vector2.Normalize(projectile.velocity);
						Vector2 spawnPos = Vector2.Lerp(projectile.Center, projectile.Center + Vector2.Normalize(projectile.velocity) * 90, progress) + posOffset;
						ParticleHandler.SpawnParticle(new PulseCircle(spawnPos, color * 0.4f, scale * 100, 20, PulseCircle.MovementType.OutwardsSquareRooted)
						{
							Angle = projectile.velocity.ToRotation(),
							ZRotation = 0.6f,
							RingColor = color,
							Velocity = velNormal * speed
						});
					}
				}
			}

			//accelerate over time, with a cap
			const float maxSpeed = 18f;
			if (projectile.velocity.Length() < maxSpeed)
				projectile.velocity *= 1.02f;
			else
				projectile.velocity = Vector2.Normalize(projectile.velocity) * maxSpeed;

			//randomize the aberration offset, at random intervals
			if (Main.rand.NextBool(10))
				aberrationOffset = Main.rand.NextFloat(1, 2f);

			//fade in quickly
			projectile.alpha = (int)MathHelper.Max(projectile.alpha - 10, 0);
		}

		public override void Kill(int timeLeft)
		{
			//Main.PlaySound(SoundID.Item10, projectile.Center);
			Main.PlaySound(SpiritMod.instance.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/EnergyImpact").WithPitchVariance(0.1f).WithVolume(0.35f), projectile.Center);

			if (!Main.dedServ)
			{
				ParticleHandler.SpawnParticle(new GranitechGunBurst(projectile.Center, Main.rand.NextFloat(0.9f, 1.1f), projectile.oldVelocity.ToRotation()));

				for (int i = 0; i < 4; ++i)
				{
					Vector2 vel = Vector2.Normalize(projectile.oldVelocity).RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(6f, 10f);
					ParticleHandler.SpawnParticle(new GranitechGunParticle(projectile.Center, vel, Main.rand.NextFloat(3f, 3.5f), 22, Main.rand.Next(2, 5)));
				}
			}

			for (int i = 0; i < 4; ++i)
			{
				float speed = Main.rand.NextFloat(3, 5.5f);
				var d = Dust.NewDustPerfect(projectile.Center, ModContent.DustType<GranitechGunDust>(), new Vector2(speed, 0).RotatedByRandom(MathHelper.Pi) + (projectile.velocity * 0.4f), 0, default);
				GranitechGunDust.RandomizeFrame(d);
				d.scale = speed / 4f;
				d.fadeIn = speed / 4f;
			}

			spawnRings = false;
		}

		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			Texture2D glow = mod.GetTexture("Items/Sets/GranitechSet/GranitechGun/GranitechGunBullet_Glow");

			//draw 3 times, with position and color offsets, for a chromatic aberration effect
			for (int j = 1; j >= -1; j--)
			{
				Vector2 posOffset = Vector2.Normalize(projectile.velocity).RotatedBy(j * MathHelper.PiOver2) * aberrationOffset;
				Color colorMod = (j == -1) ? new Color(255, 0, 0, 70) : ((j == 0) ? new Color(0, 255, 0, 70) : new Color(0, 0, 255, 70));

				Vector2 drawPosition = projectile.Center + posOffset - Main.screenPosition;
				for (int i = 1; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i++)
				{
					float progress = (ProjectileID.Sets.TrailCacheLength[projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[projectile.type];
					Color trailColor = Color.Lerp(new Color(222, 111, 127), new Color(178, 105, 140), progress).MultiplyRGBA(colorMod) * progress;
					Vector2 trailPosition = projectile.oldPos[i] + posOffset + projectile.Size / 2 - Main.screenPosition;
					sb.Draw(glow, trailPosition, null, projectile.GetAlpha(trailColor), projectile.rotation, glow.Size() / 2, projectile.scale, SpriteEffects.None, 0f);
				}

				sb.Draw(Main.projectileTexture[projectile.type], drawPosition, null, projectile.GetAlpha(colorMod), projectile.rotation, glow.Size() / 2, projectile.scale, SpriteEffects.None, 0f);
			}

			return false;
		}

		public void AdditiveCall(SpriteBatch sb)
		{
			Texture2D bloomTex = mod.GetTexture("Effects/Masks/CircleGradient");

			//trail of blooms at the positions of each bullet texture
			for (int i = 1; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i++)
			{
				float progress = (ProjectileID.Sets.TrailCacheLength[projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[projectile.type];
				Color trailColor = Color.Lerp(new Color(222, 111, 127), new Color(178, 105, 140), progress) * progress;
				Vector2 trailPosition = projectile.oldPos[i] + projectile.Size / 2 - Main.screenPosition;
				sb.Draw(bloomTex, trailPosition, null, projectile.GetAlpha(trailColor) * 0.66f, projectile.rotation, bloomTex.Size() / 2f, new Vector2(0.22f, 0.17f) * projectile.scale, SpriteEffects.None, 0);
			}

			sb.Draw(bloomTex, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(new Color(239, 241, 80)) * 0.33f, projectile.rotation, bloomTex.Size() / 2f,
				new Vector2(0.22f, 0.17f) * projectile.scale, SpriteEffects.None, 0);
		}

		//public void DoTrailCreation(TrailManager tManager) { }// tManager.CreateTrail(projectile, new GranitechBulletTrail(new Color(255, 46, 122, 0)), new RoundCap(), new DefaultTrailPosition(), 4, 220);

		//internal class GranitechBulletTrail : ITrailColor
		//{
		//	private Color _colour;

		//	public GranitechBulletTrail(Color colour)
		//	{
		//		_colour = colour;
		//	}

		//	public Color GetColourAt(float distanceFromStart, float trailLength, List<Vector2> points) => _colour;
		//}
	}
}