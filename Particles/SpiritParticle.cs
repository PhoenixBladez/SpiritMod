using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace SpiritMod.Particles
{
    public class SpiritParticle : Particle
	{
		private readonly static int MaxTime = 1620;

		public override void Update()
		{
			if (TimeActive > MaxTime)
				Kill();

			Color = Color.White * (float)Math.Sin(MathHelper.TwoPi * ((MaxTime - TimeActive) / (float)MaxTime)) * 0.33f;
		}

		public override bool ActiveCondition() => Main.LocalPlayer.GetModPlayer<MyPlayer>().ZoneSpirit;

		public override float SpawnChance() => 0.125f;

		public override void OnSpawnAttempt()
		{
			SpiritParticle spiritParticle = new SpiritParticle();

			Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
			Vector2 startingPosition = new Vector2(Main.rand.NextFloat(screenCenter.X - Main.screenWidth * 2, screenCenter.X + Main.screenWidth * 2), Main.screenPosition.Y + Main.screenHeight);

			spiritParticle.Position = startingPosition;
			spiritParticle.origScreenpos = Main.screenPosition;
			spiritParticle.Velocity = Main.rand.NextFloat(-1.2f, -0.8f) * Vector2.UnitY;
			spiritParticle.Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			spiritParticle.Scale = Main.rand.NextFloat(0.4f, 0.6f);
			spiritParticle.Color = Color.White;

			ParticleHandler.SpawnParticle(spiritParticle);
		}
	}
}