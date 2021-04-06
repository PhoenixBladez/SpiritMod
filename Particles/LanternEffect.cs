using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace SpiritMod.Particles
{
    public class LanternParticle : Particle
	{
		private readonly static int MaxTime = 3000;

		private Vector2 centerPlusPosition;
		private Vector2 spawnPosition;

		public override void Update()
		{
			if (TimeActive >= MaxTime)
				Kill();

			Vector2 WorldPosition = centerPlusPosition - Main.screenPosition;
			Vector2 ScreenPosition = spawnPosition;
			Position = Vector2.Lerp(ScreenPosition, ScreenPosition - 3 * (ScreenPosition - WorldPosition), Scale);

			Vector2 addedVelocity = Velocity.RotatedBy(MathHelper.Pi / 4 * (float)Math.Sin(Math.PI * ((MaxTime - TimeActive) / 60f) * (1f - Scale)));
			centerPlusPosition += addedVelocity;
			spawnPosition += addedVelocity;

			Rotation = MathHelper.Pi / 4 * (float)Math.Sin(Math.PI * ((MaxTime - TimeActive) / 60f) * (1f - Scale)) * .75f;
			Color = Color.White * (float)Math.Sin(MathHelper.TwoPi * ((MaxTime - TimeActive) / (float)MaxTime));
		}

		public override float SpawnChance() => Main.LocalPlayer.GetModPlayer<MyPlayer>().ZoneLantern ? 0.0125f : 0f;

		public override void OnSpawnAttempt()
		{
			LanternParticle lanternParticle = new LanternParticle();
			Vector2 position = new Vector2(Main.rand.Next(-2000, 2000), Main.rand.Next(1200, 1600));

			lanternParticle.Position = Vector2.Zero;
			lanternParticle.Velocity = Main.rand.NextFloat(-2, -.6f) * Vector2.UnitY;
			lanternParticle.Scale = Main.rand.NextFloat(0.5f, 1f);
			lanternParticle.Color = Color.White;

			lanternParticle.centerPlusPosition = Main.LocalPlayer.Center + position;
			lanternParticle.spawnPosition = position;

			ParticleHandler.SpawnParticle(lanternParticle);
		}
	}
}