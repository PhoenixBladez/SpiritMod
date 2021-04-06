using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace SpiritMod.Particles
{
    public class SpiritParticle : Particle
	{
		private readonly static int MaxTime = 1620;

		private Vector2 centerPlusPosition;
		private Vector2 spawnPosition;

		public override void Update()
		{
			if (TimeActive > MaxTime)
				Kill();

			Vector2 WorldPosition = centerPlusPosition - Main.screenPosition;
			Vector2 ScreenPosition = spawnPosition;
			Position = Vector2.Lerp(ScreenPosition, ScreenPosition - 3 * (ScreenPosition - WorldPosition), Scale);

			centerPlusPosition += Velocity;
			spawnPosition += Velocity;

			Color = Color.White * (float)Math.Sin(MathHelper.TwoPi * ((MaxTime - TimeActive) / (float)MaxTime)) * 0.33f;
		}

		public override float SpawnChance() => Main.LocalPlayer.GetModPlayer<MyPlayer>().ZoneSpirit ? 0.125f : 0f;

		public override void OnSpawnAttempt()
		{
			SpiritParticle spiritParticle = new SpiritParticle();
			Vector2 position = new Vector2(Main.rand.Next(-2000, 2000), Main.rand.Next(1400, 1800));

			spiritParticle.Position = position;
			spiritParticle.Velocity = Main.rand.NextFloat(-1.2f, -0.8f) * Vector2.UnitY;
			spiritParticle.Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			spiritParticle.Scale = Main.rand.NextFloat(0.4f, 0.6f);
			spiritParticle.Color = Color.White;

			centerPlusPosition = Main.LocalPlayer.Center + position;
			spawnPosition = position;
		}
	}
}