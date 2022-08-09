using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	class FrostBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Bolt");
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 360;
			Projectile.height = 28;
			Projectile.width = 12;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			Vector2 targetPos = Projectile.Center;
			float targetDist = 350f;
			bool targetAcquired = false;

			//loop through first 200 NPCs in Main.npc
			//this loop finds the closest valid target NPC within the range of targetDist pixels
			for (int i = 0; i < 200; i++) {
				if (Main.npc[i].CanBeChasedBy(Projectile) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[i].Center, 1, 1)) {
					float dist = Projectile.Distance(Main.npc[i].Center);
					if (dist < targetDist) {
						targetDist = dist;
						targetPos = Main.npc[i].Center;
						targetAcquired = true;
					}
				}
			}

			//change trajectory to home in on target
			if (targetAcquired) {
				float homingSpeedFactor = 6f;
				Vector2 homingVect = targetPos - Projectile.Center;
				float dist = Projectile.Distance(targetPos);
				dist = homingSpeedFactor / dist;
				homingVect *= dist;

				Projectile.velocity = (Projectile.velocity * 20 + homingVect) / 21f;
			}

			//Spawn the dust
			if (Main.rand.Next(11) == 0)
				Dust.NewDust(Projectile.position + Projectile.velocity,
					Projectile.width, Projectile.height,
					DustID.Flare_Blue, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);

			Projectile.rotation = Projectile.velocity.ToRotation() + (float)(Math.PI / 2);
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Flare_Blue);
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(4))
				target.AddBuff(BuffID.Frostburn, 200, true);
		}

	}
}
