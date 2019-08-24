using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.YoYoOverload.Projectiles
{
	public class PlagueT : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[base.projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(190);
			projectile.ranged = false;
			projectile.magic = false;
			projectile.melee = true;
			projectile.damage = 16;
			projectile.extraUpdates = 0;
			aiType = 190;
			projectile.timeLeft = 240;
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(2) == 0)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 5, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 0.9f;

			}
			return true;
		}
	}
}
