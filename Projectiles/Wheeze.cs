using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class Wheeze : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wheeze Gas");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 22;
			projectile.height = 22;
			projectile.aiStyle = 1;
			projectile.thrown = true;
			aiType = ProjectileID.Bullet;
			projectile.friendly = true;
			projectile.penetrate = 5;
			projectile.timeLeft = 180;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(20, 150);
		}

		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			target.AddBuff(20, 150);
		}

		public override bool PreAI()
		{
			projectile.velocity *= 0.95f;
			{
				projectile.tileCollide = true;
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 60, 0f, 0f);
				Main.dust[dust].scale = 0.8f;
				Main.dust[dust].noGravity = true;
			}
			return true;
		}
	}
}
