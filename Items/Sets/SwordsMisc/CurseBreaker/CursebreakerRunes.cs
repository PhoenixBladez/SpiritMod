using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SwordsMisc.CurseBreaker
{
	public class CursebreakerRunes : Particle
	{
		private Color _startColor;
		private Color _endColor;
		public int MaxTime;

		private const float FADETIME = 0.3f;

		public delegate void UpdateAction(Particle particle);

		private readonly UpdateAction _action;

		public override bool UseAdditiveBlend => true;

		public override bool UseCustomDraw => true;
		private readonly int frame;

		public CursebreakerRunes(Vector2 position, Vector2 velocity, Color startColor, Color endColor, float scale, int maxTime, UpdateAction action = null)
		{
			Position = position;
			Velocity = velocity;
			_startColor = startColor;
			_endColor = endColor;
			Scale = scale;
			MaxTime = maxTime;
			_action = action;
		}

		public override void Update()
		{
			float fadeintime = MaxTime * FADETIME;
			Color = Color.Lerp(_startColor, _endColor, TimeActive / (float)MaxTime);
			if (TimeActive < fadeintime)
				Color *= (TimeActive / fadeintime);
			else if (TimeActive > (MaxTime - fadeintime))
				Color *= ((MaxTime - TimeActive) / fadeintime);

			Lighting.AddLight(Position, Color.ToVector3() * Scale * 0.5f);

			if (_action != null)
				_action.Invoke(this);

			if (TimeActive > MaxTime)
				Kill();
		}

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Texture2D basetexture = ParticleHandler.GetTexture(Type);

			Rectangle drawframe = new Rectangle(0, frame * basetexture.Height / 4, basetexture.Width, basetexture.Height / 4);
			spriteBatch.Draw(basetexture, Position - Main.screenPosition, drawframe, Color, Rotation, drawframe.Size() / 2, Scale, SpriteEffects.None, 0);
		}
	}
}
