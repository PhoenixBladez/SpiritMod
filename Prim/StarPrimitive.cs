using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;

namespace SpiritMod.Prim
{
	public class StarPrimitive : IPrimitiveShape
	{
		public PrimitiveType GetPrimitiveType => PrimitiveType.TriangleList;

		public Vector2 Position { get; set; }
		public float Rotation { get; set; }
		public float TriangleWidth { get; set; }
		public float TriangleHeight { get; set; }
		public Color Color { get; set; }

		public void PrimitiveStructure(out VertexPositionColorTexture[] vertices, out short[] indeces)
		{
			List<VertexPositionColorTexture> vertexList = new List<VertexPositionColorTexture>();
			List<short> indexList = new List<short>();

			//First, set the center
			VertexPositionColorTexture Center = new VertexPositionColorTexture(new Vector3(Position, 0), Color, Vector2.Zero);
			vertexList.Add(Center);

			//Make temporary lists, for the sake of simpler index mapping
			List<VertexPositionColorTexture> tipvertexList = new List<VertexPositionColorTexture>();
			List<VertexPositionColorTexture> basevertexList = new List<VertexPositionColorTexture>();
			//create vertices for the triangle tips and inner pentagon
			for (int i = 0; i < 5; i++)
			{
				float triangleRotation = ((i / 5f) * MathHelper.TwoPi) + Rotation;
				Vector2 heightUnit = Vector2.UnitX.RotatedBy(triangleRotation);
				Vector2 widthUnit = Vector2.UnitX.RotatedBy(triangleRotation + MathHelper.PiOver2);
				Vector2 tippos = Position + (heightUnit * TriangleHeight);
				tipvertexList.Add(new VertexPositionColorTexture(new Vector3(tippos, 0), Color, Vector2.One));

				float widthRatio = TriangleWidth / TriangleHeight;
				float baseHeightoffset = TriangleHeight * widthRatio;
				float baseWidth = TriangleWidth * (1 - widthRatio);

				Vector2 basepos = Position + (heightUnit * baseHeightoffset) + (widthUnit * baseWidth);
				basevertexList.Add(new VertexPositionColorTexture(new Vector3(basepos, 0), Color, Vector2.One / 2));
			}
			//Then add them to the main list
			vertexList.AddRange(basevertexList);
			vertexList.AddRange(tipvertexList);

			/*The vertex list will now look like:
			 * ------
			 * 0- Center
			 * 1- Base vertex for points 1 and 2
			 * 2- Base vertex for points 2 and 3
			 * 3- Base vertex for points 3 and 4
			 * 4- Base vertex for points 4 and 5
			 * 5- Base vertex for points 5 and 1
			 * 6- Tip for point 1
			 * 7- Tip for point 2
			 * 8- Tip for point 3
			 * 9- Tip for point 4
			 * 10- Tip for point 5
			 * -----
			 * So we will map the vertices to triangles like so:
			 * [5, 0, 1]- base triangle for point 1
			 * [5, 6, 1]- tip triangle for point 1
			 * [1, 0, 2]- base triangle for point 2
			 * [1, 7, 2]- tip triangle for point 2
			 * etc.
			*/

			for(int i = 1; i <= 5; i++)
			{
				short leftBaseindex = (short)(1 + ((i + 5) % 5));
				short rightBaseindex = (short)i;
				short centerindex = 0;
				short tipIndex = (short)((i % 5) + 6);

				//Define the tip triangle
				indexList.Add(rightBaseindex); indexList.Add(tipIndex); indexList.Add(leftBaseindex);

				//Define the base triangle
				indexList.Add(leftBaseindex); indexList.Add(centerindex); indexList.Add(rightBaseindex);
			}

			vertices = vertexList.ToArray();
			indeces = indexList.ToArray();
		}
	}
}
