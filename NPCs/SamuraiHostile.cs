using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class SamuraiHostile : ModNPC
	{
		int chargeTime = 200; //how many frames between charges
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantom Samurai");
			Main.npcFrameCount[npc.type] = 9;
			NPCID.Sets.TownCritter[npc.type] = true;
			NPCID.Sets.TrailCacheLength[npc.type] = 3;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			if(NPC.downedBoss2) {
				npc.width = 30;
				npc.height = 40;
				npc.damage = 35;
				npc.noGravity = true;
				npc.defense = 8;
				npc.lifeMax = 130;
			} else if(NPC.downedBoss3) {
				npc.width = 30;
				npc.height = 40;
				npc.damage = 38;
				npc.noGravity = true;
				npc.defense = 11;
				npc.lifeMax = 180;
			} else {
				npc.width = 30;
				npc.height = 40;
				npc.damage = 33;
				npc.noGravity = true;
				npc.defense = 4;
				npc.lifeMax = 90;
			}
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 120f;
			npc.knockBackResist = .1f;
			npc.noTileCollide = true;
		}
		float frameCounter = 0;
		public override void FindFrame(int frameHeight)
		{
			frameCounter += 0.15f;
			frameCounter %= 3f;
			if(chargetimer == chargeTime) {
				frameCounter = 0;
			}
			if(charging) {
				npc.frameCounter = frameCounter + 6;
			} else if(chargetimer > chargeTime - 50) {
				npc.frameCounter = frameCounter + 3;
			} else {
				npc.frameCounter = frameCounter;
			}
			if(npc.frameCounter == 8) {
				charging = false;
			}
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			int d1 = 54;
			for(int k = 0; k < 20; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 54, new Color(0, 255, 142), .6f);
			}
			if(npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, 99);
				Gore.NewGore(npc.position, npc.velocity, 99);
				Gore.NewGore(npc.position, npc.velocity, 99);
				for(int i = 0; i < 40; i++) {
					int num = Dust.NewDust(npc.position, npc.width, npc.height, 54, 0f, -2f, 117, new Color(0, 255, 142), .6f);
					Main.dust[num].noGravity = true;
					Dust expr_62_cp_0 = Main.dust[num];
					expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
					Dust expr_92_cp_0 = Main.dust[num];
					expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
					if(Main.dust[num].position != npc.Center) {
						Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
					}
				}
			}
		}

		private static int[] SpawnTiles = { };
		int chargetimer = 0;
		bool charging = false;
		Vector2 targetLocation = Vector2.Zero;
		float chargeRotation = 0;
		public override void AI()
		{
			Player player = Main.player[npc.target];
			float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.14f;
			npc.scale = num395 + 0.95f;
			float velMax = 1f;
			float acceleration = 0.011f;
			npc.TargetClosest(true);
			Vector2 center = npc.Center;
			float deltaX = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - center.X;
			float deltaY = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - center.Y;
			float distance = (float)Math.Sqrt((double)deltaX * (double)deltaX + (double)deltaY * (double)deltaY);
			npc.ai[1] += 1f;
			if(!charging) {
				if((double)npc.ai[1] > 600.0) {
					acceleration *= 8f;
					velMax = 4f;
					if((double)npc.ai[1] > 650.0) {
						npc.ai[1] = 0f;
					}
				} else if((double)distance < 250.0) {
					npc.ai[0] += 0.9f;
					if(npc.ai[0] > 0f) {
						npc.velocity.Y = npc.velocity.Y + 0.019f;
					} else {
						npc.velocity.Y = npc.velocity.Y - 0.019f;
					}
					if(npc.ai[0] < -100f || npc.ai[0] > 100f) {
						npc.velocity.X = npc.velocity.X + 0.019f;
					} else {
						npc.velocity.X = npc.velocity.X - 0.019f;
					}
					if(npc.ai[0] > 200f) {
						npc.ai[0] = -200f;
					}
				}
				if((double)distance > 350.0) {
					velMax = 5f;
					acceleration = 0.3f;
				} else if((double)distance > 300.0) {
					velMax = 3f;
					acceleration = 0.2f;
				} else if((double)distance > 250.0) {
					velMax = 1.5f;
					acceleration = 0.1f;
				}
				float stepRatio = velMax / distance;
				float velLimitX = deltaX * stepRatio;
				float velLimitY = deltaY * stepRatio;
				if(Main.player[npc.target].dead) {
					velLimitX = (float)((double)((float)npc.direction * velMax) / 2.0);
					velLimitY = (float)((double)(-(double)velMax) / 2.0);
				}
				if(npc.velocity.X < velLimitX) {
					npc.velocity.X = npc.velocity.X + acceleration;
				} else if(npc.velocity.X > velLimitX) {
					npc.velocity.X = npc.velocity.X - acceleration;
				}
				if(npc.velocity.Y < velLimitY) {
					npc.velocity.Y = npc.velocity.Y + acceleration;
				} else if(npc.velocity.Y > velLimitY) {
					npc.velocity.Y = npc.velocity.Y - acceleration;
				}
				if((double)velLimitX > 0.0) {
					npc.rotation = (float)Math.Atan2((double)velLimitY, (double)velLimitX);
				}
				if((double)velLimitX < 0.0) {
					npc.rotation = (float)Math.Atan2((double)velLimitY, (double)velLimitX) + 3.14f;
				}
				if(chargetimer < chargeTime - 50) {
					npc.rotation = 0;
				} else if(chargetimer > chargeTime - 40) {
					npc.rotation = chargeRotation;
				}
				if (chargetimer == chargeTime - 40)
                {
                    Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/SamuraiUnsheathe"));
                }
				if(chargetimer > chargeTime - 50 && chargetimer < chargeTime - 40) {
					targetLocation = Main.player[npc.target].Center;
					chargeRotation = npc.rotation;
				}
			}
			chargetimer++;
			if(chargetimer == chargeTime) {
                Main.PlaySound(SoundID.DD2_WyvernDiveDown, npc.Center);
                Vector2 direction = targetLocation - npc.Center;
				direction.Normalize();
				direction.X = direction.X * Main.rand.Next(15, 19);
				direction.Y = direction.Y * Main.rand.Next(9, 10);
				npc.velocity.X = direction.X;
				npc.velocity.Y = direction.Y;
				npc.velocity.Y *= 0.95f;
				npc.velocity.X *= 0.95f;
				for(int i = 0; i < 20; i++) {
					int num = Dust.NewDust(npc.position, npc.width, npc.height, 14, 0f, -2f, 0, default(Color), .8f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					if(Main.dust[num].position != npc.Center)
						Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
				}
				charging = true;
			}
			if(chargetimer >= chargeTime + 20) {
				chargetimer = 0;
				charging = false;
			}
			if(!player.active || player.dead) //despawns when player is ded
			{
				npc.Transform(ModContent.NPCType<PagodaGhostPassive>());
			}
			npc.spriteDirection = npc.direction;
		}
	}
}
