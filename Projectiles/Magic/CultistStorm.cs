using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class CultistStorm : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Storm");
			Main.projFrames[projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.tileCollide = false;
			projectile.width = 60;
			projectile.timeLeft = 3000;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.height = 60;
			projectile.aiStyle = -1;
		}

		public override void AI()
		{
			Lighting.AddLight(projectile.position, 0.0f, 0.3f, 0.8f);
			projectile.spriteDirection = projectile.direction;
			projectile.frameCounter++;
			if (projectile.frameCounter >= 6)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
				if (projectile.frame >= 4)
					projectile.frame = 0;
			}

			projectile.ai[1] += 1f;
			if (projectile.ai[1] >= 7200f)
			{
				projectile.alpha += 5;
				if (projectile.alpha > 255)
				{
					projectile.alpha = 255;
					projectile.Kill();
				}
			}

			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] >= 10f)
			{
				projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = projectile.type;
				for (int num420 = 0; num420 < 1000; num420++)
				{
					if (Main.projectile[num420].active && Main.projectile[num420].owner == projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f)
					{
						num416++;
						if (Main.projectile[num420].ai[1] > num418)
						{
							num417 = num420;
							num418 = Main.projectile[num420].ai[1];
						}
					}
				}
				if (num416 > 2)
				{
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}

			//CONFIG INFO
			int range = 90;   //How many tiles away the projectile targets NPCs
			float shootVelocity = 12f; //magnitude of the shoot vector (speed of arrows shot)
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
			if (projectile.ai[0] % shootSpeed == 3 && target.active && projectile.Distance(target.Center) / 10 < range)
			{
				Vector2 ShootArea = new Vector2(projectile.Center.X, projectile.Center.Y - 25);
				Vector2 direction = target.Center - ShootArea;
				direction.Normalize();
				direction.X *= shootVelocity;
				direction.Y *= shootVelocity;
				int proj2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 25, direction.X, direction.Y, 580, projectile.damage, 0, Main.myPlayer);
				Main.projectile[proj2].friendly = true;
				Main.projectile[proj2].hostile = false;
				Main.projectile[proj2].penetrate = 2;
			}
		}


	}
}
