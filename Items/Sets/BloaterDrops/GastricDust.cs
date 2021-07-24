using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BloaterDrops
{
	public class GastricDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noLight = true;
			dust.noGravity = true;
			dust.velocity /= 2f;
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X;

			dust.scale -= 0.03f;
			if (dust.scale < 0.1f)
				dust.active = false;
			return false;
		}
	}
}