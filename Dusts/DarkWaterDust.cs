﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
	public class DarkWaterDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.noLight = false;
			dust.color.R = 50;
			dust.color.G = 54;
			dust.color.B = 66;
		}
		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return dust.color;
		}
		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.velocity.Y += 0.2f;
			if (Main.tile[(int)dust.position.X / 16, (int)dust.position.Y / 16].HasTile && Main.tile[(int)dust.position.X / 16, (int)dust.position.Y / 16].collisionType == 1)
			{
				dust.velocity *= -0.5f;
			}

			dust.rotation = dust.velocity.ToRotation();
			dust.scale *= 0.99f;
			if (dust.scale < 0.2f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}
