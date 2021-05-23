using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Projectiles.Hostile;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Utilities;

namespace SpiritMod.Projectiles.Summon
{
	public class HeartilleryMinion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aching Heart");
            Main.projFrames[projectile.type] = 9;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 28;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.sentry = true;
			projectile.ignoreWater = true;
			projectile.sentry = true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) => false;

        public override void AI()
		{
            projectile.scale = 1f;
			//CONFIG INFO
			int range = 22;   //How many tiles away the projectile targets NPCs
			float shootVelocity = 13.5f; //magnitude of the shoot vector (speed of arrows shot)
			int shootSpeed = 20;
            //TARGET NEAREST NPC WITHIN RANGE
            float lowestDist = float.MaxValue;
			if (projectile.OwnerMinionAttackTargetNPC != null && projectile.Distance(projectile.OwnerMinionAttackTargetNPC.Center) / 16 < range)
					projectile.ai[1] = projectile.OwnerMinionAttackTargetNPC.whoAmI;

			else {
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
			}

			NPC target = (Main.npc[(int)projectile.ai[1]] ?? new NPC()); //our target

			projectile.frameCounter++;
			if (target.CanBeChasedBy(this)) {

				float num395 = Main.mouseTextColor / 200f - 0.35f;
				num395 *= 0.3f;
				projectile.scale = num395 + 0.85f;
				if (projectile.frameCounter >= 6f) {
					projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
					projectile.frameCounter = 0;
					if (projectile.frame >= 8) {
						projectile.frame = 0;

						Vector2 vel = ArcVelocityHelper.GetArcVel(projectile.Center, target.Center, .4325f, 100, heightabovetarget: 20);
						for (int i = 0; i < 25; i++) {
							Dust dust = Dust.NewDustDirect(projectile.Center - Vector2.UnitY * 5, projectile.width, projectile.height, ModContent.DustType<Dusts.Blood>(), 0f, -2f, 0, default, .85f);
							dust.velocity = vel.RotatedByRandom(MathHelper.Pi / 14) * Main.rand.NextFloat(0.1f, 0.6f);
						}

						if (Main.netMode != NetmodeID.MultiplayerClient) {
							Projectile.NewProjectile(projectile.Center, vel, mod.ProjectileType("HeartilleryMinionClump"), projectile.damage, 0, Main.myPlayer);
							int numproj = Main.rand.Next(1, 4);
							for (int i = 0; i < numproj; i++)
								Projectile.NewProjectileDirect(projectile.Center, vel.RotatedByRandom(MathHelper.Pi / 8) * Main.rand.NextFloat(0.9f, 1.1f), mod.ProjectileType("HeartilleryMinionClump"), projectile.damage, 0, Main.myPlayer).netUpdate = true;
						}
						projectile.netUpdate = true;
						Main.PlaySound(SoundID.Item, projectile.Center, 95);  //make bow shooty sound
					}
				}
			}

			else if (projectile.frameCounter >= 10f)
            {
                projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
                projectile.frameCounter = 0;
                if (projectile.frame >= 3)
                {
                    projectile.frame = 0;
                }
            }

            for (int index1 = 0; index1 < 3; ++index1)
            {
                float num1 = projectile.velocity.X * 0.2f * index1;
                float num2 = -(projectile.velocity.Y * 0.2f) * index1;
                int index2 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, 5, 0.0f, 0.0f, 100, new Color(), 1.3f);
                Main.dust[index2].noGravity = false;
                Main.dust[index2].velocity.X *= 0.0f;
                Main.dust[index2].velocity.Y *= 0.5f;
                Main.dust[index2].scale *= 0.7f;
                Main.dust[index2].alpha = 100;
                Main.dust[index2].position.X -= num1;
                Main.dust[index2].position.Y -= num2;
            }
        }
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = false;
			return true;
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 22);
			for (int i = 0; i < 10; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 5, 0f, -2f, 0, default(Color), .85f);
				Main.dust[num].noGravity = false;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				if (Main.dust[num].position != projectile.Center) {
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 1f;
				}
			}
		}
	}
}