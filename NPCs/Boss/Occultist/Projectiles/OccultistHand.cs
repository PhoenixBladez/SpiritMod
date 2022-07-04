using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Particles;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Occultist.Projectiles
{
	public class OccultistHand : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Grasp");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public int maxTimeLeft = 180;
		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.hostile = true;
			Projectile.scale = Main.rand.NextFloat(0.12f, 0.2f);
			Projectile.timeLeft = maxTimeLeft;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.spriteDirection = Main.rand.NextBool() ? -1 : 1;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, new Color(99, 23, 51) * 0.2f), new RoundCap(), new ArrowGlowPosition(), 200 * Projectile.scale, 100);
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, new Color(99, 23, 51), new Color(181, 0, 116)), new NoCap(), new DefaultTrailPosition(), 150 * Projectile.scale, 60, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_3").Value, 0.2f, 1f, 1f));
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, new Color(99, 23, 51), new Color(181, 0, 116)), new NoCap(), new DefaultTrailPosition(), 150 * Projectile.scale, 60, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_4").Value, 0.2f, 1f, 1f));
		}

		public float period = 80;
		public ref float Amplitude => ref Projectile.ai[0];
		private ref float PeriodOffset => ref Projectile.ai[1];

		private float AiTimer => maxTimeLeft - Projectile.timeLeft;
		public bool DoAcceleration = true;
		public bool TileCollideCheck = true;

		private readonly float acceleration = 1.01f;

		public override void AI()
		{
			Projectile.position -= Projectile.velocity;
			Projectile.Size = Vector2.One * 20 * Projectile.scale;

			Vector2 cosVel = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Amplitude) * (float)Math.Cos((AiTimer + PeriodOffset) / period * MathHelper.TwoPi));
			Projectile.position += cosVel;
			Projectile.rotation = cosVel.ToRotation() + MathHelper.PiOver2;

			if (AiTimer == 0 && !Main.dedServ)
			{
				ParticleHandler.SpawnParticle(new StarParticle(Projectile.Center, cosVel * 0.4f, new Color(99, 23, 51), 0.4f, 15, 3));
				ParticleHandler.SpawnParticle(new StarParticle(Projectile.Center, cosVel * 0.4f, new Color(99, 23, 51), 0.4f, 15, 4));

				for (int i = 0; i < 4; i++)
					ParticleHandler.SpawnParticle(new GlowParticle(Projectile.Center, cosVel.RotatedByRandom(MathHelper.Pi / 20) * Main.rand.NextFloat(), Color.Magenta, Main.rand.NextFloat(0.02f, 0.04f), 30));
			}

			if (Main.rand.NextBool(10) && !Main.dedServ)
				ParticleHandler.SpawnParticle(new GlowParticle(Projectile.Center, Projectile.velocity * Main.rand.NextFloat(), Color.Magenta, Main.rand.NextFloat(0.02f, 0.04f), 30));

			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 25;
				Projectile.scale *= 1.15f;
				Projectile.velocity *= 1.15f;
			}
			else
				Projectile.alpha = 0;

			if (AiTimer > maxTimeLeft / 3f)
			{
				if (TileCollideCheck)
					Projectile.tileCollide = true;

				if (Projectile.velocity.Length() < 20 && DoAcceleration)
					Projectile.velocity *= acceleration;
			}
		}

		public override void Kill(int timeLeft)
		{
			if (Main.dedServ)
				return;

			SoundEngine.PlaySound(SoundID.DD2_SkeletonHurt with { PitchVariance = 0.3f, Volume = 0.5f }, Projectile.Center);
			for (int i = 0; i < 10; i++)
				ParticleHandler.SpawnParticle(new GlowParticle(Projectile.Center + Main.rand.NextVector2Circular(20, 10),
					Main.rand.NextVector2Unit() * Main.rand.NextFloat(2), Color.Magenta, Main.rand.NextFloat(0.02f, 0.04f), 30));

			for (int j = 0; j < 6; j++)
				Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(), Mod.Find<ModGore>("Skelet/grave" + Main.rand.Next(1, 5)).Type, 0.5f);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.QuickDrawTrail(Main.spriteBatch, 0.2f, drawColor: new Color(99, 23, 51));
			Projectile.QuickDraw(Main.spriteBatch);
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			Texture2D maskTex = ModContent.Request<Texture2D>(Texture + "_mask", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Main.spriteBatch.Draw(maskTex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(new Color(252, 68, 166)) * Math.Max(1 - AiTimer / 20f, 0), Projectile.rotation,
				maskTex.Size() / 2, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
		}
	}
}