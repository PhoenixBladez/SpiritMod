using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
	public class Mana : ModDust
	{
		public override void OnSpawn(Dust dust) {
			UpdateType = 235;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor) 
		=> new Color((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue, 0);
	}
}