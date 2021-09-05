using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using System;
using Terraria;

namespace SpiritMod.NPCs.Boss.Occultist.Particles
{
	public class OccultistTelegraphBeam : Particle
	{
		private readonly float _angle = 0;
		private readonly float _maxLength = 0;
		private float _length = 0;
		private readonly float _maxTime = 0;
		private float _opacity = 0;
		private readonly NPC _parent;

		private readonly Color drawColor = Color.Red;

		public OccultistTelegraphBeam(NPC Parent, float angle, float length, int maxtime)
		{
			_parent = Parent;
			_angle = angle;
			_maxLength = length;
			Position = Parent.Center;
			_maxTime = maxtime;
		}

		public override void Update()
		{
			Position = _parent.Center;
			float progress = (float)Math.Sin(Math.Pow(TimeActive / _maxTime, 1) * MathHelper.Pi);
			_opacity = progress * 0.7f;
			_length = progress * _maxLength;
			if (TimeActive > _maxTime)
				Kill();
		}

		public override bool UseCustomDraw => true;

		public override bool UseAdditiveBlend => true;

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Texture2D tex = ParticleHandler.GetTexture(Type);
			var scale = new Vector2(1, _length / tex.Height);
			float rotation = _angle - MathHelper.PiOver2;
			var origin = new Vector2(tex.Width / 2f, 0);

			spriteBatch.Draw(tex, Position - Main.screenPosition, null, drawColor * _opacity, rotation, origin, scale, SpriteEffects.None, 0);
		}
	}
}