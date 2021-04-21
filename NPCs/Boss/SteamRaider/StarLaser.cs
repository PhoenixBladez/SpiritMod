using Microsoft.Xna.Framework;
using SpiritMod.Utilities;
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
			projectile.hostile = true;
			projectile.width = 2;
			projectile.height = 2;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = 8;
			projectile.alpha = 255;
			projectile.timeLeft = 90;
			projectile.tileCollide = false;
			projectile.extraUpdates = 5;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Collision.CheckAABBvLineCollision(targetHitbox.Center.ToVector2() - targetHitbox.Size() / 2,
																													 targetHitbox.Size(),
																													 startingpoint,
																													 projectile.Center);

		public void DoTrailCreation(TrailManager tManager) => tManager.CreateTrail(projectile, new StandardColorTrail(new Color(66, 239, 245)), new RoundCap(), new DefaultTrailPosition(), 10f, 1950f);

		public override bool PreAI()
		{
			if(projectile.ai[0] == 0) {
				startingpoint = projectile.Center;
				projectile.ai[0]++;
			}
				
			Dust dust = Dust.NewDustPerfect(projectile.Center, 226);
			dust.velocity = Vector2.Zero;
			dust.noLight = true;
			dust.noGravity = true;
			return true;
		}

	}
}