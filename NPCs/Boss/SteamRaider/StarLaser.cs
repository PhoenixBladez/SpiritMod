using Microsoft.Xna.Framework;
using SpiritMod.Mechanics.Trails;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
	public class StarLaser : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Laser");
		}
		Vector2 startingpoint;

		public override void SetDefaults()
		{
			Projectile.hostile = true;
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.penetrate = 8;
			Projectile.alpha = 255;
			Projectile.timeLeft = 90;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 5;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Collision.CheckAABBvLineCollision(targetHitbox.Center.ToVector2() - targetHitbox.Size() / 2,
																													 targetHitbox.Size(),
																													 startingpoint,
																													 Projectile.Center);

		public void DoTrailCreation(TrailManager tManager) => tManager.CreateTrail(Projectile, new StandardColorTrail(new Color(66, 239, 245)), new RoundCap(), new DefaultTrailPosition(), 10f, 1950f);

		public override bool PreAI()
		{
			if(Projectile.ai[0] == 0) {
				startingpoint = Projectile.Center;
				Projectile.ai[0]++;
			}
			return true;
		}

		public override void AI()
		{
			Dust dust = Dust.NewDustPerfect(Projectile.Center, 226);
			dust.velocity = Vector2.Zero;
			dust.noLight = true;
			dust.noGravity = true;
		}
	}
}