using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class SnowFlake : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Snow Crystal");

		public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 34;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 2; i++)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueCrystalShard);

			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 6, -2, ProjectileID.NorthPoleSnowflake, Projectile.damage, Projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, -6, -2, ProjectileID.NorthPoleSnowflake, Projectile.damage, Projectile.knockBack, Main.myPlayer);
		}

		public override void AI()
		{
			Projectile.velocity *= 0.95f;
			Projectile.rotation += 0.3f;

			for (int i = 1; i <= 3; i++) {
				if (Main.rand.NextBool(4))
					Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueCrystalShard);
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(8))
				target.AddBuff(BuffID.Frostburn, 300, true);
		}
	}
}