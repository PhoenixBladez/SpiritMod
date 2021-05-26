using System;
using System.Linq;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SpiritMod;

namespace SpiritMod
{
	public static class DustHelper
	{
		public static void DrawStar(Vector2 position, int dustType, float pointAmount = 5, float mainSize = 1, float dustDensity = 1, float dustSize = 1f, float pointDepthMult = 1f, float pointDepthMultOffset = 0.5f, bool noGravity = false, float randomAmount = 0, float rotationAmount = -1)
		{
			float rot;
			if (rotationAmount < 0) { rot = Main.rand.NextFloat(0, (float)Math.PI * 2); } else { rot = rotationAmount; }

			float density = 1 / dustDensity * 0.1f;

			for (float k = 0; k < 6.28f; k += density) {
				float rand = 0;
				if (randomAmount > 0) { rand = Main.rand.NextFloat(-0.01f, 0.01f) * randomAmount; }

				float x = (float)Math.Cos(k + rand);
				float y = (float)Math.Sin(k + rand);
				float mult = ((Math.Abs(((k * (pointAmount / 2)) % (float)Math.PI) - (float)Math.PI / 2)) * pointDepthMult) + pointDepthMultOffset;//triangle wave function
				Dust.NewDustPerfect(position, dustType, new Vector2(x, y).RotatedBy(rot) * mult * mainSize, 0, default, dustSize).noGravity = noGravity;
			}
		}

		public static void DrawStar2(Vector2 position, int dustType, float pointAmount = 5, float mainSize = 1, float dustDensity = 1, float dustSize = 1f, float pointDepthMult = 1f, float pointDepthMultOffset = 0.5f, bool noGravity = false, float randomAmount = 0, float rotationAmount = -1, Color? color = null)
		{
			float rot;
			if (rotationAmount < 0) { rot = Main.rand.NextFloat(0, (float)Math.PI * 2); } else { rot = rotationAmount; }

			float density = 1 / dustDensity * 0.1f;

			for (float k = 0; k < 6.28f; k += density) {
				float rand = 0;
				if (randomAmount > 0) { rand = Main.rand.NextFloat(-0.01f, 0.01f) * randomAmount; }

				float x = (float)Math.Cos(k + rand);
				float y = (float)Math.Sin(k + rand);
				float mult = ((Math.Abs(((k * (pointAmount / 2)) % (float)Math.PI) - (float)Math.PI / 2)) * pointDepthMult) + pointDepthMultOffset;//triangle wave function

				Dust.NewDustPerfect(position, dustType, new Vector2(x, y).RotatedBy(rot) * mult * mainSize, 0, color ?? default, dustSize).noGravity = noGravity;
			}
		}

		public static void DrawCircle(Vector2 position, int dustType, float mainSize = 1, float RatioX = 1, float RatioY = 1, float dustDensity = 1, float dustSize = 1f, float randomAmount = 0, float rotationAmount = 0, bool nogravity = false)
		{
			float rot;
			if (rotationAmount < 0) { rot = Main.rand.NextFloat(0, (float)Math.PI * 2); } else { rot = rotationAmount; }

			float density = 1 / dustDensity * 0.1f;

			for (float k = 0; k < 6.28f; k += density) {
				float rand = 0;
				if (randomAmount > 0) { rand = Main.rand.NextFloat(-0.01f, 0.01f) * randomAmount; }

				float x = (float)Math.Cos(k + rand) * RatioX;
				float y = (float)Math.Sin(k + rand) * RatioY;
                if (dustType == 222 || dustType == 130 || nogravity)
                {
                    Dust.NewDustPerfect(position, dustType, new Vector2(x, y).RotatedBy(rot) * mainSize, 0, default, dustSize).noGravity = true;
                }
                else
                {
                    Dust.NewDustPerfect(position, dustType, new Vector2(x, y).RotatedBy(rot) * mainSize, 0, default, dustSize);
                }
			}
		}
		public static void DrawTriangle(Vector2 position, int dustType, float size, float dustDensity = 1f, float dustSize = 2f, float rotationAmount = -1, bool noGravity = true)
		{
            float rot;
			if (rotationAmount < 0) { rot = Main.rand.NextFloat(0, (float)Math.PI * 2); } else { rot = rotationAmount; }
			float density = 1 / dustDensity * 0.1f;
			float x = 1;
			float y = 0;
			for (float k = 0; k < 6.3f; k += density) {
				if (k % 2.093333f <= density) {
					x = (float)Math.Cos(k);
					y = (float)Math.Sin(k);
				}
				Vector2 offsetVect = new Vector2(x, y);
				offsetVect = offsetVect.RotatedBy(2.093333f);
				offsetVect *= ((k % 2.093333f) / 2.093333f) * 2f;
				Dust.NewDustPerfect(position, dustType, (new Vector2(x, y) + offsetVect).RotatedBy(rot) * size, 0, default, dustSize).noGravity = noGravity;
				//not the cleanest, but im tired of trying, ive legit been at this for 2 hours. Maybe im missing something really obvious, but hardcode a fucking hoy
				offsetVect = new Vector2(x, y);
				offsetVect = offsetVect.RotatedBy(-1.046667);
				offsetVect *= ((k % 2.093333f) / 2.093333f);
				Dust.NewDustPerfect(position, dustType, (new Vector2(x, y) + offsetVect).RotatedBy(rot) * size, 0, default, dustSize).noGravity = noGravity;
			}
		}
		public static void DrawDiamond(Vector2 position, int dustType, float size, float dustDensity = 1f, float dustSize = 2f, float rotationAmount = -1, bool noGravity = true)
		{
			float rot;
			if (rotationAmount < 0) { rot = Main.rand.NextFloat(0, (float)Math.PI * 2); } else { rot = rotationAmount; }
			float density = 1 / dustDensity * 0.1f;
			float x = 1;
			float y = 0;
			for (float k = 0; k < 6.3f; k += density) {
				if (k % 1.57f <= density) {
					x = (float)Math.Cos(k);
					y = (float)Math.Sin(k);
				}
				Vector2 offsetVect = new Vector2(x, y);
				offsetVect = offsetVect.RotatedBy(1.57f);
				offsetVect *= ((k % 1.57f) / 1.57f);
				Dust.NewDustPerfect(position, dustType, (new Vector2(x, y) + offsetVect).RotatedBy(rot) * size, 0, default, dustSize).noGravity = noGravity;
				//not the cleanest, but im tired of trying, ive legit been at this for 2 hours. Maybe im missing something really obvious, but hardcode a fucking hoy
				offsetVect = new Vector2(x, y);
				offsetVect = offsetVect.RotatedBy(-1.57f);
				offsetVect *= ((k % 1.57f) / 1.57f);
				Dust.NewDustPerfect(position, dustType, (new Vector2(x, y) + offsetVect).RotatedBy(rot) * size, 0, default, dustSize).noGravity = noGravity;
			}
		}

		public static void DrawDustImage(Vector2 position, int dustType, float size, string imagePath, float dustSize = 1f, bool noGravity = true, float rot = 0.34f)
		{
			if (Main.netMode != NetmodeID.Server) {
				float rotation = Main.rand.NextFloat(0 - rot, rot);
				Texture2D glyphTexture = ModContent.GetTexture(imagePath);
				Color[] data = new Color[glyphTexture.Width * glyphTexture.Height];
				glyphTexture.GetData(data);
				for (int i = 0; i < glyphTexture.Width; i += 2) {
					for (int j = 0; j < glyphTexture.Height; j += 2) {
						Color alpha = data[j * glyphTexture.Width + i];
						if (alpha == new Color(0, 0, 0)) {
							double dustX = (i - (glyphTexture.Width / 2));
							double dustY = (j - (glyphTexture.Height / 2));
							dustX *= size;
							dustY *= size;
							Dust.NewDustPerfect(position, dustType, new Vector2((float)dustX, (float)dustY).RotatedBy(rotation)).noGravity = noGravity;
						}
					}
				}
			}
		}

		public static void DrawDustImageRainbow(Vector2 position, float size, string imagePath, float dustSize = 1f, bool noGravity = true, float rot = 0.34f)
		{
			int red = Main.rand.Next(60, 255);
			int green = Main.rand.Next(60, 255);
			int blue = Main.rand.Next(60, 255);
			Color color = new Color(red, green, blue);
			if (Main.netMode != NetmodeID.Server) {
				float rotation = Main.rand.NextFloat(0 - rot, rot);
				Texture2D glyphTexture = ModContent.GetTexture(imagePath);
				Color[] data = new Color[glyphTexture.Width * glyphTexture.Height];
				glyphTexture.GetData(data);
				for (int i = 0; i < glyphTexture.Width; i += 2) {
					for (int j = 0; j < glyphTexture.Height; j += 2) {
						Color alpha = data[j * glyphTexture.Width + i];
						if (alpha == new Color(0, 0, 0)) {
							double dustX = (i - (glyphTexture.Width / 2));
							double dustY = (j - (glyphTexture.Height / 2));
							dustX *= size;
							dustY *= size;
							Vector2 dir = new Vector2((float)dustX, (float)dustY).RotatedBy(rotation);
							Dust.NewDustPerfect(position, 267, dir, 0, color, dustSize).noGravity = noGravity;
						}
					}
				}
			}
		}

		public static void DrawElectricity(Vector2 point1, Vector2 point2, int dusttype, float scale = 1, int armLength = 30, Color color = default, float density = 0.05f)
        {
            int nodeCount = (int)Vector2.Distance(point1, point2) / armLength;
            Vector2[] nodes = new Vector2[nodeCount + 1];

            nodes[nodeCount] = point2; //adds the end as the last point

            for (int k = 1; k < nodes.Count(); k++)
            {
                //Sets all intermediate nodes to their appropriate randomized dot product positions
                nodes[k] = Vector2.Lerp(point1, point2, k / (float)nodeCount) +
                    (k == nodes.Count() - 1 ? Vector2.Zero : Vector2.Normalize(point1 - point2).RotatedBy(1.58f) * Main.rand.NextFloat(-armLength / 2, armLength / 2));

                //Spawns the dust between each node
                Vector2 prevPos = k == 1 ? point1 : nodes[k - 1];
                for (float i = 0; i < 1; i += density)
                {
                    Dust d = Dust.NewDustPerfect(Vector2.Lerp(prevPos, nodes[k], i), dusttype, Vector2.Zero, 0, color, scale);
					d.noGravity = true;
                }
            }
        }

	}
}