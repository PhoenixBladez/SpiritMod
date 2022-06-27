
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	class AquaBall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aqua Ball");
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.minion = true;
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 500;
			Projectile.height = 8;
			Projectile.width = 8;
			Projectile.alpha = 255;
			AIType = ProjectileID.Bullet;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			Vector2 targetPos = Projectile.Center;
			float targetDist = 900f;
			bool targetAcquired = false;

			//loop through first 200 NPCs in Main.npc
			//this loop finds the closest valid target NPC within the range of targetDist pixels
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i].CanBeChasedBy(Projectile) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[i].Center, 1, 1))
				{
					float dist = Projectile.Distance(Main.npc[i].Center);
					if (dist < targetDist)
					{
						targetDist = dist;
						targetPos = Main.npc[i].Center;
						targetAcquired = true;
					}
				}
			}

			//projectile.velocity = projectile.velocity.RotatedBy(Math.PI / 40);

			for (int i = 0; i < 10; i++)
			{
				float x = Projectile.Center.X - Projectile.velocity.X / 10f * i;
				float y = Projectile.Center.Y - Projectile.velocity.Y / 10f * i;
				int num = Dust.NewDust(new Vector2(x, y), 2, 2, DustID.DungeonWater);
				Main.dust[num].velocity = Vector2.Zero;
				Main.dust[num].noGravity = true;
			}

			//change trajectory to home in on target
			if (targetAcquired)
				Projectile.velocity = Projectile.DirectionTo(targetPos) * 4;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 10);

			for (int num623 = 0; num623 < 35; num623++)
			{
				int dust = Dust.NewDust(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.DungeonWater, 0, 0);
				Main.dust[dust].noGravity = true;
			}
		}
	}
}