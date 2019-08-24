using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss
{
	[AutoloadBossHead]
	public class AncientFlyer : ModNPC
	{
		public static int _type;

		int timer = 0;
		int moveSpeed = 0;
		int moveSpeedY = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Flier");
			//Main.npcFrames[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 220;
			npc.height = 108;
			npc.damage = 23;
			npc.defense = 14;
			npc.lifeMax = 3800;
			npc.knockBackResist = 0;
			npc.boss = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.npcSlots = 5;
			Main.npcFrameCount[npc.type] = 4;
			bossBag = mod.ItemType("FlyerBag");
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.scale = 1.1f;
		}

		private int Counter;
		public override bool PreAI()
		{
			Counter++;
			int npcType = mod.NPCType("BoneHarpy1");
			bool plantAlive = false;
			for (int num569 = 0; num569 < 200; num569++)
			{
				if ((Main.npc[num569].active && Main.npc[num569].type == (npcType)))
					plantAlive = true;
			}

			if (!plantAlive)
			{
				if (Counter > 1000)
				{
					Vector2 direction = Vector2.One.RotatedByRandom(MathHelper.ToRadians(100));
					int newNPC = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("BoneHarpy1"));
					int newNPC1 = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("BoneHarpy1"));
					int newNPC2 = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("BoneHarpy1"));
					int newNPC3 = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("BoneHarpy1"));
					Main.npc[newNPC].velocity = direction * (Main.rand.Next(4, 6));
					Main.npc[newNPC1].velocity = direction * (Main.rand.Next(-7, 11));
					Main.npc[newNPC2].velocity = direction * (Main.rand.Next(12, 15));
					Main.npc[newNPC3].velocity = direction * (Main.rand.Next(3, 7));
					Counter = 0;
				}
			}
			return true;
		}

		public override void AI()
		{
			bool expertMode = Main.expertMode;
			npc.spriteDirection = npc.direction;
			Player player = Main.player[npc.target];
			if (npc.Center.X >= player.Center.X && moveSpeed >= -120) // flies to players x position
				moveSpeed--;
			else if (npc.Center.X <= player.Center.X && moveSpeed <= 120)
				moveSpeed++;

			npc.velocity.X = moveSpeed * 0.13f;

			if (npc.Center.Y >= player.Center.Y - 330f && moveSpeedY >= -30) //Flies to players Y position
				moveSpeedY--;
			else if (npc.Center.Y <= player.Center.Y - 330f && moveSpeedY <= 30)
				moveSpeedY++;

			npc.velocity.Y = moveSpeedY * 0.13f;

			timer++;
			if (timer == 300 || timer == 400 && npc.life >= (npc.lifeMax / 2)) //Fires desert feathers like a shotgun
			{
				Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);

				Main.PlaySound(3, (int)npc.position.X, (int)npc.position.Y, 2);
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction.X *= 14f;
				direction.Y *= 14f;

				int amountOfProjectiles = Main.rand.Next(8, 11);
				for (int i = 0; i < amountOfProjectiles; ++i)
				{
					float A = (float)Main.rand.Next(-200, 200) * 0.01f;
					float B = (float)Main.rand.Next(-200, 200) * 0.01f;
					int damage = expertMode ? 15 : 17;
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, mod.ProjectileType("DesertFeather"), damage, 1, Main.myPlayer, 0, 0);
				}
			}
			else if (timer == 300 || timer == 400 || timer == 500 || timer == 550)
			{
				if (Main.expertMode && npc.life >= (npc.lifeMax / 2))
				{
					Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);

					Main.PlaySound(3, (int)npc.position.X, (int)npc.position.Y, 2);

					Vector2 direction = Main.player[npc.target].Center - npc.Center;
					direction.Normalize();
					direction.X *= 15f;
					direction.Y *= 15f;

					int amountOfProjectiles = Main.rand.Next(5, 9);
					for (int i = 0; i < amountOfProjectiles; ++i)
					{
						float A = (float)Main.rand.Next(-300, 300) * 0.01f;
						float B = (float)Main.rand.Next(-300, 300) * 0.01f;
						int damage = expertMode ? 18 : 20;
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, mod.ProjectileType("ExplodingFeather"), damage, 1, Main.myPlayer, 0, 0);
					}
				}
			}
			else if (timer == 600|| timer == 650 || timer == 700 || timer == 800 || timer == 850 || timer == 880) // Fires bone waves
			{
				Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);

				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				int damage = expertMode ? 16 : 19;
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X * 12f, direction.Y * 12f, mod.ProjectileType("BoneWave"), damage, 1, Main.myPlayer, 0, 0);
			}
			else if (timer >= 900 && timer <= 1200) //Rains red comets
			{
				if (Main.rand.Next(12) == 0)
				{
					Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);

					int A = Main.rand.Next(-200, 200) * 6;
					int B = Main.rand.Next(-200, 200) - 1000;
					int damage = expertMode ? 18 : 22;
					Projectile.NewProjectile(player.Center.X + A, player.Center.Y + B, 0f, 14f, mod.ProjectileType("RedComet"), damage, 1, Main.myPlayer, 0, 0);
				}
			}
			else if (timer >= 1000) //sets velocity to 0, creates dust
			{
				npc.velocity.X = 0f;
				npc.velocity.Y = 0f;

				if (Main.rand.Next(2) == 0)
				{
					int dust = Dust.NewDust(npc.position, npc.width, npc.height, 60);
					Main.dust[dust].scale = 2f;
				}

			}

			if (timer >= 1200)
			{
				timer = 0;
			}

			if (Main.expertMode && npc.life <= 3000) //Fires comets when low on health in expert
			{
				if (Main.rand.Next(22) == 0)
				{
					int A = Main.rand.Next(-2500, 2500) * 2;
					int B = Main.rand.Next(-1000, 1000) - 700;
					int damage = expertMode ? 15 : 17;
					Projectile.NewProjectile(player.Center.X + A, player.Center.Y + B, 0f, 14f, mod.ProjectileType("RedComet"), damage, 1, Main.myPlayer, 0, 0);
				}
			}

			if (!player.active || player.dead) //despawns when player is ded
			{
				npc.TargetClosest(false);
				npc.velocity.Y = -50;
				timer = 0;
			}
		}


		public override bool PreNPCLoot()
		{
			MyWorld.downedAncientFlier = true;
			return true;
		}

		public override void NPCLoot()
		{
			if (Main.expertMode)
			{
				npc.DropBossBags();
				return;
			}

			npc.DropItem(mod.ItemType("FossilFeather"), 3, 6);

			string[] lootTable = { "SkeletalonStaff", "Talonginus" };
			int loot = Main.rand.Next(lootTable.Length);
			npc.DropItem(mod.ItemType(lootTable[loot]));

			npc.DropItem(Items.Armor.Masks.FlierMask._type, 1f / 7);
			npc.DropItem(Items.Boss.Trophy2._type, 1f / 10);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gore4"), 1f);
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
	}
}
