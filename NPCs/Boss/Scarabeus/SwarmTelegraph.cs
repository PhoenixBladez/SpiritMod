using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Scarabeus
{
	public class SwarmTelegraph : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Ball");
		}

		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.hide = true;
			projectile.timeLeft = 360;
			projectile.tileCollide = false;
			projectile.scale = 0.75f;
			projectile.alpha = 255;
			projectile.extraUpdates = 3;
		}

		public override void AI()
		{
			//follow the exact same path as the scarab line
			if (++projectile.ai[1] > 110) {
				projectile.alpha += 7;
				if (projectile.alpha > 255) {
					projectile.Kill();
				}
			}
			else
				projectile.alpha = Math.Max(projectile.alpha - 7, 0);

			if (projectile.velocity.Length() < 26)
				projectile.velocity *= 1.02f;
		}
	}
}