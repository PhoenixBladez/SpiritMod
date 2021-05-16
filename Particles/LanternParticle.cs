using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace SpiritMod.Particles
{
    public class LanternParticle : ScreenParticle
	{
		private readonly static int MaxTime = 3000;

		public override void UpdateOnScreen()
		{
			if (TimeActive >= MaxTime)
				Kill();

			Vector2 addedVelocity = Velocity.RotatedBy(MathHelper.Pi / 4 * (float)Math.Sin(Math.PI * ((MaxTime - TimeActive) / 60f) * (1f - Scale)));
			Position += addedVelocity;

			Rotation = MathHelper.Pi / 4 * (float)Math.Sin(Math.PI * ((MaxTime - TimeActive) / 60f) * (1f - Scale)) * .75f;
			Color = Color.White * (float)Math.Sin(MathHelper.TwoPi * ((MaxTime - TimeActive) / (float)MaxTime));
		}

		public override bool ActiveCondition => Main.LocalPlayer.GetModPlayer<MyPlayer>().ZoneLantern;

		public override float ScreenSpawnChance => 0.0125f;

		public override void OnSpawnAttempt()
		{
			LanternParticle lanternParticle = new LanternParticle();
			Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
			Vector2 startingPosition = new Vector2(Main.rand.NextFloat(screenCenter.X - Main.screenWidth * 2, screenCenter.X + Main.screenWidth * 2), Main.screenPosition.Y + Main.screenHeight);

			lanternParticle.Position = startingPosition;
			lanternParticle.OriginalScreenPosition = Main.screenPosition;
			lanternParticle.Velocity = Main.rand.NextFloat(-1, -.3f) * Vector2.UnitY;
			lanternParticle.Scale = Main.rand.NextFloat(0.5f, 1f);
			lanternParticle.Color = Color.White;

			ParticleHandler.SpawnParticle(lanternParticle);
		}
	}
}