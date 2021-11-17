using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using System;
using Terraria;

namespace SpiritMod.Particles
{
	public class AnthemCircle : Particle
	{
		InterpolatedFloat progress;
		InterpolatedFloat interpolatedScale;

		public override bool UseAdditiveBlend => true;

		public AnthemCircle(Vector2 position, Vector2 velocity, float startScale, float endScale, float timeLeft)
		{
			Position = position;
			Velocity = velocity;
			Scale = startScale;
			interpolatedScale = new InterpolatedFloat(startScale, timeLeft);
			interpolatedScale.Set(endScale);

			progress = new InterpolatedFloat(1f, timeLeft);
			progress.Set(0);
		}

		public override bool UseCustomDraw => true;

		public override void Update()
		{
			Velocity *= 0.95f;
			Rotation = Velocity.ToRotation();
			Scale = interpolatedScale;

			if (progress <= 0f)
				Kill();

			progress.Process(SpiritMod.deltaTime);
			interpolatedScale.Process(SpiritMod.deltaTime);
		}

		public override void CustomDraw(SpriteBatch sB)
		{
			Texture2D basetexture = SpiritMod.Instance.GetTexture("Particles/AnthemCircle");

			sB.End();

			sB.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, SpiritMod.AnthemCircle, Main.GameViewMatrix.EffectMatrix);

			sB.Draw(basetexture, Position - Main.screenPosition, null, Color.White * progress, Rotation, basetexture.Size() / 2, new Vector2(Scale / 2, Scale), SpriteEffects.None, 0);

			sB.End();

			sB.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);
		}
	}
}
