using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using System;

namespace SpiritMod.Prim
{
	/// <summary>
	/// Draws a strip of triangles to create a circle, or partial circle
	/// </summary>
	public class CirclePrimitive : IPrimitiveShape
	{
		public PrimitiveType GetPrimitiveType => PrimitiveType.TriangleStrip;
		public Vector2 Position { get; set; }
		public float Radius { get; set; }
		public Color Color { get; set; }

		public float Rotation { get; set; }
		public float MaxRadians { get; set; } = MathHelper.TwoPi;

		public void PrimitiveStructure(out VertexPositionColorTexture[] vertices, out short[] indeces)
		{
			List<VertexPositionColorTexture> vertexList = new List<VertexPositionColorTexture>();
			List<short> indexList = new List<short>();

			//Cut down a bit on boilerplate by adding a method
			void AddVertexIndex(Vector2 position, Vector2 TextureCoords)
			{
				indexList.Add((short)vertexList.Count);
				vertexList.Add(new VertexPositionColorTexture(new Vector3(position, 0), Color, TextureCoords));
			}

			AddVertexIndex(Position, new Vector2(0.5f, 0.5f));
			int maxTriangles = 20;
			for (int i = 0; i <= maxTriangles; i++)
			{
				float progress = (i / (float)maxTriangles);
				Vector2 vertex = Position - (Radius * Vector2.UnitX.RotatedBy((MaxRadians * progress) + Rotation + (MaxRadians / 2)));
				AddVertexIndex(vertex, new Vector2(1f, 0f));
				AddVertexIndex(Position, new Vector2(0.5f, 0f));
			}

			vertices = vertexList.ToArray();
			indeces = indexList.ToArray();
		}
	}
}
