using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles.Magic
{
	public class SpiritBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Bolt");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 200;
		}

		public override bool PreAI()
		{
			int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.UnusedWhiteBluePurple, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale = 2f;

			return true;
		}

		public override void Kill(int timeLeft)
		{
			int n = 4;
			int deviation = Main.rand.Next(0, 60);
			for (int i = 0; i < n; i++) {
				float rotation = MathHelper.ToRadians(360 / n * i + deviation);
				Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(rotation);
				perturbedSpeed.Normalize();
				perturbedSpeed.X *= 5.5f;
				perturbedSpeed.Y *= 5.5f;
				Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<SpiritWave>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
		}

	}
}
