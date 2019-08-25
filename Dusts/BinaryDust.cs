using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
	public class BinaryDust : ModDust
	{
		public static readonly int _type;

		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, Main.rand.NextBool() ? 0 : 16, 12, 16);
			dust.alpha = 0;
			dust.rotation = 0;
			dust.scale = 1f;
			dust.noGravity = true;
		}

		public override bool Update(Dust dust)
		{
			float light = 1 - dust.alpha / 255f;
			Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), light * 0.2f, light * 0.5f, light * 0.1f);
			dust.alpha += 4;
			if (dust.alpha >= 255)
				dust.active = false;
			return false;
		}
	}
}