using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	class SpectreArrow : ModProjectile
	{
		public const float GRAVITY = .05f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghast Arrow");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
		}

		public override bool PreAI()
		{
			Projectile.velocity.Y += GRAVITY;
			ProjectileExtras.LookAlongVelocity(this);
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0) {
				int dmg = Projectile.damage / 2;
				if (dmg < 1)
					dmg = 1;

				int[] targets = new int[Main.maxNPCs];
				int obstructed = 0;
				int visible = 0;
				for (int i = 0; i < Main.maxNPCs; i++) {
					if (Main.npc[i].CanBeChasedBy(Projectile, false)) {
						float orthDist = Math.Abs(Main.npc[i].position.X + (float)(Main.npc[i].width / 2) - Projectile.position.X + (float)(Projectile.width / 2)) + Math.Abs(Main.npc[i].position.Y + (float)(Main.npc[i].height / 2) - Projectile.position.Y + (float)(Projectile.height / 2));
						if (orthDist < 800f) {
							if (Collision.CanHit(Projectile.position, 1, 1, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height) && orthDist > 50f) {
								targets[visible] = i;
								visible++;
							}
							else if (visible == 0) {
								targets[obstructed] = i;
								obstructed++;
							}
						}
					}
				}
				if (obstructed == 0 && visible == 0) {
					return;
				}

				int npc;
				if (visible > 0) {
					npc = targets[Main.rand.Next(visible)];
				}
				else {
					npc = targets[Main.rand.Next(obstructed)];
				}
				float velocity = 4f;
				float xVel = (float)Main.rand.Next(-100, 101);
				float yVel = (float)Main.rand.Next(-100, 101);
				velocity /= (float)Math.Sqrt((double)(xVel * xVel + yVel * yVel));
				xVel *= velocity;
				yVel *= velocity;
				Projectile.NewProjectile(target.position.X, target.position.Y, xVel, yVel, ProjectileID.SpectreWrath, dmg, 0f, Projectile.owner, (float)npc, 0f);
				Projectile.Kill();
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.UnusedWhiteBluePurple);
			}
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
		}

	}
}
