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
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 500;
			Projectile.height = 12;
			Projectile.width = 12;
			Projectile.alpha = 255;
			AIType = ProjectileID.Bullet;
			Projectile.extraUpdates = 1;
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
					if (Main.npc[i].CanBeChasedBy(Projectile) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[i].Center, 1, 1))
					{
						float dist = Projectile.Distance(Main.npc[i].Center);
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
				Vector2 homingVect = Main.npc[target].Center - Projectile.Center;
				float dist = Projectile.Distance(Main.npc[target].Center);
				dist = homingSpeedFactor / dist;
				homingVect *= dist;

				Projectile.velocity = (Projectile.velocity * 20 + homingVect) / 21f;
			}

			for (int i = 0; i < 2; ++i)
			{
				var offset = new Vector2(Main.rand.Next(Projectile.width), Main.rand.Next(Projectile.height));
				var dust = Dust.NewDustPerfect(Projectile.position + Projectile.velocity + offset, DustID.Blood, Vector2.Zero, 0, default, 0.9f);
				dust.noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ProjectileID.VampireHeal, 0, 0f, Projectile.owner, Projectile.owner, 5);
	}
}