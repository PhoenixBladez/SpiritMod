using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using System;
using Terraria;

namespace SpiritMod.Items.Sets.GranitechSet
{
    public class GranitechParticle : Particle
	{
		private readonly Rectangle Frame = new Rectangle();
		public readonly int MaxTime;

		private float opacity;

		public override bool UseAdditiveBlend => true;
		public override bool UseCustomDraw => true;

		public GranitechParticle(Vector2 position, Vector2 velocity, Color color, float scale, int maxTime)
		{
			Color = color;
			Position = position;
			Velocity = velocity;
			Scale = scale;
			MaxTime = maxTime;

			int frame = Main.rand.Next(3);
			switch (frame)
			{
				case 0:
					Frame = new Rectangle(0, 44, 14, 8); //Small line
					break;
				case 1:
					Frame = new Rectangle(12, 44, 16, 8); //Large line
					break;
				case 2:
					Frame = new Rectangle(26, 44, 14, 14); //Circle
					break;
			}
		}

		public override void Update()
		{
			Lighting.AddLight(Position, Color.R / 255f, Color.G / 255f, Color.B / 255f);

			opacity = (float)Math.Pow(1 - ((float)TimeActive / MaxTime), 0.33f);
			Rotation = Velocity.ToRotation();

			if (Frame.Y == 44)
			{
				Velocity *= 0.90f;
				Scale *= 0.93f;
			}
			else
				Velocity *= 0.93f;

			if (TimeActive >= MaxTime)
				Kill();
		}

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Texture2D baseTex = ParticleHandler.GetTexture(Type);

			for (int j = -1; j <= 1; j++) //repeat multiple times with different offset and color, for chromatic aberration effect
			{
				Vector2 posOffset = Vector2.UnitX * j * 1.5f;
				Color colorMod = (j == -1) ? new Color(255, 0, 0) : ((j == 0) ? new Color(0, 255, 0) : new Color(0, 0, 255));

				spriteBatch.Draw(baseTex, Position + posOffset - Main.screenPosition, Frame, Color.MultiplyRGB(colorMod) * opacity, Rotation, Frame.Size() / 2f, Scale / 2f, SpriteEffects.None, 0);
			}
		}
	}
}