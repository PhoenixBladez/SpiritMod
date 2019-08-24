using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
	public class PlagueDust : ModDust
	{
		public static int _type;

		private const float GROWTH_RATE = 0.04f;

		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.noLight = true;
			dust.scale = 0.1f;
			dust.alpha = 100;
			dust.velocity = dust.velocity * 0.1f;
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			if (dust.scale < 1.4f)
			{
				dust.scale += GROWTH_RATE;
				dust.velocity.Y -= 0.6f * GROWTH_RATE;
			}
			else if (dust.alpha < 255)
			{
				dust.alpha += 2;
			}
			else
				dust.active = false;
			return false;
		}
	}
}