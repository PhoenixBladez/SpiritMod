using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;

namespace SpiritMod
{
	public static class ExtraUtils
	{
		public static void Bounce(this Projectile projectile, Vector2 oldVelocity, float VelocityKeptRatio = 1f)
        {
			projectile.velocity = new Vector2((projectile.velocity.X == oldVelocity.X) ? projectile.velocity.X : -oldVelocity.X * VelocityKeptRatio, (projectile.velocity.Y == oldVelocity.Y) ? projectile.velocity.Y : -oldVelocity.Y * VelocityKeptRatio/ 2);
		}

		public static Vector2 GetArcVel(Vector2 startingPos, Vector2 targetPos, float gravity, float? minArcHeight = null, float? maxArcHeight = null, float? maxXvel = null, float? heightabovetarget = null)
		{
			Vector2 DistanceToTravel = targetPos - startingPos;
			float MaxHeight = DistanceToTravel.Y - (heightabovetarget ?? 0);
			if(minArcHeight != null)
				MaxHeight = Math.Min((sbyte)MaxHeight, -(sbyte)minArcHeight);

			if (maxArcHeight != null)
				MaxHeight = Math.Max((sbyte)MaxHeight, -(sbyte)maxArcHeight);

			float TravelTime;
			float neededYvel;
			if (MaxHeight <= 0)
            {
				neededYvel = -(float)Math.Sqrt(-2 * gravity * MaxHeight);
				TravelTime = (float)Math.Sqrt(-2 * MaxHeight / gravity) + (float)Math.Sqrt(2 * Math.Max(DistanceToTravel.Y - MaxHeight, 0) / gravity); //time up, then time down
			}

			else
            {
				neededYvel = 0;
                TravelTime = (-neededYvel + (float)Math.Sqrt(Math.Pow(neededYvel, 2) - (4 * -DistanceToTravel.Y * gravity/2)))/ (gravity); //time down
            }

			if (maxXvel != null)
				return new Vector2(MathHelper.Clamp(DistanceToTravel.X / TravelTime, -(float)maxXvel, (float)maxXvel), neededYvel);

			return new Vector2(DistanceToTravel.X / TravelTime, neededYvel);
		}

        public static Vector2 GetArcVel(this Entity ent, Vector2 targetPos, float gravity, float? minArcHeight = null, float? maxArcHeight = null, float? maxXvel = null, float? heightabovetarget = null) => GetArcVel(ent.Center, targetPos, gravity, minArcHeight, maxArcHeight, maxXvel, heightabovetarget);
    }
}
