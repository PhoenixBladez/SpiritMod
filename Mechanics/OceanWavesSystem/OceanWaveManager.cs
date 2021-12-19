using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Effects.SurfaceWaterModifications;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics;

namespace SpiritMod.Mechanics.OceanWavesSystem
{
	public class OceanWaveManager
	{
		public class Wave
		{
			public readonly Vector2 Size;

			public float speed;
			public float strength;
			public Vector2 position;

			public Wave(Vector2 p, Vector2 s, float st, float sp)
			{
				Size = s;

				strength = st;
				position = p;
				speed = sp;
			}

			public bool Update()
			{
				const float BaseSpeed = 5f;

				if (position.X / 16f < Main.maxTilesX / 2)
					position.X += BaseSpeed * speed;
				else
					position.X -= BaseSpeed * speed;

				if (strength > 0.05f)
					strength *= 0.998f;
				else
					return true;

				Point tPos = (position + Size).ToTileCoordinates();
				if (WorldGen.SolidTile(tPos.X, tPos.Y))
					return true;
				return false;
			}
		}

		private static List<Wave> waves = new List<Wave>();

		public static void AddWave(Wave wave) => waves.Add(wave);

		public static void UpdateWaves(bool left, bool right, Vector2 offset)
		{
			List<Wave> removals = new List<Wave>();

			for (int i = 0; i < waves.Count; ++i)
			{
				Wave wave = waves[i];

				if (wave.Update())
					removals.Add(wave);

				if ((left && wave.position.X / 16f < Main.maxTilesX / 2f) || (right && wave.position.X / 16f > Main.maxTilesX / 2f))
					DrawWave(wave, offset);
			}

			foreach (var item in removals)
				waves.Remove(item);
		}

		private static void DrawWave(Wave wave, Vector2 offset)
		{
			Color col = GetRippleColor(wave);
			Vector2 drawPos = wave.position - offset;
			Rectangle src = new Rectangle(1, 1, 62, 62);
			Main.tileBatch.Draw(SurfaceWaterModifications.rippleTex, new Vector4(drawPos.X, drawPos.Y, wave.Size.X, wave.Size.Y) * 0.25f, src, new VertexColors(col), src.Size() / 2f, SpriteEffects.None, 0f);
		}

		public static Color GetRippleColor(Wave wave)
		{
			float g = wave.strength * 0.5f + 0.5f;
			float mult = Math.Min(Math.Abs(wave.strength), 1f);
			return new Color(0.5f, g, 0f, 1f) * mult;
		}
	}
}
