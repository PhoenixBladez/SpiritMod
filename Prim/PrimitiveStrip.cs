using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SpiritMod.Prim
{
	public enum StripTaperType
	{
		TaperEnd,
		TaperStart,
		None
	}

	/// <summary>
	/// Draws a strip of rectangles through a given array of positions, tapering and ending with a triagle towards the start or end of the array, or not at all
	/// </summary>
	public class PrimitiveStrip : IPrimitiveShape
	{
		public PrimitiveType GetPrimitiveType => PrimitiveType.TriangleStrip;
		public Vector2[] PositionArray { get; set; }
		public float Width { get; set; }
		public Color Color { get; set; }
		public StripTaperType TaperingType { get; set; }

		public delegate float StripWidthDelagate(float progress);

		public StripWidthDelagate WidthDelegate { get; set; } = null;

		public void PrimitiveStructure(out VertexPositionColorTexture[] vertices, out short[] indeces)
		{
			List<VertexPositionColorTexture> vertexList = new List<VertexPositionColorTexture>();
			List<short> indexList = new List<short>();

			//Cut down a bit on boilerplate by adding a method
			void AddVertexIndex(Vector2 position, Vector2 TextureCoords)
			{
				indexList.Add((short)vertexList.Count);
				vertexList.Add(new VertexPositionColorTexture(new Vector3(position - Main.screenPosition, 0), Color, TextureCoords));
			}

			//Check if the array is not too small first
			if(PositionArray.Length >= 2)
			{
				//Iterate through the given array of positions
				for (int i = 0; i < PositionArray.Length - 1; i++)
				{
					int start = 0;
					int end = PositionArray.Length - 2;

					float progress = (i + 1) / (float)PositionArray.Length;

					//Modify width of the triangles based on progress through iterating through the array
					float widthModifier = 1;
					switch (TaperingType)
					{
						case StripTaperType.TaperStart:
							widthModifier *= progress;
							break;
						case StripTaperType.TaperEnd:
							widthModifier *= (1 - progress);
							break;
					}
					if (WidthDelegate != null)
						widthModifier *= WidthDelegate.Invoke(progress);

					//If on the first element of the array, add the vertices corresponding to the front of the trail
					if (i == start)
					{
						Vector2 currentPosition = PositionArray[i];
						if (TaperingType == StripTaperType.TaperStart) //Only add the center point if set to taper at the start
							AddVertexIndex(currentPosition, new Vector2(0.5f, 0));

						else
						{
							Vector2 currentWidthUnit = CurveNormal(PositionArray.ToList(), i);

							float startWidth = WidthDelegate == null ? 1 : WidthDelegate.Invoke(0);
							Vector2 currentLeft = currentPosition - (currentWidthUnit * Width * startWidth);
							Vector2 currentRight = currentPosition + (currentWidthUnit * Width * startWidth);

							AddVertexIndex(currentRight, new Vector2(1, 0));
							AddVertexIndex(currentLeft, new Vector2(0, 0));
						}
					}

					Vector2 nextPosition = PositionArray[i + 1];
					if (i == end && TaperingType == StripTaperType.TaperEnd) //Only add the center point if set to taper at the end
						AddVertexIndex(nextPosition, new Vector2(0.5f, 1));

					else //Add vertices based on the next position of the array
					{
						Vector2 nextWidthUnit = CurveNormal(PositionArray.ToList(), i + 1);

						Vector2 nextLeft = nextPosition - (nextWidthUnit * Width * widthModifier);
						Vector2 nextRight = nextPosition + (nextWidthUnit * Width * widthModifier);
						AddVertexIndex(nextRight, new Vector2(1, progress));
						AddVertexIndex(nextLeft, new Vector2(0, progress));
					}
				}
			}

			vertices = vertexList.ToArray();
			indeces = indexList.ToArray();
		}

		//Helper methods I borrowed directly from trailhelper, add to static helper class eventually
		private Vector2 CurveNormal(List<Vector2> points, int index)
		{
			if (points.Count == 1) return points[0];

			if (index == 0)
			{
				return Clockwise90(Vector2.Normalize(points[1] - points[0]));
			}
			if (index == points.Count - 1)
			{
				return Clockwise90(Vector2.Normalize(points[index] - points[index - 1]));
			}
			return Clockwise90(Vector2.Normalize(points[index] - points[index - 1]));
		}

		private Vector2 Clockwise90(Vector2 vector)
		{
			return new Vector2(-vector.Y, vector.X);
		}
	}
}
