﻿using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace SpiritMod.Particles
{
    public class SpiritParticle : ScreenParticle
	{
		private readonly static int MaxTime = 1620;

		public override void UpdateOnScreen()
		{
			if (TimeActive > MaxTime)
				Kill();

			Color = Color.White * (float)Math.Sin(MathHelper.TwoPi * ((MaxTime - TimeActive) / (float)MaxTime)) * 0.33f;

			if (Position.Y < Main.worldSurface * 16)
				Velocity.X = MathHelper.Lerp(Velocity.X, Main.windSpeedCurrent * 15, 0.1f);
		}

		public override bool ActiveCondition => Main.LocalPlayer.GetModPlayer<MyPlayer>().ZoneSpirit;

		public override float ScreenSpawnChance => 0.125f;

		public override void OnSpawnAttempt()
		{
			SpiritParticle spiritParticle = new SpiritParticle();

			Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
			Vector2 startingPosition = new Vector2(Main.rand.NextFloat(screenCenter.X - Main.screenWidth * 2, screenCenter.X + Main.screenWidth * 2), Main.screenPosition.Y + Main.screenHeight);

			spiritParticle.Position = startingPosition;
			spiritParticle.OriginalScreenPosition = Main.screenPosition;
			spiritParticle.Velocity = Main.rand.NextFloat(-1.2f, -0.8f) * Vector2.UnitY;
			spiritParticle.Scale = Main.rand.NextFloat(0.4f, 0.6f);
			spiritParticle.ParallaxStrength = spiritParticle.Scale;

			ParticleHandler.SpawnParticle(spiritParticle);
		}
	}
}