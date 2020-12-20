using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Dusts
{
	public class BubbleDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, 0, 14, 14);
			dust.scale = 1f;
			dust.alpha = 75;
			dust.velocity = dust.velocity * 0.1f;
		}
		public override bool Update(Dust dust)
		{
			Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 1f / 2, 0.223529412f / 2, 0.917647059f / 2);
			dust.noGravity = true;
			dust.position += dust.velocity;
			dust.scale *= 0.99f;
			dust.alpha += 8;
			if (dust.scale < 0.5f) {
				dust.active = false;
			}
			return false;
		}
	}
}
