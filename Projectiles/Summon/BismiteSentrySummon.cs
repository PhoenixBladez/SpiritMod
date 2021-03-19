using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class BismiteSentrySummon : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Crystal");
		}

		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 46;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.penetrate = -1;
            projectile.tileCollide = true;
			projectile.sentry = true;
			projectile.ignoreWater = true;
			projectile.sentry = true;
		}
		float alphaCounter = 0;
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}
        public override void AI()
		{

			if (projectile.alpha > 0 && projectile.alpha < 239)
            {
                projectile.alpha--;
            }
            float num395 = Main.mouseTextColor / 200f - 0.35f;
            num395 *= 0.3f;
            projectile.scale = num395 + 0.65f;
            //CONFIG INFO
            int range = 18;   //How many tiles away the projectile targets NPCs
			float shootVelocity = 6f; //magnitude of the shoot vector (speed of arrows shot)
			int shootSpeed = 20;

			//TARGET NEAREST NPC WITHIN RANGE
			float lowestDist = float.MaxValue;
			for (int i = 0; i < 200; ++i) {
				NPC npc = Main.npc[i];
				//if npc is a valid target (active, not friendly, and not a critter)
				if (npc.active && npc.CanBeChasedBy(projectile) && !npc.friendly) {
					//if npc is within 50 blocks
					float dist = projectile.Distance(npc.Center);
					if (dist / 16 < range) {
						//if npc is closer than closest found npc
						if (dist < lowestDist) {
							lowestDist = dist;

							//target this npc
							projectile.ai[1] = npc.whoAmI;
							projectile.netUpdate = true;
						}
					}
				}
			}
            if (projectile.alpha == 240)
            {
                specialAttack();
                projectile.alpha--;
            }
            NPC target = (Main.npc[(int)projectile.ai[1]] ?? new NPC()); //our target
																		 //firing
			projectile.ai[0]++;
			if (projectile.ai[0] >= 60 && target.active && !target.friendly && projectile.Distance(target.Center) / 16 < range) {
				projectile.ai[0] = 0;
				Vector2 ShootArea = new Vector2(projectile.Center.X, projectile.Center.Y - 13);
				Vector2 direction = target.Center - ShootArea;
				direction.Normalize();
				direction.X *= shootVelocity;
				direction.Y *= shootVelocity;
                for (int i = 0; i < 10; i++)
                {
                    int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 167, 0f, -2f, 0, default(Color), 1.5f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    if (Main.dust[num].position != projectile.Center)
                    {
                        Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 2f;
                    }
                }
                if (Main.netMode != NetmodeID.MultiplayerClient && projectile.alpha <= 100) {

					int proj2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 13, direction.X, direction.Y, mod.ProjectileType("BismiteShot"), projectile.damage, 0, Main.myPlayer);
					Main.projectile[proj2].ranged = false;
					Main.projectile[proj2].minion = true;
				}
				
				Main.PlaySound(SoundID.DD2_CrystalCartImpact, projectile.Center);  //make bow shooty sound
			}
		}
		public void specialAttack()
        {
            Main.PlaySound(SoundID.DD2_CrystalCartImpact, projectile.Center);
            for (int i = 0; i < 10; i++)
            {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 167, 0f, -2f, 0, default(Color), 1.5f);
                Main.dust[num].noGravity = true;
                Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                if (Main.dust[num].position != projectile.Center)
                {
                    Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 3f;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
                Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y,
                    velocity.X, velocity.Y, mod.ProjectileType("BismiteShard"), projectile.damage / 2, projectile.knockBack, projectile.owner);
                Main.projectile[proj].minion = true;
                Main.projectile[proj].magic = false;
                Main.projectile[proj].velocity *= 1.5f;
                Main.projectile[proj].timeLeft = 120;
                Main.projectile[proj].hide = true;
            }
        }
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.DD2_WitherBeastHurt, (int)projectile.position.X, (int)projectile.position.Y);
			for (int i = 0; i < 10; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 167, 0f, -2f, 0, default(Color), 1.5f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				if (Main.dust[num].position != projectile.Center) {
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
		}
	}
}