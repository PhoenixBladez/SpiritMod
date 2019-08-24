using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class AstralFlareRanged : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Flare");
		}

		public override void SetDefaults()
		{
			projectile.width = 2;
			projectile.height = 2;
			aiType = ProjectileID.Flames;
			projectile.alpha = 255;
			projectile.timeLeft = 40;
			projectile.penetrate = 3;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.extraUpdates = 20;
		}

		public override void AI()
		{
			projectile.rotation += 0.1f;
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 206, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 1.5f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (int j = 0; j < 20; j++)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 206, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 1.5f;
			}
		}

	}
}
