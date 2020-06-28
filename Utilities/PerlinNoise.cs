using Microsoft.Xna.Framework;
using System;

namespace SpiritMod.Utilities
{
	public class PerlinNoise
	{
		private int[] _permutation;
		private Random _random;

		public int Seed { get; }

		public PerlinNoise(int seed)
		{
			_random = new Random(seed);
			Seed = seed;

			//shuffle to create permutation
			int[] p = new int[256];
			for(int i = 0; i < 256; i++) p[i] = i;
			_random.Shuffle(ref p);

			//append itself to double array and remove any overflow errors
			_permutation = new int[512];
			for(int i = 0; i < 512; i++) _permutation[i] = p[i % 256];
		}

		/// <returns>A value between -1 and 1</returns>
		public float Noise(float x, float y)
		{
			//get the indexes in the permutation based on the x and y values (& 255 ensures they are between 0 and 255)
			int permX = (int)(Math.Floor(x)) & 255;
			int permY = (int)(Math.Floor(y)) & 255;

			x -= (float)Math.Floor(x);
			y -= (float)Math.Floor(y);

			float u = Fade(x);
			float v = Fade(y);

			int permA = (_permutation[permX] + permY) & 255;
			int permB = (_permutation[permX + 1] + permY) & 255;

			//returns value from -1 to 1
			return MathHelper.Lerp(
				MathHelper.Lerp(Grad(_permutation[permA], x, y), Grad(_permutation[permB], x - 1, y), u),
				MathHelper.Lerp(Grad(_permutation[permA + 1], x, y - 1), Grad(_permutation[permB + 1], x - 1, y - 1), u),
				v);
		}

		public float FractionalBrownianMotion(float x, float y, int octaves)
		{
			//initiate values
			float value = 0f, mult = 0.5f;

			for(int i = 0; i < octaves; i++) {
				value += Noise(x, y) * mult;
				//multiply the values to move them and get different data
				x *= 2f;
				y *= 2f;
				//each extra octave only effects the final value by a half.
				mult *= 0.5f;
			}

			return value;
		}

		private static float Fade(float t)
		{
			//fade function pushes values closer to integers
			return t * t * t * (t * (t * 6 - 15) + 10);
		}

		private static float Grad(int hash, float x, float y)
		{
			return ((hash & 1) == 0 ? x : -x) + ((hash & 2) == 0 ? y : -y);
		}
	}
}
