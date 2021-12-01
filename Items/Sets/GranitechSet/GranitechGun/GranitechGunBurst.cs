using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod;
using SpiritMod.Particles;
using SpiritMod.Utilities;
using System;
using Terraria;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechGun
{
	public class GranitechGunBurst : Particle
	{
		private const int _numFrames = 4;
		private int _frame;
		private readonly int _direction;
		private const int _displayTime = 15;

		public GranitechGunBurst(Vector2 position, float scale, float rotation)
		{
			Position = position;
			Scale = scale;
			_direction = Main.rand.NextBool() ? -1 : 1;
			Rotation = (_direction > 0) ? (rotation - MathHelper.Pi) : rotation;
		}

		public override void Update()
		{
			_frame = (int)(_numFrames * TimeActive / _displayTime);
			Lighting.AddLight(Position, Color.Pink.ToVector3() / 2);
			if (TimeActive > _displayTime)
				Kill();
		}

		public override bool UseCustomDraw => true;

		public override bool UseAdditiveBlend => false;

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Texture2D tex = ParticleHandler.GetTexture(Type);
			var DrawFrame = new Rectangle(0, _frame * tex.Height / _numFrames, tex.Width, tex.Height / _numFrames);

			DrawAberration.DrawChromaticAberration(Vector2.UnitX.RotatedBy(Rotation), 1.5f, delegate (Vector2 offset, Color colorMod)
			{
				spriteBatch.Draw(tex, Position + offset - Main.screenPosition, DrawFrame, colorMod, Rotation, DrawFrame.Size() / 2, Scale, (_direction > 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			});
		}
	}
}
