using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
	public class ConchDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.scale = 1f;
			dust.alpha = 0;
			dust.velocity = dust.velocity * 0.1f;
			dust.noGravity = false;
		}
		public override bool Update(Dust dust)
		{
			dust.velocity.X *= 0.99f;
			dust.velocity.Y += 0.1f;
			dust.position += dust.velocity;
			dust.alpha += 1;
			if(dust.alpha > 200) {
				dust.active = false;
			}
			return false;
		}
	}
}
