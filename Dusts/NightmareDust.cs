using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
    public class NightmareDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.scale = 1f;
			dust.alpha = 0;
			dust.velocity = dust.velocity * 0.1f;
		}
		public override bool Update(Dust dust)
		{
			dust.noGravity = true;
			dust.position += dust.velocity;
			dust.velocity *= 0.92f;
			dust.scale *= 0.98f;
			dust.alpha += 12;
			if (dust.scale < 0.5f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}
