using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using SpiritMod.Particles;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SummonsMisc.FairyWhistle
{
	public class FairyProj : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fae Bolt");
			Main.projFrames[Projectile.type] = 4;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.Size = Vector2.One * 10;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.alpha = 255;
			Projectile.timeLeft = 60;
			Projectile.extraUpdates = 1;
			Projectile.hide = true;
			Projectile.scale = Main.rand.NextFloat(0.7f, 0.9f);
		}

		public override void AI()
		{
			Projectile.alpha = Math.Max(Projectile.alpha - 15, 0);
			Projectile.UpdateFrame(6);
			Lighting.AddLight(Projectile.Center, Color.LimeGreen.ToVector3() / 3);

			if(!Main.dedServ && Main.rand.NextBool(5)) //randomly moving subtle particle trail
				ParticleHandler.SpawnParticle(new FireParticle(Projectile.Center, (Vector2.Normalize(Projectile.velocity)/3) + Main.rand.NextVector2Unit() * Main.rand.NextFloat(2),
					new Color(120, 239, 255) * 0.66f, new Color(94, 255, 126) * 0.66f, Main.rand.NextFloat(0.2f, 0.3f), 40, delegate (Particle p)
					{
						p.Velocity = p.Velocity.RotatedByRandom(0.25f) * 0.98f;
					}));
		}

		public override void Kill(int timeLeft)
		{
			if (timeLeft <= 0)
				return;

			if (!Main.dedServ)
			{
				Vector2 velnormal = Vector2.Normalize(Projectile.velocity);
				velnormal *= 2;

				for (int i = 0; i < 3; i++) //weak burst of particles in direction of movement
					ParticleHandler.SpawnParticle(new FireParticle(Projectile.Center, velnormal.RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(2f),
						new Color(120, 239, 255), new Color(94, 255, 126), Main.rand.NextFloat(0.3f, 0.4f), 25, delegate (Particle p)
						{
							p.Velocity = p.Velocity.RotatedByRandom(0.1f) * 0.97f;
						}));

				for (int i = 0; i < 4; i++) //wide burst of slower moving particles in opposite direction
					ParticleHandler.SpawnParticle(new FireParticle(Projectile.Center, -velnormal.RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat(1.5f),
						new Color(120, 239, 255), new Color(94, 255, 126), Main.rand.NextFloat(0.3f, 0.4f), 25, delegate (Particle p)
						{
							p.Velocity = p.Velocity.RotatedByRandom(0.1f) * 0.97f;
						}));


				for (int i = 0; i < 3; i++) //narrow burst of faster, bigger particles
					ParticleHandler.SpawnParticle(new FireParticle(Projectile.Center, -velnormal.RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(2.5f),
						new Color(120, 239, 255), new Color(94, 255, 126), Main.rand.NextFloat(0.3f, 0.4f), 25, delegate (Particle p)
						{
							p.Velocity = p.Velocity.RotatedByRandom(0.15f) * 0.96f;
						}));
			}
		}

		public void AdditiveCall(SpriteBatch sB, Vector2 screenPos)
		{
			Texture2D bloom = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value;
			sB.Draw(bloom, Projectile.Center - Main.screenPosition, null, Color.Lerp(new Color(124, 255, 47), Color.White, 0.75f) * Projectile.Opacity, 0, bloom.Size() / 2, Projectile.scale * 0.15f, SpriteEffects.None, 0);
			Projectile.QuickDrawTrail(sB, 0.8f, drawColor: Color.White);
			Projectile.QuickDraw(sB, drawColor: Color.White);
		}
	}
}