using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
    public class TemporalDust : ModDust
	{
		private static readonly Vector3 lightColor = new Vector3(0xc1, 0x25, 0xaf) / 255 * 0.6f;

		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.customData = 0;
		}

		public override bool Update(Dust dust)
		{
			if (!dust.noLight)
            {
                Lighting.AddLight((int)(dust.position.X) >> 4, (int)(dust.position.Y) >> 4, lightColor.X, lightColor.Y, lightColor.Z);
            }

            dust.position += dust.velocity;

            if (dust.customData is int counter)
            {
                if (counter > 20)
                {
                    dust.scale *= 0.98f;
                    dust.alpha++;
                }

                if (dust.alpha > 250)
                {
                    dust.active = false;
                }

                dust.customData = counter + 1;
            }
            else
            {
                dust.active = false;
            }

            return false;
		}
	}
}