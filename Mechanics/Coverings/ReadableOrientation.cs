using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace SpiritMod.Mechanics.Coverings
{
    public struct ReadableOrientation
    {
        private BitsByte _orientation;

        public ReadableOrientation(int orientation) => _orientation = (BitsByte)orientation;

        public bool Up { get => _orientation[0]; set => _orientation[0] = value; }
        public bool Right { get => _orientation[1]; set => _orientation[1] = value; }
        public bool Down { get => _orientation[2]; set => _orientation[2] = value; }
        public bool Left { get => _orientation[3]; set => _orientation[3] = value; }

        public static explicit operator ReadableOrientation(byte value)
        {
            return new ReadableOrientation(value);
        }

        public static explicit operator byte(ReadableOrientation value)
        {
            return value._orientation;
        }
    }
}
