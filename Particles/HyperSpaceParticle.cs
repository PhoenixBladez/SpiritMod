using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace SpiritMod.Particles
{
    public class HyperSpaceParticle : ScreenParticle
	{
		private readonly static int MaxTime = 1200;

		public override void UpdateOnScreen()
		{
			if (TimeActive >= MaxTime)
				Kill();

			Vector2 addedVelocity = 1.5f * Velocity.RotatedBy(MathHelper.Pi / 6 * (float)Math.Sin(Math.PI * ((MaxTime - TimeActive) / 60f) * (1f - Scale)));
			Position += addedVelocity;

			Color = ((GetHashCode() % 2 == 0) ? new Color(50, 160, 149) : new Color(179, 86, 158)) * (float)Math.Sin(MathHelper.TwoPi * ((TimeActive - MaxTime) / (float)MaxTime));
		}

		public override bool ActiveCondition => Main.LocalPlayer.GetModPlayer<MyPlayer>().ZoneSynthwave;

		public override float ScreenSpawnChance => 0.2f;

		public override bool UseAdditiveBlend => true;

		public override void OnSpawnAttempt()
		{
			HyperSpaceParticle hyperSpaceParticle = new HyperSpaceParticle(); 
			Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
			Vector2 startingPosition = new Vector2(Main.rand.NextFloat(screenCenter.X - Main.screenWidth * 2, screenCenter.X + Main.screenWidth * 2), Main.screenPosition.Y + Main.screenHeight);

			hyperSpaceParticle.Position = startingPosition;
			hyperSpaceParticle.OriginalScreenPosition = Main.screenPosition;
			hyperSpaceParticle.Velocity = Main.rand.NextFloat(-1, -0.5f) * Vector2.UnitY;
			hyperSpaceParticle.Scale = Main.rand.NextFloat(0.04f, 0.08f);
			hyperSpaceParticle.ParallaxStrength = hyperSpaceParticle.Scale * 5;

			ParticleHandler.SpawnParticle(hyperSpaceParticle);
		}
	}
}