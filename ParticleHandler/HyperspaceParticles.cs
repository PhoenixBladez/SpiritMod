using Microsoft.Xna.Framework;
using SpritMod.Utilities;
using System;
using System.Threading;
using Terraria;

namespace SpiritMod.ParticleHandler
{
    public class HyperspaceParticles : ParticleEffects
	{
		readonly static int maxtime = 1200;
		public HyperspaceParticles() : base(new ParticleSystem("SpiritMod/ParticleHandler/HyperspaceParticle", OnUpdate, 2), 
			delegate { return SpawnCondition(); }, 
			delegate { return SInfo(); },
			maxtime, 0.25f) { }

		static void OnUpdate(Particle particle)
		{
			Vector2 WorldPosition = particle.StoredPosition[0] - Main.screenPosition;
			Vector2 ScreenPosition = particle.StoredPosition[1];
			particle.DrawPosition = Vector2.Lerp(ScreenPosition, ScreenPosition - 3 * (ScreenPosition - WorldPosition), particle.Scale);

			ParticleHandler.WrapEdges(ref particle.DrawPosition);

			for (int i = 0; i < particle.StoredPosition.Length; i++)
				particle.StoredPosition[i] += particle.Velocity.RotatedBy(MathHelper.Pi / 6 * (float)Math.Sin(Math.PI * (particle.Timer / 60f) * (1f - particle.Scale)));


			particle.Color = Color.White * (float)Math.Sin(MathHelper.TwoPi * (particle.Timer / (float)maxtime));
			particle.Timer--;
		}

		private static bool SpawnCondition() => Main.LocalPlayer.GetModPlayer<MyPlayer>().ZoneSynthwave;

		private static SpawnInfo SInfo()
		{
			Vector2 position = new Vector2(Main.rand.Next(-2000, 2000), Main.rand.Next(1200, 1600));
			return new SpawnInfo(Vector2.Zero, Main.rand.NextFloat(-2, -1) * Vector2.UnitY, Main.rand.NextFloat(MathHelper.TwoPi), Main.rand.NextFloat(0.4f, 0.6f), 
				Color.White, new Vector2[] { Main.LocalPlayer.Center + position, position });
		}
	}
}