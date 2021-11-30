using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SpiritMod.Prim
{
	/// <summary>
	/// Draws a strip of rectangles in the pattern of an arc, given an input center position, starting and ending distances from the position, and the angle range
	/// </summary>
	public class PrimitiveSlashArc : IPrimitiveShape
	{
		public PrimitiveType GetPrimitiveType => PrimitiveType.TriangleStrip;
		public Vector2 BasePosition { get; set; }
		public Vector2 DirectionUnit { get; set; }
		public float StartDistance { get; set; } = 0;
		public float EndDistance { get; set; }
		public Vector2 AngleRange { get; set; }
		public Color Color { get; set; }
		public float SlashProgress { get; set; }
		public int RectangleCount { get; set; } = 20; 


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

			for(int i = 0; i <= RectangleCount; i++)
			{
				float progress = i / (float)RectangleCount;
				progress *= SlashProgress;

				float angle = MathHelper.Lerp(AngleRange.X, AngleRange.Y, progress);

				Vector2 minDistPoint = BasePosition + (DirectionUnit.RotatedBy(angle) * StartDistance);
				Vector2 maxDistPoint = BasePosition + (DirectionUnit.RotatedBy(angle) * EndDistance);

				AddVertexIndex(maxDistPoint, new Vector2(progress, 1));
				AddVertexIndex(minDistPoint, new Vector2(progress, 0));
			}

			vertices = vertexList.ToArray();
			indeces = indexList.ToArray();
		}
	}
}
