using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class StarSun1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orion's Star");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.minion = true;
			projectile.width = 20;
			projectile.timeLeft = 150;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.height = 20;
			projectile.aiStyle = -1;
			projectile.alpha = 255;
			projectile.minionSlots = 1;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 226, Main.rand.Next(-3, 3), Main.rand.Next(-3, 3));
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale = 1.3f;
			int dust1 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 187, Main.rand.Next(-3, 3), Main.rand.Next(-3, 3));
			Main.dust[dust1].noGravity = true;
			Main.dust[dust1].scale = 1.3f;
			//CONFIG INFO
			int range = 59;   //How many tiles away the projectile targets NPCs
			float shootVelocity = 18f; //magnitude of the shoot vector (speed of arrows shot)
			int shootSpeed = 60;

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
			if (projectile.ai[0] % shootSpeed == 10 && target.active && projectile.Distance(target.Center) / 16 < range)
			{

				for (int I = 0; I < 2; I++)
				{
					Vector2 ShootArea = new Vector2(projectile.Center.X, projectile.Center.Y - 25);
					Vector2 direction = target.Center - ShootArea;
					direction.Normalize();
					direction.X *= shootVelocity;
					direction.Y *= shootVelocity;
					int proj2 = Projectile.NewProjectile(projectile.Center.X + (Main.rand.Next(3, 4)), projectile.Center.Y + (Main.rand.Next(3, 4)), direction.X, direction.Y, mod.ProjectileType("StarBeam"), projectile.damage, 0, Main.myPlayer);
				}
			}
		}

	}
}
