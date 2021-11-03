using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class IchorWrath : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Ichor Wrath");

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.penetrate = 3;
			projectile.timeLeft = 500;
			projectile.height = 12;
			projectile.width = 12;
			projectile.alpha = 255;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}

		int target = -1;

		public override void AI()
		{
			//loop through first 200 NPCs in Main.npc
			//this loop finds the closest valid target NPC within the range of targetDist pixels
			if (target == -1)
			{
				float targetDist = 450f;

				for (int i = 0; i < Main.maxNPCs; i++)
				{
					if (Main.npc[i].CanBeChasedBy(projectile) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[i].Center, 1, 1))
					{
						float dist = projectile.Distance(Main.npc[i].Center);
						if (dist < targetDist)
						{
							targetDist = dist;
							target = i;
						}
					}
				}
			}
			else //change trajectory to home in on target
			{
				if (!Main.npc[target].active)
				{
					target = -1;
					return;
				}

				float homingSpeedFactor = 6f;
				Vector2 homingVect = Main.npc[target].Center - projectile.Center;
				float dist = projectile.Distance(Main.npc[target].Center);
				dist = homingSpeedFactor / dist;
				homingVect *= dist;

				projectile.velocity = (projectile.velocity * 20 + homingVect) / 21f;
			}

			for (int i = 0; i < 2; ++i)
			{
				var offset = new Vector2(Main.rand.Next(projectile.width), Main.rand.Next(projectile.height));
				var dust = Dust.NewDustPerfect(projectile.position + projectile.velocity + offset, DustID.Blood, Vector2.Zero, 0, default, 0.9f);
				dust.noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ProjectileID.VampireHeal, 0, 0f, projectile.owner, projectile.owner, 5);
	}
}