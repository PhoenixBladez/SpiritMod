using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
	public class BloodSiphonDust : ModDust
	{
		public override bool Update(Dust dust)
		{
			dust.noGravity = true;
			dust.scale -= 0.01f;

			Dust dust2 = dust;
			dust2.scale += 0.007f;
			// dust.alpha += 12;
			int index = (int)dust.fadeIn - 1;
			if(index >= 0 && index <= Main.maxPlayers) {
				Vector2 vector = dust.position - Main.player[index].Center;
				float vectorLength = vector.Length();
				if(vectorLength <= 16) {
					dust.active = false;
				}

				vectorLength = 100f - vectorLength;
				if(vectorLength > 0f) {
					dust2 = dust;
					dust2.scale -= vectorLength * 0.0015f;
				}

				vector.Normalize();

				float num112 = (1f - dust.scale) * 20f;
				vector *= -num112;

				dust.velocity = (dust.velocity * 4f + vector) / 5f;
			}

			dust.position += dust.velocity;

			return false;
		}
	}
}
