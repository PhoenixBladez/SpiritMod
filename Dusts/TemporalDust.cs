using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
	public class TemporalDust : ModDust
	{
		public static int _type;

		private static readonly Vector3 LIGHT_COLOR = new Vector3(0xc1, 0x25, 0xaf)/255 * 0.6f;

		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.customData = 0;
		}

		public override bool Update(Dust dust)
		{
			if (!dust.noLight)
				Lighting.AddLight((int)(dust.position.X) >> 4, (int)(dust.position.Y) >> 4, LIGHT_COLOR.X, LIGHT_COLOR.Y, LIGHT_COLOR.Z);

			dust.position += dust.velocity;
			if (dust.customData is int)
			{
				int counter = (int)dust.customData;

				if (counter > 20)
				{
					dust.scale *= 0.98f;
					dust.alpha++;
				}
				if (dust.alpha > 250)
					dust.active = false;

				dust.customData = counter + 1;
			}
			else
				dust.active = false;

			return false;
		}
	}
}