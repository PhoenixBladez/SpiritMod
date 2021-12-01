using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using SpiritMod.Utilities;
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
		public GranitechParticle(Vector2 position, Vector2 velocity, Color color, float scale, int maxTime, int frame)
		{
			Color = color;
			Position = position;
			Velocity = velocity;
			Scale = scale;
			MaxTime = maxTime;

			switch (frame)
			{
				case 0:
					Frame = new Rectangle(2, 20, 20, 10); //Laser parallelogram 1
					break;
				case 1:
					Frame = new Rectangle(18, 20, 22, 12);//Laser parallelogram 2
					break;
				case 2:
					Frame = new Rectangle(4, 32, 14, 10); //Laser rectangle 1
					break;
				case 3:
					Frame = new Rectangle(20, 32, 18, 12); //Laser rectangle 2
					break;
				case 4:
					Frame = new Rectangle(0, 44, 14, 8); //Small line
					break;
				case 5:
					Frame = new Rectangle(12, 44, 16, 8); //Large line
					break;
				case 6:
					Frame = new Rectangle(26, 44, 14, 14); //Circle
					break;
			}
		}

		public override void Update()
		{
			Lighting.AddLight(Position, Color.R / 255f, Color.G / 255f, Color.B / 255f);

			opacity = (float)Math.Pow(1 - ((float)TimeActive / MaxTime), 0.5f);
			Rotation = Velocity.ToRotation();

			Velocity *= 0.87f;
			Scale *= 0.96f;

			if (TimeActive >= MaxTime)
				Kill();
		}

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Texture2D baseTex = ParticleHandler.GetTexture(Type);

			DrawAberration.DrawChromaticAberration(Vector2.UnitX.RotatedBy(Rotation), 1.5f, delegate (Vector2 offset, Color colorMod)
			{
				spriteBatch.Draw(baseTex, Position + offset - Main.screenPosition, Frame, Color.MultiplyRGB(colorMod) * opacity, Rotation, Frame.Size() / 2, Scale / 2f, SpriteEffects.None, 0);
			});
		}
	}
}