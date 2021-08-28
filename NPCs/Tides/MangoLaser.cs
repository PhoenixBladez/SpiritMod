
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
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.magic = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 350;
			projectile.height = 12;
			projectile.width = 12;
			projectile.alpha = 255;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}
		bool fired = false;
		public override void AI()
		{

			Vector2 targetPos = projectile.Center;
			float targetDist = 450f;
			bool targetAcquired = false;

			float lowestDist = float.MaxValue;
			foreach (Player player in Main.player) {
				//if npc is a valid target (active, not friendly, and not a critter)
				if (player.active) {
					//if npc is within 50 blocks
					float dist = projectile.Distance(player.Center);
					if (dist < lowestDist) {
						targetDist = dist;
						targetPos = player.Center;
						targetAcquired = true;
					}
				}
			}

			for (int i = 0; i < 6; i++) {
				Vector2 position = projectile.Center;
				Dust dust = Main.dust[Terraria.Dust.NewDust(position, 0, 0, DustID.WitherLightning, 0f, 0f, 0, new Color(255, 255, 255), 0.3947368f)];
				dust.noLight = true;
				dust.velocity = Vector2.Zero;
			}

			//change trajectory to home in on target
			if (targetAcquired && !fired) {
				Vector2 homingVect = targetPos - projectile.Center;
				homingVect.Normalize();
				homingVect *= 9f;
				projectile.velocity = homingVect;
				fired = true;

			}
		}
	}
}
