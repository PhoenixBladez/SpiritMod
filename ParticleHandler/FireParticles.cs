using Microsoft.Xna.Framework;
using SpritMod.Utilities;
using System;
using Terraria;

namespace SpiritMod.ParticleHandler
{
    public class FireParticles : ParticleEffects
	{
		readonly static int maxtime = 540;
		public FireParticles() : base(new ParticleSystem("SpiritMod/ParticleHandler/FireParticle", OnUpdate),
			delegate { return SpawnCondition(); },
			delegate { return SInfo(); },
			maxtime, 0.05f) { }
		static void OnUpdate(Particle particle)
		{
			Vector2 WorldPosition = particle.StoredPosition[0] - Main.screenPosition;
			Vector2 ScreenPosition = particle.StoredPosition[1];
			particle.DrawPosition = Vector2.Lerp(ScreenPosition, ScreenPosition - 3 * (ScreenPosition - WorldPosition), particle.Scale);

			ParticleHandler.WrapEdges(ref particle.DrawPosition);

			for (int i = 0; i < particle.StoredPosition.Length; i++)
				particle.StoredPosition[i] += particle.Velocity * (particle.Timer / (float)maxtime);

			particle.Color = Color.White * (float)Math.Sin(MathHelper.TwoPi * (particle.Timer / (float)maxtime)) * 0.85f;
			particle.Timer--;

		}

		private static bool SpawnCondition() => (Main.LocalPlayer.ZoneMeteor || Main.LocalPlayer.ZoneUnderworldHeight);

		private static SpawnInfo SInfo()
		{
			Vector2 position = new Vector2(Main.rand.Next(-2000, 2000), Main.rand.Next(1800, 2200));
			return new SpawnInfo(position, new Vector2(Main.windSpeed * 2f, Main.rand.NextFloat(-2, -6f)), Main.rand.NextFloat(MathHelper.PiOver4), Main.rand.NextFloat(0.3f, 0.85f),
				Color.White, new Vector2[] { Main.LocalPlayer.Center + position, position });
		}
	}
}