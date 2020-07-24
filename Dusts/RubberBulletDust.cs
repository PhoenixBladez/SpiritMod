using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
	public class RubberBulletDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, 0, 6, 10);
			dust.alpha = 0;
			dust.scale = 1f;
			dust.noGravity = false;
		}

		public override bool Update(Dust dust)
		{
			// dust.velocity = new Vector2(0,-2);
			dust.alpha += Main.rand.Next(8);
			if (dust.alpha >= 255) {
				dust.active = false;
			}
			dust.rotation = 0;
			return true;
		}
	}
}