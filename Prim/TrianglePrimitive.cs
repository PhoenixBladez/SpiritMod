using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;

namespace SpiritMod.Prim
{
	public class TrianglePrimitive : IPrimitiveShape
	{
		public PrimitiveType GetPrimitiveType => PrimitiveType.TriangleList;

		public Vector2 TipPosition { get; set; }
		public float Rotation { get; set; }
		public float Width { get; set; }
		public float Height { get; set; }
		public Color Color { get; set; }

		public void PrimitiveStructure(out VertexPositionColorTexture[] vertices, out short[] indeces)
		{
			List<VertexPositionColorTexture> vertexList = new List<VertexPositionColorTexture>();
			List<short> indexList = new List<short>();

			Vector2 DirectionUnit = Vector2.UnitX.RotatedBy(Rotation);
			vertexList.Add(new VertexPositionColorTexture(new Vector3(TipPosition, 0), Color, new Vector2(0.5f, 1)));
			for (int i = -1; i <= 1; i += 2)
			{

				Vector2 LowerPoint = TipPosition + (DirectionUnit.RotatedBy(MathHelper.PiOver2) * Width * i) + (DirectionUnit * Height);

				vertexList.Add(new VertexPositionColorTexture(new Vector3(LowerPoint, 0), Color, new Vector2(i == 1 ? 1 : 0, 0)));
			}

			indexList.Add(0); indexList.Add(1); indexList.Add(2);


			vertices = vertexList.ToArray();
			indeces = indexList.ToArray();
		}
	}
}
