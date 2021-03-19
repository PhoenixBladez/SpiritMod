using Microsoft.Xna.Framework;
using SpritMod.Utilities;
using System;
using System.Threading;
using Terraria;

namespace SpiritMod.ParticleHandler
{
    public class HyperspaceParticles : ParticleEffects
	{
		readonly static int maxtime = 960;
		public HyperspaceParticles() : base(new ParticleSystem("SpiritMod/ParticleHandler/HyperspaceParticle", OnUpdate, 2), 
			delegate { return SpawnCondition(); }, 
			delegate { return SInfo(); },
			maxtime, 0.5f) { }

		static void OnUpdate(Particle particle)
		{
			particle.Position = particle.StoredPosition - Main.screenPosition;
			particle.StoredPosition += particle.Velocity.RotatedBy(MathHelper.Pi / 6 * (float)Math.Sin(Math.PI * (particle.Timer / 60f) * (1f - particle.Scale)));
			particle.Color = Color.White * (float)Math.Sin(MathHelper.TwoPi * (particle.Timer / (float)maxtime));
			particle.Timer--;
		}

		private static bool SpawnCondition() => Main.LocalPlayer.GetModPlayer<MyPlayer>().ZoneSynthwave;

		private static SpawnInfo SInfo()
		{
			Vector2 position = new Vector2(Main.rand.Next(-2500, 2500), Main.rand.Next(1000, 1200));
			return new SpawnInfo(Main.LocalPlayer.Center + position, Main.rand.NextFloat(-2, -1) * Vector2.UnitY, Main.rand.NextFloat(MathHelper.TwoPi), Main.rand.NextFloat(0.4f, 0.6f), Color.White);
		}
	}
}