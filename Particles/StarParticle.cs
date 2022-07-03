using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Particles
{
	public class StarParticle : Particle
	{
		private Color starColor;
		private Color bloomColor;
		private float opacity;
		public int MaxTime;
		private float rotSpeed;

		public override bool UseAdditiveBlend => true;

		public StarParticle(Vector2 position, Vector2 velocity, Color color, float scale, int maxTime, float rotationSpeed = 1f)
		{
			Position = position;
			Velocity = velocity;
			starColor = color;
			bloomColor = color;
			Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			Scale = scale;
			MaxTime = maxTime;
			rotSpeed = rotationSpeed;
		}

		public StarParticle(Vector2 position, Vector2 velocity, Color StarColor, Color BloomColor, float scale, int maxTime, float rotationSpeed = 1f)
		{
			Position = position;
			Velocity = velocity;
			starColor = StarColor;
			bloomColor = BloomColor;
			Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			Scale = scale;
			MaxTime = maxTime;
			rotSpeed = rotationSpeed;
		}

		public override bool UseCustomDraw => true;

		public override void Update()
		{
			opacity = (float)Math.Sin(((float)TimeActive / MaxTime) * MathHelper.Pi);
			Color = bloomColor * opacity;
			Lighting.AddLight(Position, Color.R / 255f, Color.G / 255f, Color.B / 255f);
			Velocity *= 0.98f;
			Rotation += rotSpeed * ((Velocity.X > 0) ? 0.07f : -0.07f);

			if (TimeActive >= MaxTime)
				Kill();
		}

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Texture2D basetexture = ParticleHandler.GetTexture(Type);
			Texture2D bloomtexture = ModContent.Request<Texture2D>("Effects/Masks/CircleGradient", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			spriteBatch.Draw(bloomtexture, Position - Main.screenPosition, null, bloomColor * opacity * 0.5f, 0, bloomtexture.Size() / 2, Scale/2, SpriteEffects.None, 0);

			spriteBatch.Draw(basetexture, Position - Main.screenPosition, null, starColor * opacity * 0.5f, Rotation * 1.5f, basetexture.Size() / 2, Scale * 0.75f, SpriteEffects.None, 0);
			spriteBatch.Draw(basetexture, Position - Main.screenPosition, null, starColor * opacity * 0.5f, -Rotation * 1.5f, basetexture.Size() / 2, Scale * 0.75f, SpriteEffects.None, 0);

			spriteBatch.Draw(basetexture, Position - Main.screenPosition, null, starColor * opacity, Rotation, basetexture.Size() / 2, Scale, SpriteEffects.None, 0);

		}
	}
}
