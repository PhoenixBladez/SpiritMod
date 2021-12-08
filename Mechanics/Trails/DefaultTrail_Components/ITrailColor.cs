using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;

namespace SpiritMod.Mechanics.Trails
{
	public interface ITrailColor
	{
		Color GetColourAt(float distanceFromStart, float trailLength, List<Vector2> points);
	}

	#region Different Trail Color Types
	public class GradientTrail : ITrailColor
	{
		private Color _startColour;
		private Color _endColour;

		public GradientTrail(Color start, Color end)
		{
			_startColour = start;
			_endColour = end;
		}

		public Color GetColourAt(float distanceFromStart, float trailLength, List<Vector2> points)
		{
			float progress = distanceFromStart / trailLength;
			return Color.Lerp(_startColour, _endColour, progress) * (1f - progress);
		}
	}

	public class RainbowTrail : ITrailColor
	{
		private float _saturation;
		private float _lightness;
		private float _speed;
		private float _distanceMultiplier;

		public RainbowTrail(float animationSpeed = 5f, float distanceMultiplier = 0.01f, float saturation = 1f, float lightness = 0.5f)
		{
			_saturation = saturation;
			_lightness = lightness;
			_distanceMultiplier = distanceMultiplier;
			_speed = animationSpeed;
		}

		public Color GetColourAt(float distanceFromStart, float trailLength, List<Vector2> points)
		{
			float progress = distanceFromStart / trailLength;
			float hue = (Main.GlobalTime * _speed + distanceFromStart * _distanceMultiplier) % MathHelper.TwoPi;
			return ColorFromHSL(hue, _saturation, _lightness) * (1f - progress);
		}

		//Borrowed methods for converting HSL to RGB
		private Color ColorFromHSL(float h, float s, float l)
		{
			h /= MathHelper.TwoPi;

			float r = 0, g = 0, b = 0;
			if (l != 0)
			{
				if (s == 0)
					r = g = b = l;
				else
				{
					float temp2;
					if (l < 0.5f)
						temp2 = l * (1f + s);
					else
						temp2 = l + s - l * s;

					float temp1 = 2f * l - temp2;

					r = GetColorComponent(temp1, temp2, h + 0.33333333f);
					g = GetColorComponent(temp1, temp2, h);
					b = GetColorComponent(temp1, temp2, h - 0.33333333f);
				}
			}
			return new Color(r, g, b);
		}
		private float GetColorComponent(float temp1, float temp2, float temp3)
		{
			if (temp3 < 0f)
				temp3 += 1f;
			else if (temp3 > 1f)
				temp3 -= 1f;

			if (temp3 < 0.166666667f)
				return temp1 + (temp2 - temp1) * 6f * temp3;
			else if (temp3 < 0.5f)
				return temp2;
			else if (temp3 < 0.66666666f)
				return temp1 + (temp2 - temp1) * (0.66666666f - temp3) * 6f;
			else
				return temp1;
		}
	}

	public class StandardColorTrail : ITrailColor
	{
		private Color _colour;

		public StandardColorTrail(Color colour)
		{
			_colour = colour;
		}

		public Color GetColourAt(float distanceFromStart, float trailLength, List<Vector2> points)
		{
			float progress = distanceFromStart / trailLength;
			return _colour * (1f - progress);
		}
	}

	public class StarjinxTrail : ITrailColor
	{
		private float _start;
		private readonly float _speed;
		private readonly float _opacity;
		private Projectile _proj;

		public StarjinxTrail(float StartingTimer, float AnimationSpeed = 1f, float Opacity = 1f)
		{
			_start = StartingTimer;
			_speed = AnimationSpeed;
			_opacity = Opacity;
		}
		public StarjinxTrail(Projectile projectile, float StartingTimer, float AnimationSpeed = 1f, float Opacity = 1f)
		{
			_proj = projectile;
			_start = StartingTimer;
			_speed = AnimationSpeed;
			_opacity = Opacity;
		}

		public Color GetColourAt(float distanceFromStart, float trailLength, List<Vector2> points)
		{
			float progress = distanceFromStart / trailLength;
			Color returnColor = Color.Lerp(SpiritMod.StarjinxColor(_start + Main.GlobalTime * _speed), SpiritMod.StarjinxColor(_start + 5 + Main.GlobalTime * _speed), progress) * (1f - progress) * _opacity;
			if (_proj != null)
			{
				return returnColor * _proj.Opacity;
			}
			return returnColor;
		}
	}

	public class OpacityUpdatingTrail : ITrailColor
	{

		private Color _startcolor;
		private Color _endcolor;
		private Projectile _proj;
		private float _opacity = 1f;

		public OpacityUpdatingTrail(Projectile proj, Color color)
		{
			_startcolor = color;
			_endcolor = color;
			_proj = proj;
		}

		public OpacityUpdatingTrail(Projectile proj, Color startColor, Color endColor)
		{
			_startcolor = startColor;
			_endcolor = endColor;
			_proj = proj;
		}

		public Color GetColourAt(float distanceFromStart, float trailLength, List<Vector2> points)
		{
			float progress = distanceFromStart / trailLength;
			if (_proj.active && _proj != null)
				_opacity = _proj.Opacity;

			return Color.Lerp(_startcolor, _endcolor, progress) * (1f - progress) * _opacity;
		}
	}
	#endregion
}