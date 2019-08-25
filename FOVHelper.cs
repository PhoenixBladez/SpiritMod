using Microsoft.Xna.Framework;
using System;

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

        public void AdjustCone(Vector2 center, float fov, float direction)
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
                overExtended = fov > MathHelper.PiOver2 ? true : false;
            }

            origin = center;

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
                checkAboveLeft = left > 0f ? true : false;
            }
            else
            {
                checkAboveLeft = Math.Abs(left) > MathHelper.PiOver2 ? true : false;
            }

            if (verticalRight)
            {
                checkAboveRight = right > 0f ? false : true;
            }
            else
            {
                checkAboveRight = Math.Abs(right) > MathHelper.PiOver2 ? false : true;
            }
        }

        public bool IsInCone(Vector2 pos)
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
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (!overExtended)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (x <= 0f)
                    {
                        if (overExtended)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (!overExtended)
                        {
                            return false;
                        }
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
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (!overExtended)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (x * slopeLeft >= y)
                    {
                        if (overExtended)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (!overExtended)
                        {
                            return false;
                        }
                    }
                }
            }

            if (verticalRight)
            {
                return checkAboveRight ? x >= 0f : x <= 0f;
            }
            else
            {
                return checkAboveRight ? x * slopeRight <= y : x * slopeRight >= y;
            }
        }
    }
}
