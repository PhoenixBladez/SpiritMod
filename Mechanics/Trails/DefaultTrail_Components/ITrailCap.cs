using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace SpiritMod.Mechanics.Trails
{
	public interface ITrailCap
	{
		int ExtraTris { get; }
		void AddCap(VertexPositionColorTexture[] array, ref int currentIndex, Color colour, Vector2 position, Vector2 startNormal, float width);
	}

	#region Different Trail Caps
	public class RoundCap : ITrailCap
	{
		public int ExtraTris => 20;

		public void AddCap(VertexPositionColorTexture[] array, ref int currentIndex, Color colour, Vector2 position, Vector2 startNormal, float width)
		{
			//initial info
			float halfWidth = width * 0.5f;
			float arcStart = startNormal.ToRotation();
			float arcAmount = MathHelper.Pi;
			//int segments = (int)Math.Ceiling(6 * Math.Sqrt(halfWidth) * (arcAmount / MathHelper.TwoPi));
			int segments = ExtraTris;
			float theta = arcAmount / segments;
			float cos = (float)Math.Cos(theta);
			float sin = (float)Math.Sin(theta);
			float t;
			float x = (float)Math.Cos(arcStart) * halfWidth;
			float y = (float)Math.Sin(arcStart) * halfWidth;

			position -= Main.screenPosition;

			//create initial vertices
			var center = new VertexPositionColorTexture(new Vector3(position.X, position.Y, 0f), colour, Vector2.One * 0.5f);
			var prev = new VertexPositionColorTexture(new Vector3(position.X + x, position.Y + y, 0f), colour, Vector2.One);

			for (int i = 0; i < segments; i++)
			{
				//apply matrix transformation
				t = x;
				x = cos * x - sin * y;
				y = sin * t + cos * y;

				var next = new VertexPositionColorTexture(new Vector3(position.X + x, position.Y + y, 0f), colour, Vector2.One);

				//Add triangle vertices
				array[currentIndex++] = center;
				array[currentIndex++] = prev;
				array[currentIndex++] = next;

				prev = next;
			}
		}
	}

	public class TriangleCap : ITrailCap
	{
		public int ExtraTris => 1;

		private readonly float _widthmod;
		private readonly float _length;
		public TriangleCap(float width = 1f, float length = 1f)
		{
			_widthmod = width;
			_length = length;
		}

		public void AddCap(VertexPositionColorTexture[] array, ref int currentIndex, Color colour, Vector2 position, Vector2 startNormal, float width)
		{
			width *= _widthmod;
			float rotation = startNormal.ToRotation();
			float halfwidth = width / 2;
			Vector2 TipPos = position + Vector2.UnitY.RotatedBy(rotation) * width * _length - Main.screenPosition;
			Vector2 LeftBasePos = position + Vector2.UnitY.RotatedBy(rotation + MathHelper.PiOver2) * halfwidth - Main.screenPosition;
			Vector2 RightBasePos = position + Vector2.UnitY.RotatedBy(rotation - MathHelper.PiOver2) * halfwidth - Main.screenPosition;

			array[currentIndex++] = new VertexPositionColorTexture(new Vector3(LeftBasePos, 0), colour, Vector2.Zero);
			array[currentIndex++] = new VertexPositionColorTexture(new Vector3(RightBasePos, 0), colour, Vector2.One);
			array[currentIndex++] = new VertexPositionColorTexture(new Vector3(TipPos, 0), colour, new Vector2(0.5f, 1f));
		}
	}

	public class NoCap : ITrailCap
	{
		public int ExtraTris => 0;

		public void AddCap(VertexPositionColorTexture[] array, ref int currentIndex, Color colour, Vector2 position, Vector2 startNormal, float width)
		{

		}
	}
	#endregion
}