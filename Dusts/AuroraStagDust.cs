using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
	public class AuroraStagDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			var texture = ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			dust.noGravity = true;
			dust.frame = new Rectangle(0, texture.Height / 3 * Main.rand.Next(3), texture.Width, texture.Height / 3);
			dust.scale = 1.2f;
			dust.noLight = true;
		}

		public override bool Update(Dust dust)
		{
			dust.rotation += 0.005f;
			dust.position += dust.velocity;
			dust.scale -= 0.01f;

			if (dust.scale <= 0)
				dust.active = false;

			Lighting.AddLight(dust.position, 0.5f * dust.scale, 0.7f * dust.scale, 1f * dust.scale);
			return false;
		}
	}
}