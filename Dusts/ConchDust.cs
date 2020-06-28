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
		}
		public override bool Update(Dust dust)
		{
			Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 0.196f / 2, 0.870588235f / 2, 0.964705882f / 2);
			dust.noGravity = true;
			dust.position += dust.velocity;
			dust.velocity *= 0.92f;
			dust.alpha += 40;
			if(dust.alpha > 200) {
				dust.active = false;
			}
			return false;
		}
	}
}
