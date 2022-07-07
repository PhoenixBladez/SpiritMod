using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace SpiritMod.Particles
{
	public class SmokeParticle : Particle
	{
		private Color smokeColor;
		private float opacity = 0;
		public int MaxTime;

		public delegate void UpdateAction(Particle particle);

		private readonly UpdateAction _action;
		public SmokeParticle(Vector2 position, Vector2 velocity, Color color, float scale, int maxTime, UpdateAction action = null)
		{
			Position = position;
			Velocity = velocity;
			smokeColor = color;
			Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			Scale = scale;
			MaxTime = maxTime;
			_action = action;
		}


		public override bool UseCustomDraw => true;

		public override void Update()
		{
			Velocity *= 0.98f;
			float halftime = (MaxTime / 2f);
			opacity = (float)Math.Pow((halftime - Math.Abs(halftime - TimeActive)) / halftime, 0.75);
			Scale += 0.01f;
			Rotation += Velocity.X * 0.1f;

			if (_action != null)
				_action.Invoke(this);

			if (TimeActive >= MaxTime)
				Kill();
		}

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Texture2D basetexture = ParticleHandler.GetTexture(Type);
			Color lightColor = Lighting.GetColor((int)Position.X / 16, (int)Position.Y / 16);
			spriteBatch.Draw(basetexture, Position - Main.screenPosition, null, (lightColor.MultiplyRGBA(smokeColor) * opacity), Rotation, basetexture.Size() / 2, Scale, SpriteEffects.None, 0);
		}
	}
}
