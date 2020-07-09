using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Terraria;
using Terraria.ID;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.Utilities;

using ReLogic.Graphics;

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
			if(rotationAmount < 0) { rot = Main.rand.NextFloat(0, (float)Math.PI * 2); } else { rot = rotationAmount; }

			float density = 1 / dustDensity * 0.1f;

			for(float k = 0; k < 6.28f; k += density) {
				float rand = 0;
				if(randomAmount > 0) { rand = Main.rand.NextFloat(-0.01f, 0.01f) * randomAmount; }

				float x = (float)Math.Cos(k + rand);
				float y = (float)Math.Sin(k + rand);
				float mult = ((Math.Abs(((k * (pointAmount / 2)) % (float)Math.PI) - (float)Math.PI / 2)) * pointDepthMult) + pointDepthMultOffset;//triangle wave function
				Dust.NewDustPerfect(position, dustType, new Vector2(x, y).RotatedBy(rot) * mult * mainSize, 0, default, dustSize).noGravity = noGravity;
			}
		}

		public static void DrawCircle(Vector2 position, int dustType, float mainSize = 1, float RatioX = 1, float RatioY = 1, float dustDensity = 1, float dustSize = 1f, float randomAmount = 0, float rotationAmount = 0)
		{
			float rot;
			if(rotationAmount < 0) { rot = Main.rand.NextFloat(0, (float)Math.PI * 2); } else { rot = rotationAmount; }

			float density = 1 / dustDensity * 0.1f;

			for(float k = 0; k < 6.28f; k += density) {
				float rand = 0;
				if(randomAmount > 0) { rand = Main.rand.NextFloat(-0.01f, 0.01f) * randomAmount; }

				float x = (float)Math.Cos(k + rand) * RatioX;
				float y = (float)Math.Sin(k + rand) * RatioY;
				Dust.NewDustPerfect(position, dustType, new Vector2(x, y).RotatedBy(rot) * mainSize, 0, default, dustSize);
			}
		}
		public static void DrawTriangle(Vector2 position, int dustType, float size, float dustDensity = 1f, float dustSize = 2f, float rotationAmount = -1, bool noGravity = true)
		{
			float rot;
			if(rotationAmount < 0) { rot = Main.rand.NextFloat(0, (float)Math.PI * 2); } else { rot = rotationAmount; }
			float density = 1 / dustDensity * 0.1f;
			float x = 1;
			float y = 0;
			for(float k = 0; k < 6.3f; k += density)
			{ 
				if (k % 2.093333f <= density)
				{
					x = (float)Math.Cos(k);
					y = (float)Math.Sin(k);
				} 
				Vector2 offsetVect = new Vector2(x,y);
				offsetVect = offsetVect.RotatedBy(2.093333f);
				offsetVect *= ((k % 2.093333f) / 2.093333f) * 2f;
				Dust.NewDustPerfect(position, dustType, (new Vector2(x, y) + offsetVect).RotatedBy(rot) * size, 0, default, dustSize).noGravity = noGravity;
				//not the cleanest, but im tired of trying, ive legit been at this for 2 hours. Maybe im missing something really obvious, but hardcode a fucking hoy
				offsetVect = new Vector2(x,y);
				offsetVect = offsetVect.RotatedBy(-1.046667);
				offsetVect *= ((k % 2.093333f) / 2.093333f);
				Dust.NewDustPerfect(position, dustType, (new Vector2(x, y) + offsetVect).RotatedBy(rot) * size, 0, default, dustSize).noGravity = noGravity;
			}
		}
		public static void DrawDiamond(Vector2 position, int dustType, float size, float dustDensity = 1f, float dustSize = 2f, float rotationAmount = -1, bool noGravity = true)
		{
			float rot;
			if(rotationAmount < 0) { rot = Main.rand.NextFloat(0, (float)Math.PI * 2); } else { rot = rotationAmount; }
			float density = 1 / dustDensity * 0.1f;
			float x = 1;
			float y = 0;
			for(float k = 0; k < 6.3f; k += density)
			{ 
				if (k % 1.57f <= density)
				{
					x = (float)Math.Cos(k);
					y = (float)Math.Sin(k);
				} 
				Vector2 offsetVect = new Vector2(x,y);
				offsetVect = offsetVect.RotatedBy(1.57f);
				offsetVect *= ((k % 1.57f) / 1.57f);
				Dust.NewDustPerfect(position, dustType, (new Vector2(x, y) + offsetVect).RotatedBy(rot) * size, 0, default, dustSize).noGravity = noGravity;
				//not the cleanest, but im tired of trying, ive legit been at this for 2 hours. Maybe im missing something really obvious, but hardcode a fucking hoy
				offsetVect = new Vector2(x,y);
				offsetVect = offsetVect.RotatedBy(-1.57f);
				offsetVect *= ((k % 1.57f) / 1.57f);
				Dust.NewDustPerfect(position, dustType, (new Vector2(x, y) + offsetVect).RotatedBy(rot) * size, 0, default, dustSize).noGravity = noGravity;
			}
		}
		
		public static void DrawDustImage(Vector2 position, int dustType, float size, string imagePath, float dustSize = 1f)
		{
			float rot = Main.rand.NextFloat(-0.34f, 0.34f);
			Texture2D glyphTexture = ModContent.GetTexture(imagePath);
			Color[] data = new Color[glyphTexture.Width * glyphTexture.Height];
			glyphTexture.GetData(data);
				for (int i = 0; i < glyphTexture.Width; i+=2)
				{
					for (int j = 0; j < glyphTexture.Height; j+=2)
					{
						Color alpha = data[j * glyphTexture.Width + i];
						if (alpha == new Color(0, 0, 0))
						{
							double dustX = (i - (glyphTexture.Width / 2));
							double dustY = (j - (glyphTexture.Height / 2));
							dustX *= size;
							dustY *= size;
							Dust.NewDustPerfect(position,dustType, new Vector2((float)dustX, (float)dustY).RotatedBy(rot)).noGravity = true;
						}
					}
				}
		}
	}
}