using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Essences
{
	class EssenceOfNebula : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Essence of Nebula");

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 15;
			Projectile.timeLeft = 500;
			Projectile.height = 6;
			Projectile.width = 6;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 1;

			AIType = ProjectileID.Bullet;
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
				int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PinkFlame);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Vector2.Zero;
				Main.dust[dust].scale = 0.9f;
			}
		}
	}
}
