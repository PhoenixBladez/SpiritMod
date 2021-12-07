using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace SpiritMod.Dusts
{
	public class ScreamingDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.noLight = false;
			dust.color.R = 75;
			dust.color.G = 151;
			dust.color.B = 49;
			dust.color = Color.Lerp(dust.color, new Color(44, 106, 22), Main.rand.NextFloat());
			dust.frame = new Rectangle(0, Main.rand.NextBool() ? 0 : 10, 10, 10);
		}
		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return dust.color * Math.Min(dust.fadeIn, 1) * (1 - (dust.alpha / 255f));
		}
		public override bool Update(Dust dust)
		{
			//Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 0.196f/2, 0.870588235f/2, 0.464705882f/2);
			dust.fadeIn += 0.1f;
			dust.position += dust.velocity;
			dust.noGravity = true;
			dust.rotation += 0.05f;
			if (dust.fadeIn > 1)
				dust.alpha += 9;

			dust.scale *= 0.99f;
			if (dust.alpha > 220)
			{
				dust.active = false;
			}
			return false;
		}
	}
}
