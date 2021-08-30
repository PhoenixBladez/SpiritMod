using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using SpiritMod.Utilities;
using SpiritMod.Mechanics.EventSystem.Controllers;

using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;

namespace SpiritMod.Mechanics.EventSystem.Events
{
	public class ScreenFlash : Event
	{
		private readonly Color _color;
		private readonly float _maxTime;
		private readonly EaseBuilder _opacity;
		public ScreenFlash(Color color, float fadeInTime, float fadeOutTime, float peakOpacity)
		{
			_color = color;
			_maxTime = fadeInTime + fadeOutTime;

			_opacity = new EaseBuilder();
			_opacity.AddPoint(0f, 0f, EaseFunction.Linear);
			_opacity.AddPoint(fadeInTime, peakOpacity, EaseFunction.EaseQuadIn);
			_opacity.AddPoint(_maxTime, 0, EaseFunction.EaseQuadIn);
		}

		public override bool Update(float deltaTime)
		{
			_currentTime += deltaTime;
			return _currentTime >= _maxTime;
		}

		public override void DrawAtLayer(SpriteBatch spriteBatch, RenderLayers layer, bool beginSB)
		{
			if (layer == RenderLayers.All)
			{
				if (beginSB) spriteBatch.Begin();

				spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), null, _color * _opacity.Ease(_currentTime));

				if (beginSB) spriteBatch.End();
			}
		}
	}
}
