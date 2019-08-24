using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Scarabeus
{
	[AutoloadBossHead]
	public class Scarabeus : ModNPC
	{
		public static int _type;

		private float SpeedMax = 40f;
		private float SpeedDistanceIncrease = 500f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarabeus");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 100;
			npc.height = 60;
			npc.value = 5000;
			npc.damage = 26;
			npc.defense = 10;
			npc.lifeMax = 1500;
			npc.knockBackResist = 0f;
			npc.boss = true;
			npc.npcSlots = 10f;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath5;
			bossBag = mod.ItemType("BagOScarabs");
		}

		private int Counter;
		public override bool PreAI()
		{
			npc.TargetClosest(true);
			npc.spriteDirection = npc.direction;
			Player player = Main.player[npc.target];
			bool expertMode = Main.expertMode;
			bool rage = (double)npc.life <= (double)npc.lifeMax * 0.2;
			if (rage)
				SpeedMax = 40f;

			if (Main.rand.Next(500) == 0)
				Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 44);

			Counter++;
			if (Counter > 250)
			{
				int newNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Scarab"), 0, 0f, 0f, 0f, 0f, 255);
				if (Main.netMode == 2 && newNPC < 200)
					NetMessage.SendData(23, -1, -1, null, newNPC, 0f, 0f, 0f, 0, 0, 0);

				npc.netUpdate = true;
				Counter = 0;
			}
			if (npc.ai[1] != 3f && player.dead)
			{
				npc.TargetClosest(true);
				if (player.dead)
				{
					npc.ai[0] = 0f;
					npc.ai[1] = 3f;
					npc.ai[2] = 0f;
					npc.ai[3] = 0f;
				}
			}

			if (npc.ai[1] == 0f)
			{
				if (npc.Center.X >= player.Center.X && npc.ai[2] >= (0f - SpeedMax))
				{
					for (npc.ai[3] = 0f; npc.ai[3] < Math.Abs(npc.Center.X - player.Center.X); npc.ai[3] = npc.ai[3] + SpeedDistanceIncrease)
					{
						npc.ai[2] -= rage ? 4f : 2f;
					}
					npc.ai[2] -= rage ? 4f : 2f;
				}
				if (npc.Center.X <= player.Center.X && npc.ai[2] <= SpeedMax)
				{
					for (npc.ai[3] = 0f; npc.ai[3] < Math.Abs(npc.Center.X - player.Center.X); npc.ai[3] = npc.ai[3] + SpeedDistanceIncrease)
					{
						npc.ai[2] += rage ? 4f : 2f;
					}
					npc.ai[2] += rage ? 4f : 2f;
				}
				if (npc.ai[0] < 100f)
					npc.ai[0] += expertMode ? 3f : 2f;

				npc.noGravity = false;
				npc.noTileCollide = false;
				if (Main.rand.Next(2) > 0)
					npc.velocity.X = npc.ai[2] * 0.1f;

				npc.velocity.Y = npc.ai[0] * 0.26f;
				if (npc.velocity.X == 0f && Main.rand.Next(3) == 0)
				{
					npc.ai[0] = -40f;
					npc.noTileCollide = true;
				}
				if (expertMode)
				{
					if (Main.rand.Next(60) == 0)
					{
						if (npc.velocity.X < 0f)
						{
							int damage = expertMode ? 6 : 9;
							Projectile.NewProjectile(npc.position.X, npc.Center.Y, 0, 0, mod.ProjectileType("ScarabDust"), damage, 0f, player.whoAmI, 0f, 0f);
						}
						if (npc.velocity.X > 0f)
						{
							int damage = expertMode ? 6 : 9;
							Projectile.NewProjectile(npc.position.X, npc.Center.Y, 0, 0, mod.ProjectileType("ScarabDust"), damage, 0f, player.whoAmI, 0f, 0f);
						}
					}
				}
				else if (Main.rand.Next(120) == 0)
				{
					if (npc.velocity.X < 0f)
						Projectile.NewProjectile(npc.position.X, npc.Center.Y, 0, 0, mod.ProjectileType("ScarabDust"), 7, 0f, player.whoAmI, 0f, 0f);
					if (npc.velocity.X > 0f)
						Projectile.NewProjectile(npc.position.X, npc.Center.Y, 0, 0, mod.ProjectileType("ScarabDust"), 7, 0f, player.whoAmI, 0f, 0f);
				}

				if (expertMode && (Main.rand.Next(300) == 0))
				{
					npc.velocity.Y = 0f;
					npc.velocity.X = 0f;
					npc.ai[0] = -120f;
					npc.ai[3] = 0f;
					npc.ai[1] = 1f;
					npc.netUpdate = true;
				}
				else if ((Main.rand.Next(400) == 0))
				{
					npc.velocity.Y = 0f;
					npc.velocity.X = 0f;
					npc.ai[0] = -120f;
					npc.ai[3] = 0f;
					npc.ai[1] = 1f;
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[1] == 1f)
			{
				npc.noGravity = true;
				npc.noTileCollide = true;
				npc.ai[0] += 1f;
				if (npc.Center.X >= player.Center.X && npc.ai[2] >= 0f - SpeedMax * 1.3f) // flies to players x position
					npc.ai[2] -= expertMode ? 3f : 2f;

				if (npc.Center.X <= player.Center.X && npc.ai[2] <= SpeedMax * 1.3f)
					npc.ai[2] += expertMode ? 3f : 2f;

				npc.velocity.Y = npc.ai[0] * 0.08f;
				npc.velocity.X = npc.ai[2] * 0.1f;
				if (Math.Abs(npc.Center.X - player.Center.X) < 40)
				{
					npc.velocity.Y = 0f;
					npc.velocity.X = 0f;
					npc.ai[0] = 0f;
					npc.ai[2] = 0f;
					npc.ai[3] = 0f;
					npc.ai[1] = 2f;
					npc.netUpdate = true;
				}
				if (npc.ai[0] > 0f)
				{
					npc.ai[3] = 0f;
					npc.ai[1] = 0f;
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[1] == 2f)
			{
				npc.velocity.X = 0f;
				npc.noGravity = false;
				npc.noTileCollide = false;
				npc.ai[0] += 3f;
				npc.velocity.Y = npc.ai[0] * 0.2f;
				if (Math.Abs(npc.Center.Y - player.Center.Y) < 10 || npc.ai[0] > 120f)
				{
					npc.velocity.X = 0f;
					npc.ai[2] = 0f;
					npc.ai[3] = 0f;
					npc.ai[1] = 0f;
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[1] == 3f)
			{
				npc.damage = 0;
				npc.defense = 9999;
				npc.noTileCollide = true;
				npc.alpha += 7;
				if (npc.alpha > 255)
				{
					npc.alpha = 255;
				}
				npc.velocity.X = npc.velocity.X * 0.98f;
			}

			return true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection, -1f, 0, default(Color), 1f);
			}
			if (npc.life <= 0)
			{
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 100;
				npc.height = 60;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 30; num621++)
				{
					int randomDustType = Main.rand.Next(3);
					if (randomDustType == 0)
						randomDustType = 5;
					else if (randomDustType == 1)
						randomDustType = 36;
					else
						randomDustType = 32;

					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, randomDustType, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 50; num623++)
				{
					int randomDustType = Main.rand.Next(3);
					if (randomDustType == 0)
						randomDustType = 5;
					else if (randomDustType == 1)
						randomDustType = 36;
					else
						randomDustType = 32;

					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, randomDustType, 0f, 0f, 100, default(Color), 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, randomDustType, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.75f);
		}

		public override bool PreNPCLoot()
		{
			MyWorld.downedScarabeus = true;
			return true;
		}

		public override void NPCLoot()
		{
			if (Main.expertMode)
			{
				npc.DropBossBags();
				return;
			}

			npc.DropItem(mod.ItemType("Chitin"), 25, 36);
			npc.DropItem(mod.ItemType("GildedIdol"), 1f / 9);

			string[] lootTable = { "ScarabBow", "OrnateStaff", "ScarabSword" };
			int loot = Main.rand.Next(lootTable.Length);
			npc.DropItem(mod.ItemType(lootTable[loot]));

			npc.DropItem(Items.Armor.Masks.ScarabMask._type, 1f / 7);
			npc.DropItem(Items.Boss.Trophy1._type, 1f / 10);
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