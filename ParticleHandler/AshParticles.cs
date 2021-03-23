using Microsoft.Xna.Framework;
using SpritMod.Utilities;
using System;
using Terraria;

namespace SpiritMod.ParticleHandler
{
    public class AshParticles : ParticleEffects
	{
		readonly static int maxtime = 1100;
		public AshParticles() : base(new ParticleSystem("SpiritMod/ParticleHandler/AshParticle", OnUpdate),
			delegate { return SpawnCondition(); },
			delegate { return SInfo(); },
			maxtime, 0.65f) { }
		static void OnUpdate(Particle particle)
		{
			Vector2 WorldPosition = particle.StoredPosition[0] - Main.screenPosition;
			Vector2 ScreenPosition = particle.StoredPosition[1];
			particle.DrawPosition = Vector2.Lerp(ScreenPosition, ScreenPosition - 3 * (ScreenPosition - WorldPosition), particle.Scale);

			ParticleHandler.WrapEdges(ref particle.DrawPosition);

			for (int i = 0; i < particle.StoredPosition.Length; i++)
				particle.StoredPosition[i] += particle.Velocity;//new Vector2(particle.Velocity.X  * (particle.Timer / (float)maxtime), particle.Velocity.Y + (particle.Timer / (float)maxtime));

			particle.Color = Color.Black * (float)Math.Sin(MathHelper.TwoPi * (particle.Timer / (float)maxtime)) * 0.85f;
			particle.Timer--;

		}

		private static bool SpawnCondition() => Main.LocalPlayer.ZoneUnderworldHeight && MyWorld.ashRain;

		private static SpawnInfo SInfo()
		{
			Vector2 position = new Vector2(Main.rand.Next(-2000, 2000), Main.rand.Next(1200, 1800));
			return new SpawnInfo(position, new Vector2(Main.windSpeed * 12f, Main.rand.NextFloat(1, 3f)), Main.rand.NextFloat(MathHelper.TwoPi), Main.rand.NextFloat(0.3f, 1.05f),
				Color.White, new Vector2[] { Main.LocalPlayer.Center + position, position });
		}
	}
}