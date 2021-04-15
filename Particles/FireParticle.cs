using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace SpiritMod.Particles
{
	public class FireParticle : Particle
	{
		private static readonly int MaxTime = 540;

		public override void Update()
		{
			if (TimeActive >= MaxTime)
				Kill();

			Velocity *= 0.9978f;
			Color = Color.White * (float)Math.Sin(MathHelper.TwoPi * ((MaxTime - TimeActive) / (float)MaxTime)) * 0.85f;
		}

		public override bool ActiveCondition() => Main.LocalPlayer.ZoneMeteor || Main.LocalPlayer.ZoneUnderworldHeight;

		public override float SpawnChance() => 0.62f;

		public override void OnSpawnAttempt()
		{
			FireParticle fireParticle = new FireParticle();

			Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
			Vector2 startingPosition = new Vector2(Main.rand.NextFloat(screenCenter.X - Main.screenWidth * 2, screenCenter.X + Main.screenWidth * 2), Main.screenPosition.Y + Main.screenHeight);

			fireParticle.Position = startingPosition;
			fireParticle.origScreenpos = Main.screenPosition;
			fireParticle.Velocity = new Vector2(Main.windSpeed * 2f, Main.rand.NextFloat(-2, -6f));
			fireParticle.Rotation = Main.rand.NextFloat(MathHelper.PiOver4);
			fireParticle.Scale = Main.rand.NextFloat(0.3f, 0.85f);
			fireParticle.Color = Color.White;

			ParticleHandler.SpawnParticle(fireParticle);
		}
	}
}