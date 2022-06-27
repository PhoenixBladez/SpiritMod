using Microsoft.Xna.Framework;
using SpiritMod.Mechanics.Trails;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class SteamBeam : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Steam Beam");
		}

		public override void SetDefaults()
		{
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = 4;
			Projectile.timeLeft = 150;
			Projectile.height = 6;
			Projectile.width = 6;

			Projectile.alpha = 255;
			AIType = ProjectileID.Bullet;
		}
		public override void AI()
		{
			if (Projectile.velocity.Length() < 28)
				Projectile.velocity *= 1.02f;
			else
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 28;
		}

		public void DoTrailCreation(TrailManager tManager) => tManager.CreateTrail(Projectile, new StandardColorTrail(new Color(82, 232, 255)), new RoundCap(), new DefaultTrailPosition(), 10f, 450f);

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0) {
				Projectile.Kill();
			}
			else {
				Projectile.ai[0] += 0.1f;
				if (Projectile.velocity.X != oldVelocity.X) {
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y) {
					Projectile.velocity.Y = -oldVelocity.Y;
				}
				Projectile.velocity *= 0.75f;
			}
			return false;
		}

	}
}
