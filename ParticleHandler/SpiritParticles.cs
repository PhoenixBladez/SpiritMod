using Microsoft.Xna.Framework;
using SpritMod.Utilities;
using System;
using Terraria;

namespace SpiritMod.ParticleHandler
{
    public class SpiritParticles : ParticleEffects
	{
		public SpiritParticles() : base(new ParticleSystem("SpiritMod/ParticleHandler/HyperspaceParticle", OnUpdate),
			delegate { return Main.LocalPlayer.GetModPlayer<MyPlayer>().ZoneSpirit; },
			delegate { return SInfo(); },
			480)
		{ }
		static void OnUpdate(Particle particle)
		{
			particle.Position = particle.StoredPosition - Main.screenPosition;
			particle.StoredPosition += particle.Velocity;
			particle.Color = Color.Lerp(Color.White, Color.Transparent, (float)Math.Sin(MathHelper.TwoPi * (particle.Timer / 480)) / 2 + 0.5f);
			particle.Timer--;
		}
		private static SpawnInfo SInfo() => new SpawnInfo(Main.LocalPlayer.Center + new Vector2(Main.rand.Next(-1500, 1500), Main.rand.Next(1000, 1200)), Main.rand.NextFloat(-4, -2) * Vector2.UnitY, Main.rand.NextFloat(MathHelper.TwoPi), Main.rand.NextFloat(0.4f, 0.6f), Color.White);
	}
}