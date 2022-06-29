using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class TerraProj : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Elemental Wrath");

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 540;
		}

		public override bool PreAI()
		{
			Projectile.tileCollide = true;
			int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f);
			Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f);
			Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f);
			Main.dust[dust].scale = 1.5f;
			Main.dust[dust].noGravity = true;
			return true;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 30f, 0f, ModContent.ProjectileType<TerraProj2>(), Projectile.damage / 2, 0f, Projectile.owner, 0f, 0f);
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, -30f, 0f, ModContent.ProjectileType<TerraProj2>(), Projectile.damage / 2, 0f, Projectile.owner, 0f, 0f);
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 0f, -30f, ModContent.ProjectileType<TerraProj2>(), Projectile.damage / 2, 0f, Projectile.owner, 0f, 0f);
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 10f, 30f, ModContent.ProjectileType<TerraProj2>(), Projectile.damage / 2, 0f, Projectile.owner, 0f, 0f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.Ichor, 300, true);

			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.Frostburn, 300, true);

			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.CursedInferno, 300, true);
		}
	}
}

