
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Tides
{
	class MangoLaser : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mango Laser");
		}

		public override void SetDefaults()
		{
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 350;
			Projectile.height = 12;
			Projectile.width = 12;
			Projectile.alpha = 255;
			AIType = ProjectileID.Bullet;
			Projectile.extraUpdates = 1;
		}
		bool fired = false;
		public override void AI()
		{

			Vector2 targetPos = Projectile.Center;
			float targetDist = 450f;
			bool targetAcquired = false;

			float lowestDist = float.MaxValue;
			foreach (Player player in Main.player) {
				//if npc is a valid target (active, not friendly, and not a critter)
				if (player.active) {
					//if npc is within 50 blocks
					float dist = Projectile.Distance(player.Center);
					if (dist < lowestDist) {
						targetDist = dist;
						targetPos = player.Center;
						targetAcquired = true;
					}
				}
			}

			for (int i = 0; i < 6; i++) {
				Vector2 position = Projectile.Center;
				Dust dust = Main.dust[Terraria.Dust.NewDust(position, 0, 0, DustID.WitherLightning, 0f, 0f, 0, new Color(255, 255, 255), 0.3947368f)];
				dust.noLight = true;
				dust.velocity = Vector2.Zero;
			}

			//change trajectory to home in on target
			if (targetAcquired && !fired) {
				Vector2 homingVect = targetPos - Projectile.Center;
				homingVect.Normalize();
				homingVect *= 9f;
				Projectile.velocity = homingVect;
				fired = true;

			}
		}
	}
}
