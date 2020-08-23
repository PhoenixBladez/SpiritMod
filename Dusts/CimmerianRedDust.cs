using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
	public class CimmerianRedDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.noLight = false;
			dust.color.R = 255;
			dust.color.G = 31;
			dust.color.B = 49;
			dust.alpha = 255;
		}
		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return dust.color;
		}
		public override bool Update(Dust dust)
		{
			//Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 0.196f/2, 0.870588235f/2, 0.464705882f/2);
			dust.position += dust.velocity;
			dust.noGravity = true;
			dust.rotation += 0.05f;
			dust.alpha -= 5;
			dust.velocity *= 0.91f;
			dust.scale *= 0.982f;
			if (dust.scale < 0.03f) {
				dust.active = false;
			}
			return false;
		}
	}
}
