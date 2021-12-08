using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod;
using SpiritMod.Particles;
using SpiritMod.Utilities;
using System;
using Terraria;

namespace SpiritMod.Items.Sets.SummonsMisc.SanguineFlayer
{
	public class SanguineFlayerRip : Particle
	{
		private const int _numFrames = 5;
		private int _frame;
		private readonly int _direction;
		private const int _displayTime = 20;

		public SanguineFlayerRip(Vector2 position, float scale, float rotation)
		{
			Position = position;
			Scale = scale;
			_direction = Main.rand.NextBool() ? -1 : 1;
			Rotation = (_direction < 0) ? (rotation - MathHelper.Pi) : rotation;
		}

		public override void Update()
		{
			_frame = (int)(_numFrames * TimeActive / _displayTime);
			if (TimeActive >= _displayTime)
				Kill();
		}

		public override bool UseCustomDraw => true;

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Texture2D tex = ParticleHandler.GetTexture(Type);
			var DrawFrame = new Rectangle(0, _frame * tex.Height / _numFrames, tex.Width, tex.Height / _numFrames);
			Vector2 origin = new Vector2(60, 40); //Hardcoded to make texture centered properly
			Color lightColor = Lighting.GetColor((int)Position.X / 16, (int)Position.Y / 16);
			spriteBatch.Draw(tex, Position - Main.screenPosition, DrawFrame, lightColor, Rotation, origin, Scale, (_direction > 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
		}
	}
}
