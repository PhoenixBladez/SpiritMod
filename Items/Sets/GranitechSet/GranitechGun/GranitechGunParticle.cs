using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using System;
using System.Linq;
using Terraria;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechGun
{
    public class GranitechGunParticle : Particle
	{
		private Color bloomColor;
		private float opacity;
		public int MaxTime;

		public override bool UseAdditiveBlend => true;
		public override bool UseCustomDraw => true;

		public GranitechGunParticle(Vector2 position, Vector2 velocity, Color color, float scale, int maxTime)
		{
			Position = position;
			Velocity = velocity;
			bloomColor = color;
			Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			Scale = scale;
			MaxTime = maxTime;
		}

		public override void Update()
		{
			Lighting.AddLight(Position, Color.R / 255f, Color.G / 255f, Color.B / 255f);

			opacity = 1 - ((float)TimeActive / MaxTime);
			Velocity *= 0.93f;
			Rotation = Velocity.ToRotation();

			if (TimeActive >= MaxTime)
				Kill();
		}

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Texture2D baseTex = ParticleHandler.GetTexture(Type);
			Texture2D bloomTex = SpiritMod.instance.GetTexture("Effects/Masks/CircleGradient");

			Rectangle frame = new Rectangle(0, 0, 20, 44);

			spriteBatch.Draw(baseTex, Position - Main.screenPosition, frame, Color.White * opacity, Rotation, new Vector2(10, 22), Scale / 2f, SpriteEffects.None, 0);

			spriteBatch.Draw(bloomTex, Position - Main.screenPosition, null, bloomColor * opacity, 0, bloomTex.Size() / 2f, Scale / 4f, SpriteEffects.None, 0);
			spriteBatch.Draw(bloomTex, Position - Main.screenPosition, null, bloomColor * opacity * 0.5f, 0, bloomTex.Size() / 2f, Scale / 2f, SpriteEffects.None, 0);
		}
	}
}