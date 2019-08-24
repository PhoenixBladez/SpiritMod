using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class QuicksilverDroplet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quicksilver Droplet");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(326);
			projectile.hostile = false;
			projectile.width = 5;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.timeLeft = 180;
			projectile.alpha = 255;
			projectile.penetrate = 5;
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(2) == 1)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SilverCoin, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			if (Main.rand.Next(2) == 1)
			{
				player.statLife += 4;
				player.HealEffect(4);
			}
		}
	}
}
