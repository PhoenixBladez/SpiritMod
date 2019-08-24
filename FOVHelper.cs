using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod
{
	class FOVHelper
	{
		public const float POS_X_DIR = 0f;
		public const float NEG_X_DIR = (float)Math.PI;
		public const float POS_Y_DIR = (float)Math.PI / 2f;
		public const float NEG_Y_DIR = (float)Math.PI / -2f;
		public const float UP = NEG_Y_DIR;
		public const float DOWN = POS_Y_DIR;
		public const float LEFT = NEG_X_DIR;
		public const float RIGHT = POS_X_DIR;

		private bool returnDefault = true;
		private bool defaultValue = false;
		private bool overExtended = false;
		Vector2 origin = Vector2.Zero;

		private bool checkAboveLeft = false;
		private bool verticalLeft = false;
		private float slopeLeft = 0f;

		private bool checkAboveRight = false;
		private bool verticalRight = false;
		private float slopeRight = 0f;

		public void adjustCone(Vector2 center, float fov, float direction)
		{
			if (fov >= Math.PI)
			{
				returnDefault = true;
				defaultValue = true;
				return;
			}
			else if (fov <= 0d)
			{
				returnDefault = true;
				defaultValue = false;
				return;
			}
			else
			{
				returnDefault = false;
				if (fov > MathHelper.PiOver2)
				{
					overExtended = true;
				}
				else
				{
					overExtended = false;
				}
			}
			origin.X = center.X;
			origin.Y = center.Y;
			float left = direction + fov * .5f;
			float right = direction - fov * .5f;
			left = MathHelper.WrapAngle(left);
			right = MathHelper.WrapAngle(right);
			slopeLeft = (float)Math.Tan(left);
			slopeRight = (float)Math.Tan(right);
			verticalLeft = float.IsNaN(slopeLeft);
			verticalRight = float.IsNaN(slopeRight);

			if (verticalLeft)
			{
				if (left > 0f)
				{
					checkAboveLeft = true;
				}
				else
				{
					checkAboveLeft = false;
				}
			}
			else
			{
				if (Math.Abs(left) > MathHelper.PiOver2)
				{
					checkAboveLeft = true;
				}
				else
				{
					checkAboveLeft = false;
				}
			}

			if (verticalRight)
			{
				if (right > 0f)
				{
					checkAboveRight = false;
				}
				else
				{
					checkAboveRight = true;
				}
			}
			else
			{
				if (Math.Abs(right) > MathHelper.PiOver2)
				{
					checkAboveRight = false;
				}
				else
				{
					checkAboveRight = true;
				}
			}
		}

		public bool isInCone(Vector2 pos)
		{
			if (returnDefault)
			{
				return defaultValue;
			}
			float x = pos.X - origin.X;
			float y = pos.Y - origin.Y;

			if (verticalLeft)
			{
				if (checkAboveLeft)
				{
					if (x >= 0f)
					{
						if (overExtended)
							return true;
					}
					else
					{
						if (!overExtended)
							return false;
					}
				}
				else
				{
					if (x <= 0f)
					{
						if (overExtended)
							return true;
					}
					else
					{
						if (!overExtended)
							return false;
					}
				}
			}
			else
			{
				if (checkAboveLeft)
				{
					if (x * slopeLeft <= y)
					{
						if (overExtended)
							return true;
					}
					else
					{
						if (!overExtended)
							return false;
					}
				}
				else
				{
					if (x * slopeLeft >= y)
					{
						if (overExtended)
							return true;
					}
					else
					{
						if (!overExtended)
							return false;
					}
				}
			}

			if (verticalRight)
			{
				if (checkAboveRight)
				{
					return (x >= 0f);
				}
				else
				{
					return (x <= 0f);
				}
			}
			else
			{
				if (checkAboveRight)
				{
					return (x * slopeRight <= y);
				}
				else
				{
					return (x * slopeRight >= y);
				}
			}
		}
	}
}
