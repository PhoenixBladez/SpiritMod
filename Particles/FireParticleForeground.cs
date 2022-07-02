using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace SpiritMod.Particles
{
	public class FireParticleForeground : ScreenParticle
	{
		private static readonly int MaxTime = 360;

		private readonly Color Yellow = new Color(246, 255, 0);
		private readonly Color Orange = new Color(232, 37, 2);
		public override void UpdateOnScreen()
		{
			if (TimeActive >= MaxTime)
				Kill();

			Velocity.X += Main.windSpeedCurrent/4;
			Velocity.X *= 0.97f;
			Velocity.Y *= 0.998f;
			if (Main.rand.NextBool(6))
				Velocity = Velocity.RotatedByRandom(0.1f);
			Color = Color.Lerp(Yellow, Orange, (float)TimeActive/MaxTime) * (float)Math.Sin(MathHelper.Pi * (TimeActive / (float)MaxTime)) * 0.85f;
		}

		public override bool ActiveCondition => Main.LocalPlayer.ZoneMeteor || Main.LocalPlayer.ZoneUnderworldHeight;

		public override float ScreenSpawnChance => (MyWorld.ashRain && Main.LocalPlayer.ZoneUnderworldHeight) ? 0.08f : 0.12f;

		public override bool UseCustomScreenDraw => true;

		public override bool UseAdditiveBlend => true;

		public override void CustomScreenDraw(SpriteBatch spriteBatch)
		{
			Texture2D tex = ParticleHandler.GetTexture(Type);
			Texture2D bloom = Terraria.ModLoader.ModContent.Request<Texture2D>("Effects/Masks/CircleGradient");
			spriteBatch.Draw(bloom, GetDrawPosition(), null, Color * 0.6f, 0, bloom.Size() / 2, Scale / 5f, SpriteEffects.None, 0);
			spriteBatch.Draw(tex, GetDrawPosition(), null, Color, Velocity.ToRotation(), tex.Size() / 2, new Vector2(Scale, Scale * 0.75f), SpriteEffects.None, 0);
		}

		public override void OnSpawnAttempt()
		{
			FireParticleForeground fireParticle = new FireParticleForeground();

			Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
			Vector2 startingPosition = new Vector2(Main.rand.NextFloat(screenCenter.X - Main.screenWidth * 2, screenCenter.X + Main.screenWidth * 2), Main.screenPosition.Y + Main.screenHeight);

			fireParticle.Position = startingPosition;
			fireParticle.OriginalScreenPosition = Main.screenPosition;
			fireParticle.Velocity = new Vector2(0, Main.rand.NextFloat(-3, -6));
			fireParticle.Rotation = Main.rand.NextFloat(MathHelper.PiOver4);
			fireParticle.Scale = Main.rand.NextFloat(0.4f, 0.5f);
			fireParticle.ParallaxStrength = (float)Math.Pow(fireParticle.Scale, 3);

			ParticleHandler.SpawnParticle(fireParticle);
		}
	}
}