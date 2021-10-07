using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using Terraria;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechGun
{
    public class GranitechGunParticle : Particle
	{
		private readonly Rectangle Frame = new Rectangle();
		public readonly int MaxTime;

		private float opacity;


		public override bool UseAdditiveBlend => true;
		public override bool UseCustomDraw => true;

		public GranitechGunParticle(Vector2 position, Vector2 velocity, float scale, int maxTime, int frame = 0)
		{
			Color = Main.rand.NextBool(2) ? new Color(255, 46, 122) : new Color(239, 241, 80);
			Position = position;
			Velocity = velocity;
			Scale = scale;
			MaxTime = maxTime;

			Frame = new Rectangle(0, 0, 20, 44); //Ring 1
			if (frame == 1)
				Frame = new Rectangle(20, 0, 20, 44); //Ring 2
			else if (frame == 2)
				Frame = new Rectangle(0, 44, 14, 8); //Small line
			else if (frame == 3)
				Frame = new Rectangle(12, 44, 16, 8); //Large line
			else if (frame == 4)
				Frame = new Rectangle(26, 44, 14, 14); //Circle
		}

		public override void Update()
		{
			Lighting.AddLight(Position, Color.R / 255f, Color.G / 255f, Color.B / 255f);

			opacity = 1 - ((float)TimeActive / MaxTime);
			Rotation = Velocity.ToRotation();

			if (Frame.Y == 44)
				Velocity *= 0.98f;
			else
				Velocity *= 0.93f;

			if (TimeActive >= MaxTime)
				Kill();
		}

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Texture2D baseTex = ParticleHandler.GetTexture(Type);
			Texture2D bloomTex = SpiritMod.instance.GetTexture("Effects/Masks/CircleGradient");

			spriteBatch.Draw(baseTex, Position - Main.screenPosition, Frame, Color * opacity, Rotation, Frame.Size() / 2f, Scale / 2f, SpriteEffects.None, 0);

			float bloomScale = Frame.Y == 44 ? 0.5f : 1f;
			spriteBatch.Draw(bloomTex, Position - Main.screenPosition, null, Color * opacity * 0.5f * bloomScale, 0, bloomTex.Size() / 2f, Scale / 4f * bloomScale, SpriteEffects.None, 0);
			spriteBatch.Draw(bloomTex, Position - Main.screenPosition, null, Color * opacity * 0.25f * bloomScale, 0, bloomTex.Size() / 2f, Scale / 2f * bloomScale, SpriteEffects.None, 0);
		}
	}
}