using System;
using System.Collections.Generic;
using System.Text;

namespace SpiritMod.Utilities.Noise
{
    public interface INoise
    {
        float Noise1D(float x);
        float Noise2D(float x, float y);
        float Noise2DOctaves(float x, float y, int octaves, float lacunarity = 1.75f);
    }
}
