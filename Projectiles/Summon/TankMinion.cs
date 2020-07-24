using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class TankMinion : ModProjectile
	{
		string phase = "";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thermal Tank");
			Main.projFrames[projectile.type] = 8;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 54;
			projectile.height = 30;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.sentry = true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.X != projectile.velocity.X && phase == "moving")
				projectile.velocity.Y = -6.5f;

			return false;
		}

		public override void AI()
		{
			if (projectile.velocity.Y < 5)
				projectile.velocity.Y += 0.3f;

			if (projectile.velocity.Y > 5)
				projectile.velocity.Y = 5;

			#region moving
			if (phase == "moving") {
				projectile.frameCounter++;
				if (projectile.frameCounter >= 5) {
					projectile.frame++;
					projectile.frameCounter = 0;
					if (projectile.frame >= 5) {
						projectile.frame = 0;
					}
				}
			}
			#endregion

			#region shooting
			if (phase == "shooting") {
				projectile.frameCounter++;
				if (projectile.frameCounter >= 8) {
					projectile.frame++;
					projectile.frameCounter = 0;
					if (projectile.frame >= 8) {
						projectile.frame = 5;
					}
				}
			}
			#endregion

			int range = 100;   //How many tiles away the projectile targets NPCs
			float shootVelocity = 18f;

			bool hasTarget = false;
			bool hasTargetInCone = false;
			float maxRange = 1600f;
			NPC target = projectile.OwnerMinionAttackTargetNPC;
			if (target != null && target.CanBeChasedBy(projectile)) {
				float dist = projectile.Distance(target.Center);
				if (dist < maxRange && Collision.CanHit(projectile.position, projectile.width, projectile.height, target.position, target.width, target.height)) {
					maxRange = dist;
					hasTarget = true;
				}
			}
			if (!hasTarget) {
				for (int i = 0; i < 200; i++) {
					if (Main.npc[i].CanBeChasedBy(this)) {
						float dist = projectile.Distance(Main.npc[i].Center);
						var shootOffset = projectile.Center + new Vector2(10 * projectile.direction, -8);
						var angleToTarget = Main.npc[i].Center - shootOffset;
						angleToTarget.Normalize();
						var aimCone1 = MathHelper.PiOver4;
						var aimCone2 = MathHelper.PiOver4 * 3;
						var aimAngle = angleToTarget.ToRotation();
						if (aimAngle > MathHelper.PiOver4 * 3) aimAngle -= MathHelper.Pi;
						bool inCone = Math.Abs(aimAngle) < aimCone1 || Math.Abs(aimAngle) > aimCone2;
						if (inCone) hasTargetInCone = true;
						if ((inCone || !hasTargetInCone) && dist < maxRange && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height)) {
							maxRange = dist;
							target = Main.npc[i];
							hasTarget = true;
						}
					}
				}
			}

			if (hasTarget) {
				var shootOffset = projectile.Center + new Vector2(10 * projectile.direction, -8);
				var angleToTarget = target.Center - shootOffset;
				angleToTarget.Normalize();
				var aimCone1 = MathHelper.PiOver4;
				var aimCone2 = MathHelper.PiOver4 * 3;
				var aimAngle = angleToTarget.ToRotation();
				bool inCone = Math.Abs(aimAngle) < aimCone1 || Math.Abs(aimAngle) > aimCone2;
				bool closeEnough = projectile.Distance(target.Center) / 16 < (range / 2);
				if (inCone && closeEnough) {
					phase = "shooting";
					if (projectile.position.X - target.position.X > 0)
						projectile.velocity.X = -0.02f;
					else
						projectile.velocity.X = 0.02f;

					if (projectile.frame == 6 && projectile.frameCounter == 4) {
						Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
						projectile.frameCounter = 0;
						projectile.frame = 7;

						Projectile.NewProjectile(shootOffset, angleToTarget * shootVelocity, ProjectileID.RocketI, projectile.damage, 0, Main.myPlayer);
					}
				}
				else {
					phase = "moving";

					if (projectile.position.X - target.position.X > 0) {
						projectile.velocity.X = -2;
					}
					else {
						projectile.velocity.X = 2;
					}

					// if our target is outside of the aiming cone, move backwards
					if (!inCone && closeEnough) {
						projectile.velocity.X *= -1;
						projectile.direction *= -1;
					}
				}
			}
			else {
				phase = "idle";
				projectile.velocity.X = 0;
				projectile.frame = 1;
			}
			projectile.spriteDirection = projectile.direction;
		}
	}
}