using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace SpiritMod.Particles
{
	public class AshParticle : ScreenParticle
	{
		private static readonly int MaxTime = 600;

		public override void UpdateOnScreen()
		{
			if (TimeActive >= MaxTime)
				Kill();

			Velocity.X += Main.windSpeed / 2f;
			Velocity.X *= 0.992f;
			Color = Color.Black * (float)Math.Sin(MathHelper.TwoPi * ((MaxTime - TimeActive) / (float)MaxTime)) * 0.8f;
		}

		public override bool ActiveCondition => Main.LocalPlayer.ZoneUnderworldHeight && MyWorld.ashRain;

		public override float ScreenSpawnChance => 0.8f;

		public override void OnSpawnAttempt()
		{
			AshParticle ashParticle = new AshParticle();
			Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
			Vector2 startingPosition = new Vector2(Main.rand.NextFloat(screenCenter.X - Main.screenWidth * 2, screenCenter.X + Main.screenWidth * 2), Main.screenPosition.Y);

			ashParticle.Position = startingPosition;
			ashParticle.OriginalScreenPosition = Main.screenPosition;
			ashParticle.Velocity = new Vector2(Main.windSpeed * 12f, Main.rand.NextFloat(3, 4f));
			ashParticle.Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			ashParticle.Scale = Main.rand.NextFloat(0.8f, 1.2f);
			ashParticle.ParallaxStrength = ashParticle.Scale / 3f;
			ashParticle.Color = Color.White;

			ParticleHandler.SpawnParticle(ashParticle);
		}
	}
}