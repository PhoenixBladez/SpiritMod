using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace SpiritMod.Mechanics.Coverings
{
    public struct CoverData
    {
        public byte Type { get; set; }
        public byte Data { get; set; }

        public int Orientation
        {
            get => Data & 0b00001111;
            set
            {
                Data = (byte)((Data & 0b11110000) | (value & 0b00001111));
            }
        }
        public int Variation
        {
            get => (Data & 0b11110000) >> 4;
            set
            {
                Data = (byte)((Data & 0b00001111) | ((value << 4) & 0b11110000));
            }
        }
    }
}
