using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Prim;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.Trails.CustomTrails
{
	public class VNoiseMotionTrail : BaseTrail
	{
		public float DissolveSpeed { get; set; }
		private Vector2 _startPoint;
		private Vector2 _endPoint;
		private readonly float _startWidth;
		private readonly float _startOpacity;
		private float _opacity;
		private float _width;
		private readonly Color _color;
		private const int MAXTIMELEFT = 12;
		private int _timeLeft = MAXTIMELEFT;

		public VNoiseMotionTrail(Projectile projectile, Color color, float width, float opacity, TrailLayer layer = TrailLayer.UnderProjectile) : base(projectile, layer)
		{
			_startPoint = projectile.Center;
			_endPoint = projectile.Center;
			_startWidth = width;
			_width = width;
			_color = color;
			_startOpacity = opacity;
			_opacity = opacity;
		}

		public override void Dissolve()
		{
			_timeLeft--;
			_width = _startWidth * ((_timeLeft / (float)MAXTIMELEFT) /2 + 0.5f);
			_opacity = _startOpacity * (_timeLeft / (float)MAXTIMELEFT);
			if (_timeLeft <= 0)
				Dead = true;
		}

		public override void Update() => _endPoint = MyProjectile.Center;

		public override void Draw(Effect effect, GraphicsDevice device)
		{
			if (Dead) return;
			
			//set the parameters for the shader
			Effect noiseTrailEffect = ModContent.Request<Effect>("SpiritMod/Effects/MotionNoiseTrail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			noiseTrailEffect.Parameters["uTexture"].SetValue(ModContent.Request<Texture2D>("SpiritMod/Textures/voronoiLooping", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
			noiseTrailEffect.Parameters["uColor"].SetValue(_color.ToVector4());
			noiseTrailEffect.Parameters["progress"].SetValue(Main.GlobalTimeWrappedHourly * -1.5f);
			float length = (_startPoint - _endPoint).Length();

			noiseTrailEffect.Parameters["xMod"].SetValue(length / 800f);
			noiseTrailEffect.Parameters["yMod"].SetValue(0.66f);

			Vector2 center = Vector2.Lerp(_startPoint, _endPoint, 0.5f);
			float rotation = (_startPoint - _endPoint).ToRotation();
			Color lightColor = Lighting.GetColor((int)center.X / 16, (int)center.Y / 16);
			SquarePrimitive squarePrimitive = new SquarePrimitive
			{
				Height = _width,
				Length = length,
				Color = lightColor * _opacity,
				Position = center - Main.screenPosition,
				Rotation = rotation
			};

			PrimitiveRenderer.DrawPrimitiveShape(squarePrimitive, noiseTrailEffect);
		}
	}
}