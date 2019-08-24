using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class AsuraTrail : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Asura's Trail");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;

			projectile.timeLeft = 30;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.hostile = false;
			projectile.melee = true;
			projectile.friendly = true;
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(4) == 0)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 244, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 244, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust].velocity *= 0.5f;
				Main.dust[dust2].velocity *= 0.5f;
				Main.dust[dust2].scale = 1.2f;
				Main.dust[dust].scale = 1.2f;
			}
			return true;
		}

	}
}
