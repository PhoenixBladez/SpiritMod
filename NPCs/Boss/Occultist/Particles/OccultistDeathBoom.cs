using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod;
using SpiritMod.Particles;
using System;
using Terraria;

namespace SpiritMod.NPCs.Boss.Occultist.Particles
{
	public class OccultistDeathBoom : Particle
	{
		private const int _numFrames = 8;
		private int _frame;
		private const int _displayTime = 30;

		public OccultistDeathBoom(Vector2 position, float scale, float rotation = 0)
		{
			Position = position;
			Scale = scale;
			Rotation = rotation;
		}

		public override void Update()
		{
			_frame = (int)(_numFrames * TimeActive / _displayTime);
			if (TimeActive > _displayTime)
				Kill();
		}

		public override bool UseCustomDraw => true;

		public override bool UseAdditiveBlend => true;

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Texture2D tex = ParticleHandler.GetTexture(Type);
			Texture2D bloom = SpiritMod.Instance.GetTexture("Effects/Masks/CircleGradient");
			float bloomOpacity = Math.Abs(_displayTime / 2f - TimeActive) / (_displayTime / 2f);
			bloomOpacity = 1 - bloomOpacity;
			spriteBatch.Draw(bloom, Position - Main.screenPosition, null, new Color(255, 116, 156) * bloomOpacity, Rotation, bloom.Size() / 2, Scale * 1.5f, SpriteEffects.None, 0);
			var DrawFrame = new Rectangle(0, _frame * tex.Height / _numFrames, tex.Width, tex.Height / _numFrames);
			spriteBatch.Draw(tex, Position - Main.screenPosition, DrawFrame, Color.White, Rotation, DrawFrame.Size() / 2, Scale, SpriteEffects.None, 0);
		}
	}
}
