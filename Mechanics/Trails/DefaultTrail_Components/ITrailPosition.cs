using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent;

namespace SpiritMod.Mechanics.Trails
{
	public interface ITrailPosition
	{
		Vector2 GetNextTrailPosition(Projectile projectile);
	}

	public class DefaultTrailPosition : ITrailPosition
	{
		public Vector2 GetNextTrailPosition(Projectile projectile) => projectile.Center;
	}

	public class SleepingStarTrailPosition : ITrailPosition
	{
		public Vector2 GetNextTrailPosition(Projectile projectile)
		{
			var drawOrigin = new Vector2(TextureAssets.Projectile[projectile.type].Value.Width * 0.5f, projectile.height * 0.5f);
			return projectile.position + drawOrigin + Vector2.UnitY * projectile.gfxOffY;
		}
	}

	public class ArrowGlowPosition : ITrailPosition
	{
		public Vector2 GetNextTrailPosition(Projectile projectile) => projectile.Center + projectile.velocity + Vector2.UnitY * projectile.gfxOffY;
	}

	public class ZigZagTrailPosition : ITrailPosition
	{
		private int _zigType;
		private int _zigMove;
		private float _strength;

		public ZigZagTrailPosition(float strength)
		{
			_strength = strength;
			_zigType = 0;
			_zigMove = 1;
		}

		public Vector2 GetNextTrailPosition(Projectile projectile)
		{
			Vector2 offset = Vector2.Zero;
			if (_zigType == -1) offset = projectile.velocity.TurnLeft();
			else if (_zigType == 1) offset = projectile.velocity.TurnRight();
			if (_zigType != 0) offset.Normalize();

			_zigType += _zigMove;
			if (_zigType == 2)
			{
				_zigType = 0;
				_zigMove = -1;
			}
			else if (_zigType == -2)
			{
				_zigType = 0;
				_zigMove = 1;
			}

			return projectile.Center + offset * _strength;
		}
	}

	public class WaveTrailPos : ITrailPosition
	{
		private float _counter;
		private readonly float _strength;
		public WaveTrailPos(float strength)
		{
			_strength = strength;
		}

		public Vector2 GetNextTrailPosition(Projectile proj)
		{
			_counter += 0.33f;
			Vector2 offset = Vector2.UnitX.RotatedBy((float)Math.Sin(MathHelper.PiOver4 * _counter));
			return proj.Center + offset.RotatedBy(proj.velocity.ToRotation()) * _strength;
		}
	}
}