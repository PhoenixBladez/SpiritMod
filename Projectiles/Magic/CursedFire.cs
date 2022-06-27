using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class CursedFire : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Fire");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 600;
		}

		public override bool PreAI()
		{
			for (int i = 0; i < 2; ++i)
			{
				int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
				dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.CursedTorch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}
			return true;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(Projectile.position.X, Projectile.position.Y, 30f, 0f, ModContent.ProjectileType<CursedFlames>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
			Projectile.NewProjectile(Projectile.position.X, Projectile.position.Y, -30f, 0f, ModContent.ProjectileType<CursedFlames>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
			Projectile.NewProjectile(Projectile.position.X, Projectile.position.Y, 0f, -30f, ModContent.ProjectileType<CursedFlames>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
			Projectile.NewProjectile(Projectile.position.X, Projectile.position.Y, 10f, 30f, ModContent.ProjectileType<CursedFlames>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
			Projectile.NewProjectile(Projectile.position.X, Projectile.position.Y, -10f, 30f, ModContent.ProjectileType<CursedFlames>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);

			Projectile.NewProjectile(Projectile.position.X - 100, Projectile.position.Y - 100, 0f, 30f, ModContent.ProjectileType<NightSpit>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
			Projectile.NewProjectile(Projectile.position.X + 100, Projectile.position.Y - 100, 0f, 30f, ModContent.ProjectileType<NightSpit>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.CursedInferno, 300, true);
		}

	}
}