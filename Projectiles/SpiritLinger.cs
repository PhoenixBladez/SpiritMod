using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class SpiritLinger : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lingering Souls");
		}

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 25;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 60;
			projectile.alpha = 255;
			projectile.thrown = true;
		}

		public override bool PreAI()
		{
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 187, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].scale *= 1.6f;
			Main.dust[dust].noGravity = true;
			int dust1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 187, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust1].scale *= 1.6f;
			Main.dust[dust1].noGravity = true;
			int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 187, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust2].scale *= 1.6f;
			Main.dust[dust2].noGravity = true;
			int dust12 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 187, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust12].scale *= 1.6f;
			Main.dust[dust12].noGravity = true;

			return true;
		}
	}
}