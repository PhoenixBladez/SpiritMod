using Microsoft.Xna.Framework;
using SpritMod.Utilities;
using System;
using Terraria;

namespace SpiritMod.ParticleHandler
{
    public class SpiritParticles : ParticleEffects
	{
		readonly static int maxtime = 1620;
		public SpiritParticles() : base(new ParticleSystem("SpiritMod/ParticleHandler/SpiritParticle", OnUpdate),
			delegate { return SpawnCondition(); },
			delegate { return SInfo(); },
			maxtime, 0.125f) { }
		static void OnUpdate(Particle particle)
		{
			particle.DrawPosition = Vector2.Lerp(particle.StoredPosition[0] - Main.screenPosition, particle.StoredPosition[1], 0.75f * (1f - particle.Scale));

			for (int i = 0; i < particle.StoredPosition.Length; i++)
				particle.StoredPosition[i] += particle.Velocity;

			particle.Color = Color.White * (float)Math.Sin(MathHelper.TwoPi * (particle.Timer / (float)maxtime)) * 0.33f;
			particle.Timer--;
		}

		private static bool SpawnCondition() => Main.LocalPlayer.GetModPlayer<MyPlayer>().ZoneSpirit;

		private static SpawnInfo SInfo()
		{
			Vector2 position = new Vector2(Main.rand.Next(-2000, 2000), Main.rand.Next(1400, 1800));
			return new SpawnInfo(position, Main.rand.NextFloat(-1.2f, -0.8f) * Vector2.UnitY, Main.rand.NextFloat(MathHelper.TwoPi), Main.rand.NextFloat(0.4f, 0.6f),
				Color.White, new Vector2[] { Main.LocalPlayer.Center + position, position });
		}
	}
}