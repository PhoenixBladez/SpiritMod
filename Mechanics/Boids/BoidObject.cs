using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.Interfaces;
using System;
using System.Collections.Generic;
using Terraria;

namespace SpiritMod.Mechanics.Boids
{
	internal class Fish : Entity, IComponent
	{
		public Vector2 Acceleration { get; set; }

		public const float Vision = 100;
		private const float MaxForce = 0.02f;
		private const float MaxVelocity = 2f;

		private int Frame = 0;
		private int TextureID = 0;
		private int SpawnTimer = 0;

		private Flock parent;

		public List<Fish> AdjFish = new List<Fish>();

		public Fish(Flock flock, int texID = -1)
		{
			parent = flock;

			TextureID = texID == -1 ? Main.rand.Next(flock.FlockTextures.Length) : 1;
			SpawnTimer = 100;
		}

		Vector2 Limit(Vector2 vec, float val)
		{
			if (vec.LengthSquared() > val * val)
				return Vector2.Normalize(vec) * val;
			return vec;
		}

		public Vector2 AvoidTiles(int range) //WIP for Qwerty
		{
			Vector2 sum = new Vector2(0, 0);
			Point tilePos = position.ToTileCoordinates();

			const int TileRange = 2;

			for (int i = -TileRange; i < TileRange + 1; i++)
			{
				for (int j = -TileRange; j < TileRange + 1; j++)
				{
					if (WorldGen.InWorld(tilePos.X + i, tilePos.Y + j, 10))
					{
						Tile tile = Framing.GetTileSafely(tilePos.X + i, tilePos.Y + j);
						float pdist = Vector2.DistanceSquared(position, new Vector2(tilePos.X + i, tilePos.Y + j) * 16);
						if (pdist < range * range && pdist > 0 && (tile.active() && Main.tileSolid[tile.type] || tile.liquid < 100))
						{
							Vector2 d = position - new Vector2(tilePos.X + i, tilePos.Y + j) * 16;
							Vector2 norm = Vector2.Normalize(d);
							Vector2 weight = norm;
							sum += weight;
						}
					}
				}
			}

			if (sum != Vector2.Zero)
			{
				sum = Vector2.Normalize(sum) * MaxVelocity;
				Vector2 acc = sum - velocity;
				return Limit(acc, MaxForce);
			}
			return Vector2.Zero;
		}

		//Avoid you [Client Side]
		//TODO: Entity Pass, not client side maybe?
		public Vector2 AvoidHooman(int range)
		{
			float pdist = Vector2.DistanceSquared(position, Main.LocalPlayer.Center);
			Vector2 sum = new Vector2(0, 0);

			if (pdist < range * range && pdist > 0)
			{
				Vector2 d = position - Main.LocalPlayer.Center;
				Vector2 norm = Vector2.Normalize(d);
				Vector2 weight = norm;
				sum += weight;
			}

			if (sum != Vector2.Zero)
			{
				sum = Vector2.Normalize(sum) * MaxVelocity;
				Vector2 acc = sum - velocity;
				return Limit(acc, MaxForce);
			}
			return Vector2.Zero;
		}

		//Cant overlap
		public Vector2 Seperation(int range)
		{
			int count = 0;
			Vector2 sum = new Vector2(0, 0);
			for (int j = 0; j < AdjFish.Count; j++)
			{
				var OtherFish = AdjFish[j];
				float dist = Vector2.DistanceSquared(position, OtherFish.position);
				if (dist < range * range && dist > 0)
				{
					Vector2 d = position - OtherFish.position;
					Vector2 norm = Vector2.Normalize(d);
					Vector2 weight = norm / dist;
					sum += weight;
					count++;
				}
			}

			if (count > 0)
				sum /= count;

			if (sum != Vector2.Zero)
			{
				sum = Vector2.Normalize(sum) * MaxVelocity;
				Vector2 acc = sum - velocity;
				return Limit(acc, MaxForce);
			}
			return Vector2.Zero;
		}

		//Must face the same general direction
		public Vector2 Allignment(int range)
		{
			int count = 0;
			Vector2 sum = new Vector2(0, 0);
			for (int j = 0; j < AdjFish.Count; j++)
			{
				var OtherFish = AdjFish[j];
				float dist = Vector2.DistanceSquared(position, OtherFish.position);
				if (dist < range * range && dist > 0)
				{
					sum += OtherFish.velocity;
					count++;
				}
			}

			if (count > 0)
				sum /= count;

			if (sum != Vector2.Zero)
			{
				sum = Vector2.Normalize(sum) * MaxVelocity;
				Vector2 acc = sum - velocity;
				return Limit(acc, MaxForce);
			}
			return Vector2.Zero;
		}

		//Must stay close
		public Vector2 Cohesion(int range)
		{
			int count = 0;
			Vector2 sum = new Vector2(0, 0);
			for (int j = 0; j < AdjFish.Count; j++)
			{
				var OtherFish = AdjFish[j];
				float dist = Vector2.DistanceSquared(position, OtherFish.position);
				if (dist < range * range && dist > 0)
				{
					sum += OtherFish.position;
					count++;
				}
			}

			if (count > 0)
			{
				sum /= count;
				sum -= position;
				sum = Vector2.Normalize(sum) * MaxVelocity;
				Vector2 acc = sum - velocity;
				return Limit(acc, MaxForce);
			}
			return Vector2.Zero;
		}

		public void Draw(SpriteBatch spritebatch)
		{
			Point point = position.ToTileCoordinates();
			Color lightColour = Lighting.GetColor(point.X, point.Y);
			float alpha = MathHelper.Clamp(1 - (SpawnTimer-- / 100f), 0f, 1f);
			Texture2D texture = parent.FlockTextures[TextureID];

			Rectangle source = new Rectangle(0, texture.Height / 2 * (Frame % 2), texture.Width, texture.Height / 2);

			spritebatch.Draw(texture, position.ForDraw(), source,
				lightColour * alpha, velocity.ToRotation() + (float)Math.PI, new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2).Center.ToVector2(),
				parent.FlockScale, SpriteEffects.None, 0f);
		}

		public void ApplyForces()
		{
			velocity += Acceleration;
			velocity = Limit(velocity, MaxVelocity);
			position += velocity;
			Acceleration *= 0;
		}

		public void Update()
		{
			//arbitrarily weight
			Acceleration += Seperation(25) * 1.5f;
			Acceleration += Allignment(50) * 1f;
			Acceleration += Cohesion(50) * 1f;
			Acceleration += AvoidHooman(50) * 4f;
			Acceleration += AvoidTiles(100) * 5f;
			ApplyForces();

			if (Main.rand.Next(7) == 0)
				Frame++;
		}
	}
}
