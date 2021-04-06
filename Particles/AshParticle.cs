using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace SpiritMod.Particles
{
	public class AshParticle : Particle
	{
		private static readonly int MaxTime = 1100;

		private Vector2 centerPlusPosition;
		private Vector2 spawnPosition;

		public override void Update()
		{
			if (TimeActive >= MaxTime)
				Kill();

			Vector2 worldPosition = centerPlusPosition - Main.screenPosition;
			Vector2 screenPosition = spawnPosition;
			Position = Vector2.Lerp(screenPosition, screenPosition - 3 * (screenPosition - worldPosition), Scale);

			centerPlusPosition += Velocity;
			spawnPosition += Velocity;

			Color = Color.Black * (float)Math.Sin(MathHelper.TwoPi * ((MaxTime - TimeActive) / (float)MaxTime)) * 0.85f;
		}

		public override float SpawnChance() => Main.LocalPlayer.ZoneUnderworldHeight && MyWorld.ashRain ? 0.65f : 0f;

		public override void OnSpawnAttempt()
		{
			AshParticle ashParticle = new AshParticle();

			Vector2 startingPosition = new Vector2(Main.rand.Next(-2000, 2000), Main.rand.Next(1200, 1800)) + Main.screenPosition;
			ashParticle.Position = startingPosition;
			ashParticle.Velocity = new Vector2(Main.windSpeed * 12f, Main.rand.NextFloat(1, 3f));
			ashParticle.Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			ashParticle.Scale = Main.rand.NextFloat(0.3f, 1.05f);
			ashParticle.Color = Color.White;

			ashParticle.centerPlusPosition = Main.LocalPlayer.Center + startingPosition;
			ashParticle.spawnPosition = startingPosition;

			ParticleHandler.SpawnParticle(ashParticle);
		}
	}
}