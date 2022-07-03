using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Particles;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Pathfinder.PathfinderGores
{
	public class PathfinderGore : Particle
	{
		private int maxTime;
		private float opacity = 1;
		private float glowOpacity = 1;
		private int chosenTexture = Main.rand.Next(1, 4);

		private List<Vector2> oldPositions = new List<Vector2>();
		private const int OLDPOSMAXLENGTH = 6;

		private const float FADETIME = 120;
		private const float GLOWFADETIME = 60;

		public override bool UseCustomDraw => true;

		public PathfinderGore(Vector2 position, Vector2 velocity, float scale, int maxTime)
		{
			Position = position;
			Velocity = velocity;
			Scale = scale;
			this.maxTime = maxTime;
		}

		public override void Update()
		{
			glowOpacity = Math.Max(glowOpacity - 2 / GLOWFADETIME, 0);

			Rotation += Velocity.X * 0.1f;
			Velocity.Y += 0.3f;

			//X Collision
			if (!Collision.CanHit(Position, 0, 0, Position + Vector2.UnitX * Velocity.X, 0, 0))
			{
				Position.X -= Velocity.X;
				Velocity.X *= (Math.Abs(Velocity.X) > 1) ? -0.25f : 0;
			}

			//Y Collision
			if (!Collision.CanHit(Position, 0, 0, Position + Vector2.UnitY * Velocity.Y, 0, 0))
			{
				Position.Y -= Velocity.Y;
				Velocity.Y *= (Math.Abs(Velocity.Y) > 1) ? -0.25f : 0;
			}

			if (glowOpacity > 0 && Main.rand.NextBool(8))
				ParticleHandler.SpawnParticle(new FireParticle(Position, Velocity * Main.rand.NextFloat(),
					Color.White * glowOpacity, Color.HotPink * glowOpacity, Main.rand.NextFloat(0.4f, 0.5f), 20, delegate (Particle p)
					{
						p.Velocity *= 0.95f;
					}));

			Lighting.AddLight(Position, Color.ToVector3() * Scale * 0.5f * glowOpacity);

			oldPositions.Add(Position);
			if (oldPositions.Count > OLDPOSMAXLENGTH)
				oldPositions.RemoveAt(0);

			if (TimeActive > maxTime - FADETIME)
				opacity = Math.Max(opacity - 1 / FADETIME, 0);

			if (TimeActive > maxTime)
				Kill();
		}

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			string texturePath = "SpiritMod/NPCs/StarjinxEvent/Enemies/Pathfinder/PathfinderGores/PathfinderGore";
			if(chosenTexture == 2 || chosenTexture == 3)
				texturePath += chosenTexture;

			Texture2D Tex = ModContent.Request<Texture2D>(texturePath, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Texture2D Glow = ModContent.Request<Texture2D>(texturePath + "_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			Color glowColor = Color.HotPink * glowOpacity;

			if(glowOpacity > 0)
				for(int i = 0; i < oldPositions.Count; i++)
				{
					float Opacity = 0.5f * i / (float)oldPositions.Count;
					Vector2 drawPosition = oldPositions[i] - Main.screenPosition;

					spriteBatch.Draw(Glow, drawPosition, null, glowColor * Opacity, Rotation, Glow.Size() / 2, Scale * 1.2f, SpriteEffects.None, 0);
					spriteBatch.Draw(Tex, drawPosition, null, glowColor * Opacity, Rotation, Tex.Size() / 2, Scale, SpriteEffects.None, 0);
				}
			Color lightColor = Lighting.GetColor((int)Position.X / 16, (int)Position.Y / 16) * opacity;
			spriteBatch.Draw(Glow, Position - Main.screenPosition, null, glowColor, Rotation, Glow.Size() / 2, Scale * 1.2f, SpriteEffects.None, 0);
			spriteBatch.Draw(Tex, Position - Main.screenPosition, null, lightColor, Rotation, Tex.Size() / 2, Scale, SpriteEffects.None, 0);
		}
	}
}
