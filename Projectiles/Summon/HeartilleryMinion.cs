using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Projectiles.Hostile;
using Terraria;
using Terraria.Audio;
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
            Main.projFrames[Projectile.type] = 9;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 28;
			Projectile.timeLeft = Projectile.SentryLifeTime;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.sentry = true;
			Projectile.ignoreWater = true;
			Projectile.sentry = true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) => false;

        public override void AI()
		{
            Projectile.scale = 1f;
			//CONFIG INFO
			int range = 22;   //How many tiles away the projectile targets NPCs
            //TARGET NEAREST NPC WITHIN RANGE
            float lowestDist = float.MaxValue;
			NPC target = new NPC();
			if (Projectile.OwnerMinionAttackTargetNPC != null && Projectile.Distance(Projectile.OwnerMinionAttackTargetNPC.Center) / 16 < range)
				target = Projectile.OwnerMinionAttackTargetNPC;

			else {
				for (int i = 0; i < 200; ++i) {
					NPC npc = Main.npc[i];
					//if npc is a valid target (active, not friendly, and not a critter)
					if (npc.active && npc.CanBeChasedBy(Projectile) && !npc.friendly) {
						//if npc is within 50 blocks
						float dist = Projectile.Distance(npc.Center);
						if (dist / 16 < range) {
							//if npc is closer than closest found npc
							if (dist < lowestDist) {
								lowestDist = dist;

								//target this npc
								target = npc;
								Projectile.netUpdate = true;
							}
						}
					}
				}
			}


			Projectile.frameCounter++;
			if (target.CanBeChasedBy(this))
			{

				float num395 = Main.mouseTextColor / 200f - 0.35f;
				num395 *= 0.3f;
				Projectile.scale = num395 + 0.85f;
				if (Projectile.frameCounter >= 6f)
				{
					Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
					Projectile.frameCounter = 0;
					if (Projectile.frame >= 8)
					{
						Projectile.frame = 0;

						Vector2 vel = ArcVelocityHelper.GetArcVel(Projectile.Center, target.Center, .4325f, 100, heightabovetarget: 20);
						for (int i = 0; i < 25; i++)
						{
							Dust dust = Dust.NewDustDirect(Projectile.Center - Vector2.UnitY * 5, Projectile.width, Projectile.height, ModContent.DustType<Dusts.Blood>(), 0f, -2f, 0, default, .85f);
							dust.velocity = vel.RotatedByRandom(MathHelper.Pi / 14) * Main.rand.NextFloat(0.1f, 0.6f);
						}

						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vel, ModContent.ProjectileType<HeartilleryMinionClump>(), Projectile.damage, 0, Main.myPlayer);
							int numproj = Main.rand.Next(1, 4);
							for (int i = 0; i < numproj; i++)
								Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, vel.RotatedByRandom(MathHelper.Pi / 8) * Main.rand.NextFloat(0.9f, 1.1f), ModContent.ProjectileType<HeartilleryMinionClump>(), Projectile.damage, 0, Main.myPlayer).netUpdate = true;
						}
						Projectile.netUpdate = true;
						SoundEngine.PlaySound(SoundID.Item95, Projectile.Center);  //make bow shooty sound
					}
				}
			}

			else if (Projectile.frameCounter >= 10f)
			{
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
				Projectile.frameCounter = 0;
				if (Projectile.frame >= 3)
					Projectile.frame = 0;
			}

            for (int index1 = 0; index1 < 3; ++index1)
            {
                float num1 = Projectile.velocity.X * 0.2f * index1;
                float num2 = -(Projectile.velocity.Y * 0.2f) * index1;
                int index2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.width, Projectile.height, DustID.Blood, 0.0f, 0.0f, 100, new Color(), 1.3f);
                Main.dust[index2].noGravity = false;
                Main.dust[index2].velocity.X *= 0.0f;
                Main.dust[index2].velocity.Y *= 0.5f;
                Main.dust[index2].scale *= 0.7f;
                Main.dust[index2].alpha = 100;
                Main.dust[index2].position.X -= num1;
                Main.dust[index2].position.Y -= num2;
            }
        }
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false;
			return true;
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCDeath22, Projectile.Center);
			for (int i = 0; i < 10; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, -2f, 0, default, .85f);
				Main.dust[num].noGravity = false;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				if (Main.dust[num].position != Projectile.Center) {
					Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 1f;
				}
			}
		}
	}
}