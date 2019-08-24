using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using SpiritMod.Items.Halloween;
using Terraria.Utilities;

namespace SpiritMod
{
	public static class Utililties
	{
		public static float GetDamageBoost(this Player player)
		{
			float min = player.meleeDamage;
			if (player.magicDamage < min)
				min = player.magicDamage;
			if (player.rangedDamage < min)
				min = player.rangedDamage;
			if (player.thrownDamage < min)
				min = player.thrownDamage;
			if (player.minionDamage < min)
				min = player.minionDamage;
			return min;
		}

		public static Vector2 NextVec2CircularEven(this UnifiedRandom rand, float halfWidth, float halfHeight)
		{
			double x = rand.NextDouble();
			double y = rand.NextDouble();
			if (x + y > 1)
			{
				x = 1 - x;
				y = 1 - y;
			}
			double s = 1 / (x + y);
			if (double.IsNaN(s))
				return Vector2.Zero;
			s *= s;
			s = Math.Sqrt(x * x * s + y * y * s);
			s = 1 / s;
			x *= s;
			y *= s;
			double r = rand.NextDouble() * (2 * Math.PI);
			double cos = Math.Cos(r);
			double sin = Math.Sin(r);
			return new Vector2((float)(x * cos - y * sin) * halfWidth, (float)(x * sin + y * cos) * halfHeight);
		}


		public static bool LeftOf(this Vector2 point, Vector2 check)
		{
			return check.X * point.Y - check.Y * point.X < 0;
		}

		public static float SideOfNormalize(this Vector2 point, Vector2 check)
		{
			float length = check.Length();
			length = (check.X * point.Y - check.Y * point.X) / length;
			if (float.IsNaN(length))
				return 0f;
			return length;
		}

		public static float SideOf(this Vector2 point, Vector2 checkNorm)
		{
			return checkNorm.X * point.Y - checkNorm.Y * point.X;
		}


		public static Vector2 TurnRight(this Vector2 vec)
		{
			return new Vector2(-vec.Y, vec.X);
		}

		public static Vector2 TurnLeft(this Vector2 vec)
		{
			return new Vector2(vec.Y, -vec.X);
		}

		public static bool Nearing(this Vector2 vec, Vector2 target)
		{
			return 0 < vec.X * target.X + vec.Y * target.Y;
		}
	}
}
