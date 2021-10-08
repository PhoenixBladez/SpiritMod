using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using System.Linq;

namespace SpiritMod.Prim
{
	/// <summary>
	/// Draws a square out of 2 triangles.
	/// </summary>
	public class SquarePrimitive : IPrimitiveShape
	{
		public PrimitiveType GetPrimitiveType => PrimitiveType.TriangleStrip;
		public Vector2 Position { get; set; }
		public float Height { get; set; }
		public float Length { get; set; }
		public Color Color { get; set; }

		/// <summary>
		/// Used for the purpose of having a pseudo 3D effect for pulse circle particles, multiplies the color based on the x coordinate of the texture coordinates
		/// </summary>
		public float ColorXCoordMod { get; set; }

		public float Rotation { get; set; }

		public void PrimitiveStructure(out VertexPositionColorTexture[] vertices, out short[] indeces)
		{
			List<VertexPositionColorTexture> vertexList = new List<VertexPositionColorTexture>();
			List<short> indexList = new List<short>();

			//Cut down a bit on boilerplate by adding a method
			void AddVertexIndex(Vector2 position, Vector2 TextureCoords)
			{
				indexList.Add((short)vertexList.Count);
				Color color = Color;
				color.R *= (byte)MathHelper.Lerp(1, ColorXCoordMod, TextureCoords.X);
				color.G *= (byte)MathHelper.Lerp(1, ColorXCoordMod, TextureCoords.X);
				color.B *= (byte)MathHelper.Lerp(1, ColorXCoordMod, TextureCoords.X);
				vertexList.Add(new VertexPositionColorTexture(new Vector3(position, 0), color, TextureCoords));
			}

			for(int x = 1; x >= -1; x -= 2) //Set corners on the left and right
			{
				for(int y = -1; y <= 1; y += 2) //Set corners on the top and bottom
				{
					Vector2 cornerPos = Position - new Vector2(x * Length / 2, y * Height / 2).RotatedBy(Rotation);
					AddVertexIndex(cornerPos, new Vector2((x + 1) / 2, (y + 1) / 2));
				}
			}

			vertices = vertexList.ToArray();
			indeces = indexList.ToArray();
		}
	}
}
