using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using SpiritMod.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Occultist.Projectiles
{
	public class OccultistHand : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Grasp");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public int maxTimeLeft = 180;
		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.hostile = true;
			projectile.scale = Main.rand.NextFloat(0.12f, 0.2f);
			projectile.timeLeft = maxTimeLeft;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.spriteDirection = Main.rand.NextBool() ? -1 : 1;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(99, 23, 51) * 0.2f), new RoundCap(), new ArrowGlowPosition(), 200 * projectile.scale, 100);
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(99, 23, 51), new Color(181, 0, 116)), new NoCap(), new DefaultTrailPosition(), 150 * projectile.scale, 60, new ImageShader(mod.GetTexture("Textures/Trails/Trail_3"), 0.2f, 1f, 1f));
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(99, 23, 51), new Color(181, 0, 116)), new NoCap(), new DefaultTrailPosition(), 150 * projectile.scale, 60, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.2f, 1f, 1f));
		}

		public float period = 80;
		public ref float Amplitude => ref projectile.ai[0];
		private ref float PeriodOffset => ref projectile.ai[1];

		private float AiTimer => maxTimeLeft - projectile.timeLeft;
		public bool DoAcceleration = true;
		public bool TileCollideCheck = true;

		private readonly float acceleration = 1.01f;

		public override void AI()
		{
			projectile.position -= projectile.velocity;
			projectile.Size = Vector2.One * 20 * projectile.scale;

			Vector2 cosVel = projectile.velocity.RotatedBy(MathHelper.ToRadians(Amplitude) * (float)Math.Cos((AiTimer + PeriodOffset) / period * MathHelper.TwoPi));
			projectile.position += cosVel;
			projectile.rotation = cosVel.ToRotation() + MathHelper.PiOver2;

			if (AiTimer == 0 && !Main.dedServ)
			{
				ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, cosVel * 0.4f, new Color(99, 23, 51), 0.4f, 15, 3));
				ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, cosVel * 0.4f, new Color(99, 23, 51), 0.4f, 15, 4));

				for (int i = 0; i < 4; i++)
					ParticleHandler.SpawnParticle(new GlowParticle(projectile.Center, cosVel.RotatedByRandom(MathHelper.Pi / 20) * Main.rand.NextFloat(), Color.Magenta, Main.rand.NextFloat(0.02f, 0.04f), 30));
			}

			if (Main.rand.NextBool(10) && !Main.dedServ)
				ParticleHandler.SpawnParticle(new GlowParticle(projectile.Center, projectile.velocity * Main.rand.NextFloat(), Color.Magenta, Main.rand.NextFloat(0.02f, 0.04f), 30));

			if (projectile.alpha > 0)
			{
				projectile.alpha -= 25;
				projectile.scale *= 1.15f;
				projectile.velocity *= 1.15f;
			}
			else
				projectile.alpha = 0;

			if (AiTimer > maxTimeLeft / 3f)
			{
				if (TileCollideCheck)
					projectile.tileCollide = true;

				if (projectile.velocity.Length() < 20 && DoAcceleration)
					projectile.velocity *= acceleration;
			}
		}

		public override void Kill(int timeLeft)
		{
			if (Main.dedServ)
				return;

			Main.PlaySound(SoundID.DD2_SkeletonHurt.WithPitchVariance(0.3f).WithVolume(0.5f), projectile.Center);
			for (int i = 0; i < 10; i++)
				ParticleHandler.SpawnParticle(new GlowParticle(projectile.Center + Main.rand.NextVector2Circular(20, 10),
					Main.rand.NextVector2Unit() * Main.rand.NextFloat(2), Color.Magenta, Main.rand.NextFloat(0.02f, 0.04f), 30));

			for (int j = 0; j < 6; j++)
				Gore.NewGore(projectile.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(), mod.GetGoreSlot("Gores/Skelet/grave" + Main.rand.Next(1, 5)), 0.5f);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDrawTrail(spriteBatch, 0.2f, drawColor: new Color(99, 23, 51));
			projectile.QuickDraw(spriteBatch);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D maskTex = ModContent.GetTexture(Texture + "_mask");
			spriteBatch.Draw(maskTex, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(new Color(252, 68, 166)) * Math.Max(1 - AiTimer / 20f, 0), projectile.rotation,
				maskTex.Size() / 2, projectile.scale, projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
		}
	}
}