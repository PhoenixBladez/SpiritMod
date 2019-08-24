using System;

using Microsoft.Xna.Framework;

using Terraria;
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
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
		}

		public override bool PreAI()
		{
			projectile.velocity.Y += GRAVITY;
			ProjectileExtras.LookAlongVelocity(this);
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
			{
				int dmg = projectile.damage / 2;
				if (dmg < 1)
					dmg = 1;

				int[] targets = new int[Main.maxNPCs];
				int obstructed = 0;
				int visible = 0;
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					if (Main.npc[i].CanBeChasedBy(projectile, false))
					{
						float orthDist = Math.Abs(Main.npc[i].position.X + (float)(Main.npc[i].width / 2) - projectile.position.X + (float)(projectile.width / 2)) + Math.Abs(Main.npc[i].position.Y + (float)(Main.npc[i].height / 2) - projectile.position.Y + (float)(projectile.height / 2));
						if (orthDist < 800f)
						{
							if (Collision.CanHit(projectile.position, 1, 1, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height) && orthDist > 50f)
							{
								targets[visible] = i;
								visible++;
							}
							else if (visible == 0)
							{
								targets[obstructed] = i;
								obstructed++;
							}
						}
					}
				}
				if (obstructed == 0 && visible == 0)
				{
					return;
				}

				int npc;
				if (visible > 0)
				{
					npc = targets[Main.rand.Next(visible)];
				}
				else
				{
					npc = targets[Main.rand.Next(obstructed)];
				}
				float velocity = 4f;
				float xVel = (float)Main.rand.Next(-100, 101);
				float yVel = (float)Main.rand.Next(-100, 101);
				velocity /= (float)Math.Sqrt((double)(xVel * xVel + yVel * yVel));
				xVel *= velocity;
				yVel *= velocity;
				Projectile.NewProjectile(target.position.X, target.position.Y, xVel, yVel, 356, dmg, 0f, projectile.owner, (float)npc, 0f);
				projectile.Kill();
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 206);
			}
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
		}

	}
}
