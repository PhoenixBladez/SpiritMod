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
				if (!pEffect.Condition())
					continue;

				Vector2 spawnPosition = pEffect.spawninfo().SpawnPosition;

				pEffect.particlesystem.AddParticle(new Particle(spawnPosition, 
					pEffect.spawninfo().SpawnVel, 
					pEffect.spawninfo().SpawnRot, 
					pEffect.spawninfo().SpawnScale, 
					pEffect.spawninfo().spawnColor, 
					pEffect.MaxTimeLeft, 
					spawnPosition));

				pEffect.particlesystem.DrawParticles(spriteBatch);
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
			public Color spawnColor;
			public SpawnInfo(Vector2 SpawnPosition, Vector2 SpawnVel, float SpawnRot, float SpawnScale, Color spawnColor)
			{
				this.SpawnPosition = SpawnPosition;
				this.SpawnVel = SpawnVel;
				this.SpawnRot = SpawnRot;
				this.SpawnScale = SpawnScale;
				this.spawnColor = spawnColor;
			}
		}

		public delegate SpawnInfo spawnInfo();


		public Conditional Condition;
		public spawnInfo spawninfo;
		public int MaxTimeLeft;

		public ParticleEffects(ParticleSystem particlesystem, Conditional Condition, spawnInfo spawninfo, int MaxTimeLeft)
		{
			this.particlesystem = particlesystem;
			this.Condition = Condition;
			this.spawninfo = spawninfo;
			this.MaxTimeLeft = MaxTimeLeft;
		}
	}
}