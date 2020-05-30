using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
    public class FloranDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.scale = 1f;
			dust.alpha = 0;
			dust.velocity = dust.velocity * 0.1f;
		}
		public override bool Update(Dust dust)
		{
            Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 0.196f/2, 0.870588235f/2, 0.464705882f/2);
			dust.noGravity = true;
			dust.scale *= 0.66f;
			dust.alpha += 12;
			if (dust.scale < 0.5f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}
