using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class CombustionBlaze : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Combustion Blaze");
		}

		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 360;
			projectile.alpha = 255;
			projectile.tileCollide = true;
			aiType = ProjectileID.Bullet;
		}

		public override bool PreAI()
		{
			for (int i = 0; i < 4; ++i)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale *= 1.6f;
				Main.dust[dust].noGravity = true;
			}
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(ModContent.BuffType<StackingFireBuff>(), 180);
		}

		public override void Kill(int timeLeft)
		{
			Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
		}

	}
}