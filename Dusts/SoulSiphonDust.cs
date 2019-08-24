using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
	public class SoulSiphonDust : ModDust
	{
		public override bool Update(Dust dust)
		{
			dust.noGravity = true;
			dust.scale -= 0.01f;

			Dust dust5 = dust;
			dust5.scale += 0.007f;
			int num110 = (int)dust.fadeIn - 1;
			if (num110 >= 0 && num110 <= 255)
			{
				Vector2 vector5 = dust.position - Main.player[num110].Center;
				float num111 = vector5.Length();
				if (num111 <= 16)
					dust.active = false;
				num111 = 100f - num111;
				if (num111 > 0f)
				{
					dust5 = dust;
					dust5.scale -= num111 * 0.0015f;
				}
				vector5.Normalize();
				float num112 = (1f - dust.scale) * 20f;
				vector5 *= -num112;
				dust.velocity = (dust.velocity * 4f + vector5) / 5f;
			}

			dust.position += dust.velocity;
			return false;
		}
	}
}
