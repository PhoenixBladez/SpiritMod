using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class BirdSpiritPortal : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Yaksha Portal");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.minion = true;
			projectile.width = 40;
			projectile.height = 40;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.timeLeft = 10;
			projectile.minionSlots = .5f;
		}

		public override bool PreAI()
		{
			projectile.tileCollide = false;
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 244, 0f, 0f);
			Main.dust[dust].scale = 1.5f;
			Main.dust[dust].noGravity = true;
			return true;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 244, Main.rand.Next(-3, 3), Main.rand.Next(-3, 3));
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale = 2f;
			//CONFIG INFO
			int range = 30;   //How many tiles away the projectile targets NPCs
			float shootVelocity = 18f; //magnitude of the shoot vector (speed of arrows shot)
			int shootSpeed = 80;

			//TARGET NEAREST NPC WITHIN RANGE
			float lowestDist = float.MaxValue;
			for (int i = 0; i < 200; ++i)
			{
				NPC npc = Main.npc[i];
				//if npc is a valid target (active, not friendly, and not a critter)
				if (npc.active && npc.CanBeChasedBy(projectile))
				{
					//if npc is within 50 blocks
					float dist = projectile.Distance(npc.Center);
					if (dist / 16 < range)
					{
						//if npc is closer than closest found npc
						if (dist < lowestDist)
						{
							lowestDist = dist;

							//target this npc
							projectile.ai[1] = npc.whoAmI;
						}
					}
				}
			}

			NPC target = (Main.npc[(int)projectile.ai[1]] ?? new NPC()); //our target
																		 //firing
			projectile.ai[0]++;
			if (projectile.ai[0] % shootSpeed == 4 && target.active && projectile.Distance(target.Center) / 16 < range)
			{
				Vector2 ShootArea = new Vector2(projectile.Center.X, projectile.Center.Y - 25);
				Vector2 direction = target.Center - ShootArea;
				direction.Normalize();
				direction.X *= shootVelocity;
				direction.Y *= shootVelocity;
				int proj2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 25, direction.X, direction.Y, mod.ProjectileType("DietySoul2"), 30, 0, Main.myPlayer);
			}
		}

		public override void Kill(int timeLeft)
		{
			if (Main.rand.Next(6) == 1)
			{
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, Main.rand.Next(-2, 4), -5, mod.ProjectileType("DeitySoul1"), 40, 0.4f, Main.myPlayer);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, Main.rand.Next(-2, 4), -5, mod.ProjectileType("DeitySoul1"), 40, 0.4f, Main.myPlayer);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, Main.rand.Next(-2, 4), -5, mod.ProjectileType("DeitySoul1"), 40, 0.4f, Main.myPlayer);
			}
		}

	}
}
