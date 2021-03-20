using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using System.Linq;
using System;
using SpritMod.Utilities;

namespace SpiritMod.ParticleHandler
{

	public static class ParticleHandler
	{
		private static Mod _mod;
		private static List<ParticleEffects> _particleEffects;

		public static void Load(Mod mod)
		{
			_mod = mod;

			_particleEffects = new List<ParticleEffects>();
			var pEffects = mod.Code.GetTypes().Where(x => !x.IsAbstract && x.IsSubclassOf(typeof(ParticleEffects)) && x.IsClass); //use reflection to get types in the mod, then find particleeffects types that arent the abstract class
			foreach(Type t in pEffects) 
				_particleEffects.Add(Activator.CreateInstance(t) as ParticleEffects);//use activator to convert the types into objects, as a direct conversion to particleeffects does not work
		}

		public static void DrawParticles(SpriteBatch spriteBatch)
		{
			foreach(ParticleEffects pEffect in _particleEffects) {

				pEffect.particlesystem.DrawParticles(spriteBatch, pEffect.opacity);

				if (!pEffect.Condition()) {
					pEffect.opacity = MathHelper.Lerp(pEffect.opacity, 0, 0.05f);
					continue;
				}

				pEffect.opacity = MathHelper.Lerp(pEffect.opacity, 1, 0.05f);

				if (Main.rand.NextFloat() > pEffect.density)
					continue;

				pEffect.particlesystem.AddParticle(new Particle(pEffect.spawninfo().SpawnPosition, 
					pEffect.spawninfo().SpawnVel, 
					pEffect.spawninfo().SpawnRot, 
					pEffect.spawninfo().SpawnScale, 
					pEffect.spawninfo().SpawnColor, 
					pEffect.MaxTimeLeft, 
					pEffect.spawninfo().StoredPosition));
			}
		}

		public static void Unload()
		{
			_mod = null;
			_particleEffects = null;
		}
	}
	public abstract class ParticleEffects
	{
		public ParticleSystem particlesystem;
		public delegate bool Conditional();

		public struct SpawnInfo
		{
			public Vector2 SpawnPosition;
			public Vector2 SpawnVel;
			public float SpawnRot;
			public float SpawnScale;
			public Color SpawnColor;
			public Vector2[] StoredPosition;
			public SpawnInfo(Vector2 SpawnPosition, Vector2 SpawnVel, float SpawnRot, float SpawnScale, Color SpawnColor, Vector2[] StoredPosition = null)
			{
				this.SpawnPosition = SpawnPosition;
				this.SpawnVel = SpawnVel;
				this.SpawnRot = SpawnRot;
				this.SpawnScale = SpawnScale;
				this.SpawnColor = SpawnColor;
				this.StoredPosition = StoredPosition ?? new Vector2[] { SpawnPosition }; //default to storing the spawn position if stored position is null
			}
		}

		public delegate SpawnInfo spawnInfo();


		public Conditional Condition;
		public spawnInfo spawninfo;
		public int MaxTimeLeft;
		public float density;
		public float opacity = 0;

		public ParticleEffects(ParticleSystem particlesystem, Conditional Condition, spawnInfo spawninfo, int MaxTimeLeft, float density = 1f)
		{
			this.particlesystem = particlesystem;
			this.Condition = Condition;
			this.spawninfo = spawninfo;
			this.MaxTimeLeft = MaxTimeLeft;
			this.density = density;
		}
	}
}