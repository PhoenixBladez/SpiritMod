using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace SpiritMod.Particles
{
	public class AuroraStarParticle : Particle
	{
		private Color glowColor;
		public int MaxTime;

		public override bool UseAdditiveBlend => true;

		public AuroraStarParticle(Vector2 position, Vector2 velocity, Color color, float rotation, float scale, int maxTime)
		{
			Position = position;
			Velocity = velocity;
			glowColor = color;
			Rotation = rotation;
			Scale = scale;
			MaxTime = maxTime;
		}

		public override bool UseCustomDraw => true;

		public override void Update()
		{
			float opacity = (float)Math.Sin((MaxTime - TimeActive / (float)MaxTime) * MathHelper.Pi);
			Color = glowColor * opacity;
			Lighting.AddLight(Position, Color.R / 255f, Color.G / 255f, Color.B / 255f);
			Velocity *= 0.98f;
			Rotation += (Velocity.X > 0) ? 0.07f : -0.07f;

			if (TimeActive >= MaxTime)
				Kill();
		}

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Texture2D basetexture = ParticleHandler.GetTexture(Type);
			Texture2D bloomtexture = SpiritMod.instance.GetTexture("Effects/Masks/CircleGradient");

			spriteBatch.Draw(bloomtexture, Position - Main.screenPosition, null, Color * 0.5f, 0, bloomtexture.Size() / 2, Scale/2, SpriteEffects.None, 0);

			spriteBatch.Draw(basetexture, Position - Main.screenPosition, null, Color * 0.5f, Rotation * 1.5f, basetexture.Size() / 2, Scale * 0.75f, SpriteEffects.None, 0);
			spriteBatch.Draw(basetexture, Position - Main.screenPosition, null, Color * 0.5f, -Rotation * 1.5f, basetexture.Size() / 2, Scale * 0.75f, SpriteEffects.None, 0);

			spriteBatch.Draw(basetexture, Position - Main.screenPosition, null, Color, Rotation, basetexture.Size() / 2, Scale, SpriteEffects.None, 0);

		}
	}
}
