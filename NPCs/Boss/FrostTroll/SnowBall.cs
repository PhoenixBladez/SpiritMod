using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.FrostTroll
{
	public class SnowBall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snowball");
		}

		public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 34;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 2; i++)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Snow);

			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 6, -2, ProjectileID.FrostBlastHostile, Projectile.damage, Projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, -6, -2, ProjectileID.FrostBlastHostile, Projectile.damage, Projectile.knockBack, Main.myPlayer);
		}

		public override void AI()
		{
			Projectile.rotation += 0.3f;

			for (int i = 1; i <= 3; i++) {
				if (Main.rand.NextBool(4))
					Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Snow);
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.NextBool(4))
				target.AddBuff(BuffID.Frostburn, 180, true);
		}
	}
}