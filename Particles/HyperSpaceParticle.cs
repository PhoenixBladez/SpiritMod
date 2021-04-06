using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace SpiritMod.Particles
{
    public class HyperspaceParticle : Particle
	{
		private readonly static int MaxTime = 1200;

		private Vector2 centerPlusPosition;
		private Vector2 spawnPosition;

		public override void Update()
		{
			if (TimeActive >= MaxTime)
				Kill();

			Vector2 worldPosition = centerPlusPosition;
			Vector2 screenPosition = spawnPosition;
			Position = Vector2.Lerp(screenPosition, screenPosition - 3 * (screenPosition - worldPosition), Scale);

			Vector2 addedVelocity = Velocity.RotatedBy(MathHelper.Pi / 6 * (float)Math.Sin(Math.PI * ((MaxTime - TimeActive) / 60f) * (1f - Scale)));
			centerPlusPosition += addedVelocity;
			spawnPosition += addedVelocity;

			Color = Color.White * (float)Math.Sin(MathHelper.TwoPi * ((TimeActive - MaxTime) / (float)MaxTime));
		}

		public override float SpawnChance() => Main.LocalPlayer.GetModPlayer<MyPlayer>().ZoneSynthwave ? 0.25f : 0f;

		public override void OnSpawnAttempt()
		{
			HyperspaceParticle hyperSpaceParticle = new HyperspaceParticle();
			Vector2 position = new Vector2(Main.rand.Next(-2000, 2000), Main.rand.Next(1200, 1600));

			hyperSpaceParticle.Position = Vector2.Zero;
			hyperSpaceParticle.Velocity = Main.rand.NextFloat(-2, -1) * Vector2.UnitY;
			hyperSpaceParticle.Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			hyperSpaceParticle.Scale = Main.rand.NextFloat(0.4f, 0.6f);
			hyperSpaceParticle.Color = Color.White;

			hyperSpaceParticle.centerPlusPosition = Main.LocalPlayer.position + position;
			hyperSpaceParticle.spawnPosition = position;
		}
	}
}