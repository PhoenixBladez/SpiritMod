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
	public class FlameTrail : BaseTrail
	{
		public float DissolveSpeed { get; set; }
		private List<Vector2> _points;
		private readonly int _maxPoints;
		private int _pointsToKill;
		private float _width;
		private float _deathProgress;
		private readonly Color _startColor;
		private readonly Color _midColor;
		private readonly Color _endColor;

		public FlameTrail(Projectile projectile, Color startColor, Color midColor, Color endColor, float width, int maxPoints, float opacity = 1, TrailLayer layer = TrailLayer.UnderProjectile) : base(projectile, layer)
		{
			_points = new List<Vector2>();
			_width = width;
			_maxPoints = maxPoints;
			_deathProgress = 1f;
			_startColor = startColor;
			_midColor = midColor;
			_endColor = endColor;
		}

		public override void Dissolve()
		{
			_deathProgress = _points.Count / (float)_pointsToKill;

			if(_points.Count > 0)
				_points.RemoveAt(_points.Count - 1);

			if (_points.Count == 0)
				Dead = true;
		}

		public override void OnStartDissolve() => _pointsToKill = _points.Count;

		public override void Update()
		{
			_points.Insert(0, MyProjectile.Center);
			if (_points.Count > _maxPoints)
				_points.RemoveAt(_points.Count - 1);
		}

		public override void Draw(Effect effect, BasicEffect effect2, GraphicsDevice device)
		{
			if (Dead || _points.Count <= 1) return;

			//set the parameters for the shader
			Effect flametrailEffect = ModContent.Request<Effect>("Effects/FlameTrail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			flametrailEffect.Parameters["uTexture"].SetValue(ModContent.Request<Texture2D>("Textures/Trails/Trail_3", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
			flametrailEffect.Parameters["uTexture2"].SetValue(ModContent.Request<Texture2D>("Textures/Trails/Trail_4", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
			flametrailEffect.Parameters["Progress"].SetValue(Main.GlobalTimeWrappedHourly * -1f);
			flametrailEffect.Parameters["xMod"].SetValue(1.5f);
			flametrailEffect.Parameters["StartColor"].SetValue(_startColor.ToVector4());
			flametrailEffect.Parameters["MidColor"].SetValue(_midColor.ToVector4());
			flametrailEffect.Parameters["EndColor"].SetValue(_endColor.ToVector4());


			float getWidthMod(float progress = 0) => ((float)Math.Sin((Main.GlobalTimeWrappedHourly - progress) * MathHelper.TwoPi * 1.5f) * 0.33f + 1.33f) / (float)Math.Pow(1 - progress, 0.1f);
			IPrimitiveShape[] shapesToDraw = new IPrimitiveShape[]
			{
				new CirclePrimitive
				{
					Color = Color.White * _deathProgress,
					Radius = _width * _deathProgress * getWidthMod(),
					Position = _points[0] - Main.screenPosition,
					MaxRadians = MathHelper.TwoPi
				},
				new PrimitiveStrip
				{
					Color = Color.White * _deathProgress,
					Width = _width * _deathProgress,
					PositionArray = _points.ToArray(),
					TaperingType = StripTaperType.TaperEnd,
					WidthDelegate = delegate (float progress) { return getWidthMod(progress); }
				}
			};

			PrimitiveRenderer.DrawPrimitiveShapeBatched(shapesToDraw, flametrailEffect);
		}
	}
}