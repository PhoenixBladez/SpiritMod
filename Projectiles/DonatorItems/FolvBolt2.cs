
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	class FolvBolt2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Missile");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.magic = true;
			projectile.penetrate = 2;
			projectile.timeLeft = 300;
			projectile.height = 8;
			projectile.width = 8;
			projectile.alpha = 255;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}
		int timer;
		public override void AI()
		{

			Vector2 targetPos = projectile.Center;
			float targetDist = 450f;
			bool targetAcquired = false;

			//loop through first 200 NPCs in Main.npc
			//this loop finds the closest valid target NPC within the range of targetDist pixels
			for(int i = 0; i < 200; i++) {
				if(Main.npc[i].CanBeChasedBy(projectile) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[i].Center, 1, 1)) {
					float dist = projectile.Distance(Main.npc[i].Center);
					if(dist < targetDist) {
						targetDist = dist;
						targetPos = Main.npc[i].Center;
						targetAcquired = true;
					}
				}
			}

			for(int i = 0; i < 3; i++) {
				Dust dust;
				// You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
				Vector2 position = projectile.Center;
				dust = Main.dust[Terraria.Dust.NewDust(position, 0, 0, 272, 0f, 0f, 0, new Color(255, 255, 255), 0.4947368f)];
				dust.noLight = true;
				dust.velocity = Vector2.Zero;
			}
			timer++;
			if(timer >= 60) {
				for(float num2 = 0.0f; (double)num2 < 10; ++num2) {
					int dustIndex = Dust.NewDust(projectile.Center, 2, 2, 272, 0f, 0f, 0, default(Color), .5f);
					Main.dust[dustIndex].noGravity = true;
					Main.dust[dustIndex].velocity = Vector2.Normalize(projectile.Center.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))) * 1.6f;
				}
				timer = 0;
			}

			//change trajectory to home in on target
			if(targetAcquired) {
				float homingSpeedFactor = 6f;
				Vector2 homingVect = targetPos - projectile.Center;
				float dist = projectile.Distance(targetPos);
				dist = homingSpeedFactor / dist;
				homingVect *= dist;

				projectile.velocity = (projectile.velocity * 20 + homingVect) / 21f;
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 3);
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width = 5;
			projectile.height = 5;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
			for(float num2 = 0.0f; (double)num2 < 10; ++num2) {
				int dustIndex = Dust.NewDust(projectile.position, 2, 2, 272, 0f, 0f, 0, default(Color), 1f);
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].velocity = Vector2.Normalize(projectile.position.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))) * 1.6f;
			}

		}
	}
}
