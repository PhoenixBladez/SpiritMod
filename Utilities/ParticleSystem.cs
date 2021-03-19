//---------------------------------------------------------------------------
// Ported from Starlight River with permission from developers. 
// Removal of this comment block constitutes surrender of said permission.
//---------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace SpritMod.Utilities
{
	public delegate void Update(Particle particle);

	public class ParticleSystem
	{
		private readonly List<Particle> Particles = new List<Particle>();
		private readonly Texture2D Texture;
		private readonly Update UpdateDelegate;
		private readonly int Styles;

		public static bool ShowParticles => true; //change to whatever you need this. I'd reccomend making this a config option.

		public ParticleSystem(string texture, Update updateDelegate, int styles = 1)
		{
			Texture = GetTexture(texture);
			UpdateDelegate = updateDelegate;
			Styles = styles;
		}

		public static bool OnScreen(Vector2 pos) => pos.X > -16 && pos.X < Main.screenWidth + 16 && pos.Y > -16 && pos.Y < Main.screenHeight + 16;

		public void DrawParticles(SpriteBatch spriteBatch)
		{
			if (ShowParticles)
				for (int k = 0; k < Particles.Count; k++) {
					Particle particle = Particles[k];

					if (!Main.gameInactive) UpdateDelegate(particle);
					if (OnScreen(particle.Position)) {
						int height = Texture.Height / Styles;
						spriteBatch.Draw(Texture, particle.Position, new Rectangle(0, particle.GetHashCode() % Styles * height, Texture.Width, height), particle.Color, particle.Rotation, Texture.Size() / 2, particle.Scale, 0, 0);
					}
					if (particle.Timer <= 0) Particles.Remove(particle);
				}
		}

		public void AddParticle(Particle particle)
		{
			if (ShowParticles && !Main.gameInactive)
				Particles.Add(particle);
		}

	}

	public class Particle
	{
		internal Vector2 Position;
		internal Vector2 Velocity;
		internal Vector2 StoredPosition;
		internal float Rotation;
		internal float Scale;
		internal Color Color;
		internal int Timer;

		public Particle(Vector2 position, Vector2 velocity, float rotation, float scale, Color color, int timer, Vector2 storedPosition)
		{
			Position = position; Velocity = velocity; Rotation = rotation; Scale = scale; Color = color; Timer = timer; StoredPosition = storedPosition;
		}
	}
}

/*---------------------------------------------------------------------------------------------------------------
 * Usage instructions:
 * 1. Create an instance of this class in an appropriate place, passing the path to the texture
 * as a string, and an appropriate body for the particle update delegate. see line 16 for definition
 * of this delegate type.
 * 
 * 2. call the DrawParticles method in the desired location (obeying the normal rules of terraria's
 * immediate drawing mode.) you can call AddParticle in this same place with your desired spawning logic.
 * 
 * 3. Ensure you dispose the instance if it is static.
 * ---------------------------------------------------------------------------------------------------------------
 * Tips for creating effects:
 * particle position is it's screen position. If you want world-relative particles, use storedPosition to 
 * store the world position and set position to storedPosition - screenPosition in update.
 * 
 * utilize the styles variable for multiple particles with the same behavior but different textures
 * 
 * create another particleSystem instance if you need  a second type of particle with different behavior
 * 
 * you must decrement time yourself. particles are removed at <= 0 timer. you can set a particle's timer to 0
 * to kill it gracefully.
 * 
 * dont forget to kill particles. dont.
 * ---------------------------------------------------------------------------------------------------------------
 */