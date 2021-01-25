using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SpiritMod.Noise
{
    //based on https://github.com/KdotJPG/OpenSimplex2/blob/master/csharp/OpenSimplex2S.cs
    //credit to KdotJPG, DarkShadow and anyone else who contributed to it
    public class OpenSimplexNoise : INoise
    {
        private const int PSIZE = 2048;
        private const int PMASK = 2047;

        private short[] perm;
        private Grad2[] permGrad2;

        private double _1Dy;

        public OpenSimplexNoise() : this(DateTime.Now.Ticks) { }
        public OpenSimplexNoise(long seed)
        {
            perm = new short[PSIZE];
            permGrad2 = new Grad2[PSIZE];
            short[] source = new short[PSIZE];
            for (short i = 0; i < PSIZE; i++)
                source[i] = i;
            for (int i = PSIZE - 1; i >= 0; i--)
            {
                seed = seed * 6364136223846793005L + 1442695040888963407L;
                int r = (int)((seed + 31) % (i + 1));
                if (r < 0)
                    r += (i + 1);
                perm[i] = source[r];
                permGrad2[i] = GRADIENTS_2D[perm[i]];
                source[r] = source[i];
            }
            _1Dy = seed * 0.00001;
        }

        public float Noise1D(float x)
        {
            return Noise2D(x, (float)_1Dy);
        }

        public float Noise2D(float x, float y)
        {
            // Get points for A2* lattice
            double s = 0.366025403784439 * (x + y);
            double xs = x + s, ys = y + s;

            return 0.5f + (float)noise2_Base(xs, ys) * 0.5f;
        }

        public float Noise2DOctaves(float x, float y, int octaves, float lacunarity = 1.75f)
        {
            //initial values
            float value = 0f;
            float gain = 1f / lacunarity;

            for (int i = 0; i < octaves; i++)
            {
                value += Noise2D(x, y) * gain;
                //multiply the values to move them and get different data
                x *= lacunarity;
                y *= lacunarity;
                //each extra octave only effects the final value by a half.
                gain *= 0.5f;
            }

            return value;
        }

        private double noise2_Base(double xs, double ys)
        {
            double value = 0;

            // Get base points and offsets
            int xsb = FastFloor(xs), ysb = FastFloor(ys);
            double xsi = xs - xsb, ysi = ys - ysb;

            // Index to point list
            int a = (int)(xsi + ysi);
            int index =
                (a << 2) |
                (int)(xsi - ysi / 2 + 1 - a / 2.0) << 3 |
                (int)(ysi - xsi / 2 + 1 - a / 2.0) << 4;

            double ssi = (xsi + ysi) * -0.211324865405187;
            double xi = xsi + ssi, yi = ysi + ssi;

            // Point contributions
            for (int i = 0; i < 4; i++)
            {
                LatticePoint2D c = LOOKUP_2D[index + i];

                double dx = xi + c.dx, dy = yi + c.dy;
                double attn = 2.0 / 3.0 - dx * dx - dy * dy;
                if (attn <= 0) continue;

                int pxm = (xsb + c.xsv) & PMASK, pym = (ysb + c.ysv) & PMASK;
                Grad2 grad = permGrad2[perm[pxm] ^ pym];
                double extrapolation = grad.dx * dx + grad.dy * dy;

                attn *= attn;
                value += attn * attn * extrapolation;
            }

            return value;
        }

        //Utility

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int FastFloor(double x)
        {
            int xi = (int)x;
            return x < xi ? xi - 1 : xi;
        }

        //Lookup Tables & Gradients

        private static LatticePoint2D[] LOOKUP_2D;
        private const double N2 = 0.05481866495625118;
        private static Grad2[] GRADIENTS_2D;

        static OpenSimplexNoise()
        {
            LOOKUP_2D = new LatticePoint2D[32];

            for (int i = 0; i < 8; i++)
            {
                int i1, j1, i2, j2;
                if ((i & 1) == 0)
                {
                    if ((i & 2) == 0) { i1 = -1; j1 = 0; } else { i1 = 1; j1 = 0; }
                    if ((i & 4) == 0) { i2 = 0; j2 = -1; } else { i2 = 0; j2 = 1; }
                }
                else
                {
                    if ((i & 2) != 0) { i1 = 2; j1 = 1; } else { i1 = 0; j1 = 1; }
                    if ((i & 4) != 0) { i2 = 1; j2 = 2; } else { i2 = 1; j2 = 0; }
                }
                LOOKUP_2D[i * 4 + 0] = new LatticePoint2D(0, 0);
                LOOKUP_2D[i * 4 + 1] = new LatticePoint2D(1, 1);
                LOOKUP_2D[i * 4 + 2] = new LatticePoint2D(i1, j1);
                LOOKUP_2D[i * 4 + 3] = new LatticePoint2D(i2, j2);
            }

            GRADIENTS_2D = new Grad2[PSIZE];
            Grad2[] grad2 = {
                new Grad2( 0.130526192220052,  0.99144486137381),
                new Grad2( 0.38268343236509,   0.923879532511287),
                new Grad2( 0.608761429008721,  0.793353340291235),
                new Grad2( 0.793353340291235,  0.608761429008721),
                new Grad2( 0.923879532511287,  0.38268343236509),
                new Grad2( 0.99144486137381,   0.130526192220051),
                new Grad2( 0.99144486137381,  -0.130526192220051),
                new Grad2( 0.923879532511287, -0.38268343236509),
                new Grad2( 0.793353340291235, -0.60876142900872),
                new Grad2( 0.608761429008721, -0.793353340291235),
                new Grad2( 0.38268343236509,  -0.923879532511287),
                new Grad2( 0.130526192220052, -0.99144486137381),
                new Grad2(-0.130526192220052, -0.99144486137381),
                new Grad2(-0.38268343236509,  -0.923879532511287),
                new Grad2(-0.608761429008721, -0.793353340291235),
                new Grad2(-0.793353340291235, -0.608761429008721),
                new Grad2(-0.923879532511287, -0.38268343236509),
                new Grad2(-0.99144486137381,  -0.130526192220052),
                new Grad2(-0.99144486137381,   0.130526192220051),
                new Grad2(-0.923879532511287,  0.38268343236509),
                new Grad2(-0.793353340291235,  0.608761429008721),
                new Grad2(-0.608761429008721,  0.793353340291235),
                new Grad2(-0.38268343236509,   0.923879532511287),
                new Grad2(-0.130526192220052,  0.99144486137381)
            };
            for (int i = 0; i < grad2.Length; i++)
            {
                grad2[i].dx /= N2; grad2[i].dy /= N2;
            }
            for (int i = 0; i < PSIZE; i++)
            {
                GRADIENTS_2D[i] = grad2[i % grad2.Length];
            }
        }

        private class LatticePoint2D
        {
            public int xsv, ysv;
            public double dx, dy;
            public LatticePoint2D(int xsv, int ysv)
            {
                this.xsv = xsv; this.ysv = ysv;
                double ssv = (xsv + ysv) * -0.211324865405187;
                this.dx = -xsv - ssv;
                this.dy = -ysv - ssv;
            }
        }

        private class Grad2
        {
            public double dx, dy;
            public Grad2(double dx, double dy)
            {
                this.dx = dx; this.dy = dy;
            }
        }
    }
}
