using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod;
using SpiritMod.Particles;
using System;
using Terraria;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechGun
{
	public class GranitechGunBurst : Particle
	{
		private const int _numFrames = 2;
		private int _frame;
		private readonly int _direction;
		private const int _displayTime = 10;

		public GranitechGunBurst(Vector2 position, float scale)
		{
			Position = position;
			Scale = scale;
			_direction = Main.rand.NextBool() ? -1 : 1;
		}

		public override void Update()
		{
			_frame = (int)(_numFrames * TimeActive / _displayTime);
			Lighting.AddLight(Position, Color.Pink.ToVector3() / 2);
			if (TimeActive > _displayTime)
				Kill();
		}

		public override bool UseCustomDraw => true;

		public override bool UseAdditiveBlend => true;

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Texture2D tex = ParticleHandler.GetTexture(Type);
			var DrawFrame = new Rectangle(0, _frame * tex.Height / _numFrames, tex.Width, tex.Height / _numFrames);

			for (int j = -1; j <= 1; j++) //repeat multiple times with different offset and color, for chromatic aberration effect
			{
				Vector2 posOffset = Vector2.UnitX * j * 1.5f;
				Color colorMod = (j == -1) ? new Color(255, 0, 0) : ((j == 0) ? new Color(0, 255, 0) : new Color(0, 0, 255));

				spriteBatch.Draw(tex, Position + posOffset - Main.screenPosition, DrawFrame, colorMod, Rotation, DrawFrame.Size() / 2, Scale, (_direction > 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			}
		}
	}
}
