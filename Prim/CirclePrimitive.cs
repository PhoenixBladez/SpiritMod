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
		public float Radius { get; set; } = 1;
		public Color Color { get; set; }
		public Vector2 ScaleModifier { get; set; } = Vector2.One;

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
			int maxTriangles = 30;
			for (int i = 0; i <= maxTriangles; i++)
			{
				float progress = (i / (float)maxTriangles);
				float vertRotation = (MaxRadians * progress);

				Vector2 vertex = Position + new Vector2(Radius * ScaleModifier.Y * (float)Math.Cos(vertRotation), Radius * ScaleModifier.X * (float)Math.Sin(vertRotation)).RotatedBy(Rotation);
				AddVertexIndex(vertex, new Vector2(1f, 0f));
				AddVertexIndex(Position, new Vector2(0.5f, 0f));
			}

			vertices = vertexList.ToArray();
			indeces = indexList.ToArray();
		}
	}
}
