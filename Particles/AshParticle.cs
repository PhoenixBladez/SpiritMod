using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace SpiritMod.Particles
{
	public class AshParticle : Particle
	{
		private static readonly int MaxTime = 1100;

		public override void Update()
		{
			if (TimeActive >= MaxTime)
				Kill();

			Color = Color.Black * (float)Math.Sin(MathHelper.TwoPi * ((MaxTime - TimeActive) / (float)MaxTime)) * 0.85f;
		}

		public override bool ActiveCondition() => Main.LocalPlayer.ZoneUnderworldHeight && MyWorld.ashRain;

		public override float SpawnChance() => 0.65f;

		public override void OnSpawnAttempt()
		{
			AshParticle ashParticle = new AshParticle();
			Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
			Vector2 startingPosition = new Vector2(Main.rand.NextFloat(screenCenter.X - Main.screenWidth * 2, screenCenter.X + Main.screenWidth * 2), Main.screenPosition.Y);

			ashParticle.Position = startingPosition;
			ashParticle.origScreenpos = Main.screenPosition;
			ashParticle.Velocity = new Vector2(Main.windSpeed * 12f, Main.rand.NextFloat(1, 3f));
			ashParticle.Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			ashParticle.Scale = Main.rand.NextFloat(0.3f, 1.05f);
			ashParticle.Color = Color.White;

			ParticleHandler.SpawnParticle(ashParticle);
		}
	}
}