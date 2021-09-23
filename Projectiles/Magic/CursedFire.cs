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
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 40;
			projectile.height = 40;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.alpha = 255;
			projectile.timeLeft = 600;
		}

		public override bool PreAI()
		{
			for (int i = 0; i < 2; ++i)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.ShadowbeamStaff, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
				dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.CursedTorch, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}
			return true;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 30f, 0f, ModContent.ProjectileType<CursedFlames>(), projectile.damage, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -30f, 0f, ModContent.ProjectileType<CursedFlames>(), projectile.damage, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, -30f, ModContent.ProjectileType<CursedFlames>(), projectile.damage, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 10f, 30f, ModContent.ProjectileType<CursedFlames>(), projectile.damage, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -10f, 30f, ModContent.ProjectileType<CursedFlames>(), projectile.damage, 0f, projectile.owner, 0f, 0f);

			Projectile.NewProjectile(projectile.position.X - 100, projectile.position.Y - 100, 0f, 30f, ModContent.ProjectileType<NightSpit>(), projectile.damage, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X + 100, projectile.position.Y - 100, 0f, 30f, ModContent.ProjectileType<NightSpit>(), projectile.damage, 0f, projectile.owner, 0f, 0f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.CursedInferno, 300, true);
		}

	}
}