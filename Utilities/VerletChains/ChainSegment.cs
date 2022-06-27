using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;

namespace SpiritMod.VerletChains
{
	public class ChainSegment
	{
		public ChainSegment(ChainVertex vertex1, ChainVertex vertex2, float length)
		{
			Vertex1 = vertex1;
			Vertex2 = vertex2;
			Length = length;
		}

		public Texture2D Texture { get; set; }
		public Rectangle DrawRectangle { get; set; }
		public ChainVertex Vertex1 { get; set; }
		public ChainVertex Vertex2 { get; set; }
		public float Length { get; set; }

		public float Rotation()
		{
			Vector2 delta = (Vertex1.Position - Vertex2.Position);
			return (float)Math.Atan2(delta.Y, delta.X);
		}

		public void ConstrainLine()
		{
			Vector2 delta = Vertex2.Position - Vertex1.Position;
			float distance = delta.Length();

			float fraction = ((Length - distance) / Math.Max(distance, 1)) / 2;

			delta *= fraction;

			if (!Vertex1.Static)
				Vertex1.Position -= delta;
			if (!Vertex2.Static)
				Vertex2.Position += delta;
		}

		public void Draw(SpriteBatch sB)
		{
			Vector2 delta = (Vertex1.Position - Vertex2.Position);
			float rotation = (float)Math.Atan2(delta.Y, delta.X);

			sB.Draw(TextureAssets.MagicPixel.Value, Vertex2.Position - Main.screenPosition, null, Color.White, rotation, new Vector2(0f, 0.5f), new Vector2(delta.Length(), 2f), SpriteEffects.None, 0);
		}

		public void Draw(SpriteBatch sB, Texture2D texture, float scale, Color color = default)
		{
			sB.Draw(texture, Vertex2.Position - Main.screenPosition, null, Lighting.GetColor((int)Vertex2.Position.X / 16, (int)(Vertex2.Position.Y / 16.0)), Rotation() + MathHelper.PiOver2, texture.Size()/2f, scale, SpriteEffects.None, 0);
		}
	}
}
