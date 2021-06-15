using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace SpiritMod.VerletChains
{
	public struct ChainPhysics
	{
		public float drag;
		public float groundBounce;
		public float gravity;

		public ChainPhysics(float drag = 0.9f, float groundBounce = 0.5f, float gravity = 0.2f)
		{
			this.drag = drag;
			this.groundBounce = groundBounce;
			this.gravity = gravity;
		}
	}

	public class Chain
	{
		private const int STIFFNESS = 6;
		public List<ChainSegment> Segments { get; set; }
		public List<ChainVertex> Vertices { get; set; }

		public Texture2D Texture { get; set; }
		public Texture2D HeadTexture { get; set; }

		public ChainVertex FirstVertex { get; set; }
		public ChainVertex LastVertex { get; set; }

		public ChainPhysics Coefficients { get; set; }

		public Chain(Texture2D texture, float segmentLength, int segmentCount, Vector2 startPosition, ChainPhysics? physicsCoefficients = null, bool staticFirst = true, bool staticLast = true)
		{
			Texture = texture;
			HeadTexture = texture;
			Coefficients = physicsCoefficients ?? new ChainPhysics();
			PopulateLists(segmentLength, segmentCount, startPosition, staticFirst, staticLast);
		}

		public Chain(Texture2D texture, Texture2D textureHead, float segmentLength, int segmentCount, Vector2 startPosition, ChainPhysics? physicsCoefficients = null, bool staticFirst = true, bool staticLast = true)
		{
			Texture = texture;
			HeadTexture = textureHead;
			Coefficients = new ChainPhysics();
			PopulateLists(segmentLength, segmentCount, startPosition, staticFirst, staticLast);
		}

		private void PopulateLists(float segmentLength, int segmentCount, Vector2 startPosition, bool staticFirst = true, bool staticLast = true)
		{
			Segments = new List<ChainSegment>();
			Vertices = new List<ChainVertex>();

			for (int i = 0; i < segmentCount; i++)
			{
				Vertices.Add(new ChainVertex(startPosition + new Vector2(0, segmentLength*i), 1f, Coefficients.drag, Coefficients.groundBounce, Coefficients.gravity));
			}

			for (int i = 0; i < segmentCount-1; i++)
			{
				Segments.Add(new ChainSegment(Vertices[i], Vertices[i + 1], segmentLength));
			}

			FirstVertex = Vertices.First();
			FirstVertex.Static = staticFirst;

			LastVertex = Vertices.Last();
			LastVertex.Static = staticLast;
		}

		public void Update(Vector2 startPosition, Vector2 endPosition)
		{
			FirstVertex.StaticPos = startPosition;
			LastVertex.StaticPos = endPosition;

			foreach (var vertex in Vertices)
			{
				vertex.Update();
				//vertex.StandardConstrain();
				vertex.SetStatic();
			}

			for (int i = 0; i < STIFFNESS; i++)
			{
				foreach (var segment in Segments)
				{
					segment.ConstrainLine();
				}
			}
		}

		public void Draw(SpriteBatch sB)
		{
			foreach (var segment in Segments)
			{
				if (segment == Segments.Last())
					segment.Draw(sB, HeadTexture);
				else
					segment.Draw(sB, Texture);
			}
		}

		public void Draw(SpriteBatch sB, out float endRotation, out Vector2 endPosition)
		{
			endRotation = 0;
			endPosition = Vector2.Zero;
			foreach (var segment in Segments) {
				segment.Draw(sB, Texture);
				endPosition = segment.Vertex2.Position;
				Vector2 delta = segment.Vertex1.Position - segment.Vertex2.Position;
				endRotation = (float)Math.Atan2(delta.Y, delta.X);
			}
		}
	}
}
