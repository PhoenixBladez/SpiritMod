using Microsoft.Xna.Framework;
using SpritMod.Utilities;
using System;
using Terraria;

namespace SpiritMod.ParticleHandler
{
    public class SpiritParticles : ParticleEffects
	{
		readonly static int maxtime = 960;
		public SpiritParticles() : base(new ParticleSystem("SpiritMod/ParticleHandler/SpiritParticle", OnUpdate),
			delegate { return SpawnCondition(); },
			delegate { return SInfo(); },
			maxtime, 0.33f) { }
		static void OnUpdate(Particle particle)
		{
			particle.Position = particle.StoredPosition - Main.screenPosition;
			particle.StoredPosition += particle.Velocity;
			particle.Color = Color.White * (float)Math.Sin(MathHelper.TwoPi * (particle.Timer / (float)maxtime));
			particle.Timer--;
		}

		private static bool SpawnCondition() => Main.LocalPlayer.GetModPlayer<MyPlayer>().ZoneSpirit;

		private static SpawnInfo SInfo()
		{
			Vector2 position = new Vector2(Main.rand.Next(-2500, 2500), Main.rand.Next(1000, 1200));
			return new SpawnInfo(Main.LocalPlayer.Center + position, Main.rand.NextFloat(-2, -1) * Vector2.UnitY, Main.rand.NextFloat(MathHelper.TwoPi), Main.rand.NextFloat(0.2f, 0.4f), Color.White);
		}
	}
}