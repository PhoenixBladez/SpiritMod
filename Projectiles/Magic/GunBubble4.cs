﻿using Terraria.ModLoader;
using Terraria;

namespace SpiritMod.Projectiles.Magic
{
	public class GunBubble4 : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.width = 30;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 150;
			projectile.alpha = 75;
		}

		public override void AI()
		{
			if (projectile.timeLeft == 150)
			{
				projectile.scale = Main.rand.NextFloat(0.6f,1.1f);
			}
			projectile.velocity.X *= 0.99f;
			projectile.velocity.Y -= 0.015f;
		}
	}
}