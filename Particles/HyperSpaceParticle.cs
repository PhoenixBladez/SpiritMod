using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace SpiritMod.Particles
{
    public class HyperSpaceParticle : Particle
	{
		private readonly static int MaxTime = 1200;

		public override void Update()
		{
			if (TimeActive >= MaxTime)
				Kill();

			Vector2 addedVelocity = 1.5f * Velocity.RotatedBy(MathHelper.Pi / 6 * (float)Math.Sin(Math.PI * ((MaxTime - TimeActive) / 60f) * (1f - Scale)));
			Position += addedVelocity;

			Color = Color.White * (float)Math.Sin(MathHelper.TwoPi * ((TimeActive - MaxTime) / (float)MaxTime));
		}

		public override bool ActiveCondition() => Main.LocalPlayer.GetModPlayer<MyPlayer>().ZoneSynthwave;

		public override float SpawnChance() => 0.2f;

		public override bool UseCustomDraw => true;

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Rectangle drawFrame = new Rectangle(0, ((GetHashCode() % 2 == 0) ? 0 : 1) * Texture.Height / 2, Texture.Width, Texture.Height/2);
			spriteBatch.Draw(Texture, DrawPosition(), drawFrame, Color * ActiveOpacity, Rotation, Origin, Scale * Main.GameViewMatrix.Zoom, SpriteEffects.None, 0);
		}

		public override void OnSpawnAttempt()
		{
			HyperSpaceParticle hyperSpaceParticle = new HyperSpaceParticle(); 
			Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
			Vector2 startingPosition = new Vector2(Main.rand.NextFloat(screenCenter.X - Main.screenWidth * 2, screenCenter.X + Main.screenWidth * 2), Main.screenPosition.Y + Main.screenHeight);

			hyperSpaceParticle.Position = startingPosition;
			hyperSpaceParticle.origScreenpos = Main.screenPosition;
			hyperSpaceParticle.Velocity = Main.rand.NextFloat(-1, -0.5f) * Vector2.UnitY;
			hyperSpaceParticle.Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			hyperSpaceParticle.Scale = Main.rand.NextFloat(0.4f, 0.6f);
			hyperSpaceParticle.Color = Color.White;

			ParticleHandler.SpawnParticle(hyperSpaceParticle);
		}
	}
}