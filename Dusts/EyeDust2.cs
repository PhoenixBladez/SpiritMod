using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
    public class EyeDust2 : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.scale = 8f;
			dust.alpha = 0;
		}
		public override bool Update(Dust dust)
		{
			dust.scale *= 0.95f;
			if (dust.scale < 0.5f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}
