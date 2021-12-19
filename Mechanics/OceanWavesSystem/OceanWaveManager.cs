using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpiritMod.Mechanics.OceanWavesSystem
{
	public class OceanWaveManager
	{
		public struct Wave
		{
			public readonly Vector2 Position;
			public readonly Vector2 Size;
			public readonly float Strength;

			public Wave(Vector2 p, Vector2 s, float st)
			{
				Position = p;
				Size = s;
				Strength = st;
			}
		}

		public static List<Wave> waves = new List<Wave>();
	}
}
