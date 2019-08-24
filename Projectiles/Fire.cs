using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class Fire : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Malevolent Wrath");
		}

		public override void SetDefaults()
		{
			projectile.width = 80;
			projectile.timeLeft = 20;
			projectile.height = 80;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.alpha = 255;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = true;
		}

		public override void AI()
		{
			Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 300);
		}
	}
}
