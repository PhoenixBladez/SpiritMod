using Microsoft.Xna.Framework;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Utilities;
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
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 4;
			projectile.timeLeft = 150;
			projectile.height = 6;
			projectile.width = 6;

			projectile.alpha = 255;
			aiType = ProjectileID.Bullet;
		}
		public override void AI()
		{
			if (projectile.velocity.Length() < 28)
				projectile.velocity *= 1.02f;
			else
				projectile.velocity = Vector2.Normalize(projectile.velocity) * 28;
		}

		public void DoTrailCreation(TrailManager tManager) => tManager.CreateTrail(projectile, new StandardColorTrail(new Color(82, 232, 255)), new RoundCap(), new DefaultTrailPosition(), 10f, 450f);

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.penetrate--;
			if (projectile.penetrate <= 0) {
				projectile.Kill();
			}
			else {
				projectile.ai[0] += 0.1f;
				if (projectile.velocity.X != oldVelocity.X) {
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y) {
					projectile.velocity.Y = -oldVelocity.Y;
				}
				projectile.velocity *= 0.75f;
			}
			return false;
		}

	}
}
