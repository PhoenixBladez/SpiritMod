using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
	public class BlizzardDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.frame = new Rectangle(0, Texture.Height / 3 * Main.rand.Next(3), Texture.Width(), Texture.Height() / 3);
			dust.noLight = true;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor) => dust.color;

		public override bool Update(Dust dust)
		{
			dust.color = Lighting.GetColor((int)(dust.position.X / 16), (int)(dust.position.Y / 16)).MultiplyRGB(Color.Lerp(Color.White * .1f, new Color(98, 252, 252), dust.scale / 1f)) * 0.34f;
			dust.scale *= 0.992f;
			dust.position += dust.velocity;
			dust.rotation += .02f;
			dust.velocity *= 0.97f;

			if (dust.scale <= 0.4f) dust.active = false;
			return false;
		}
	}
}