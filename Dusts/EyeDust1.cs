using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
    public class EyeDust1 : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.scale = 1f;
			dust.alpha = 0;
		}
		private void Trail(Vector2 from, Vector2 to)
		{
			float distance = Vector2.Distance(from, to);
			float step = 1 / distance;
			for (float w = 0; w < distance; w+= 2)
			{
				Dust.NewDustPerfect(Vector2.Lerp(from, to, w * step), 95, Vector2.Zero);
			}
		}
		public override bool Update(Dust dust)
		{
			Vector2 currentSpeed = new Vector2(dust.velocity.X, dust.velocity.Y);
			dust.velocity = currentSpeed.RotatedBy(Main.rand.Next(-1, 2) * (Math.PI / 20));
			dust.position += dust.velocity;
			dust.scale *= 0.95f;
			if (dust.scale < 0.5f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}
