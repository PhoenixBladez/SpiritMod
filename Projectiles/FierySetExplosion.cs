using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class FierySetExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Wrath");
		}

		public override void SetDefaults()
		{
			Projectile.width = 2;
			Projectile.timeLeft = 2;
			Projectile.height = 2;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.friendly = true;
		}

		public override void AI()
		{
			Projectile.Kill();
		}

		public override void Kill(int timeLeft)
		{
			int n = 8;
			int deviation = Main.rand.Next(0, 300);
			for (int i = 0; i < n; i++) {
				float rotation = MathHelper.ToRadians(270 / n * i + deviation);
				Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X + 1, Projectile.velocity.Y).RotatedBy(rotation);
				perturbedSpeed.Normalize();
				perturbedSpeed.X *= 2.5f;
				perturbedSpeed.Y *= 2.5f;
				Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<Blaze>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
		}
	}
}
