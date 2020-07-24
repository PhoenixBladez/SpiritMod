
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
	public class CrystalRainDust : ModDust
	{
		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X * 0.3f;
			dust.scale -= 0.03f;

			if (dust.scale < 0.05f) {
				dust.active = false;
			}

			return false;
		}
	}
}
