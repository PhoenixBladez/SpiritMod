using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.NPCs.ChestZombie
{
	public class Chest_Zombie : ModNPC
	{
		public int dashTimer = 0;
		public bool isDashing = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chest Zombie");
			Main.npcFrameCount[npc.type] = 17;
		}
		public override void SetDefaults()
		{
			npc.lifeMax = 120;
			npc.defense = 10;
			npc.value = 100f;
			aiType = -1;
			npc.knockBackResist = 0.2f;
			npc.width = 30;
			npc.height = 50;
			npc.damage = 17;
			npc.lavaImmune = true;
			npc.noTileCollide = false;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath2;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) => SpawnCondition.OverworldNightMonster.Chance * 0.011f;
		public override void AI()
		{
			Player player = Main.player[npc.target];

			npc.TargetClosest(true);
			
			if (npc.velocity.X > 0f)
					npc.spriteDirection = 1;
			else
				npc.spriteDirection = -1;

			dashTimer++;
			npc.aiStyle = 3;
			if (dashTimer >= 120) {
				npc.aiStyle = -1;
				npc.velocity.X *= 0.98f;
				npc.damage = 60;
				if (dashTimer == 120)
				{
					npc.frameCounter = 0;
					npc.netUpdate = true;
				}
				isDashing = true;

				Collision.StepUp(ref npc.position, ref npc.velocity, npc.width, npc.height, ref npc.stepSpeed, ref npc.gfxOffY);

				if (npc.velocity.X == 0)
					dashTimer = 0;
			}
			else {
				npc.damage = 30;
				if (Math.Abs(npc.velocity.X) > 4)
					npc.velocity.X *= 0.94f;
			}
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Zombie_Chest"));
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("OldLeather"), 5);
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{		
			for (int k = 0; k < 4; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default(Color), 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.7f);
			}
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ChestZombie/ChestZombieGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ChestZombie/ChestZombieGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ChestZombie/ChestZombieGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ChestZombie/ChestZombieGore4"), 1f);
			}
		}

		public override void FindFrame(int frameHeight)
		{
			const int Frame_1 = 0;
			const int Frame_2 = 1;
			const int Frame_3 = 2;
			const int Frame_4 = 3;
			const int Frame_5 = 4;
			const int Frame_6 = 5;
			const int Frame_7 = 6;
			const int Frame_8 = 7;
			const int Frame_9 = 8;
			const int Frame_10 = 9;
			const int Frame_11 = 10;
			const int Frame_12 = 11;
			const int Frame_13 = 12;
			const int Frame_14 = 13;
			const int Frame_15 = 14;
			const int Frame_16 = 15;
			const int Frame_17 = 16;

			Player player = Main.player[npc.target];
			npc.frameCounter++;
			if (npc.velocity.X != 0f)
			{
				if (npc.velocity.Y != 0f)
				{
					npc.frame.Y = Frame_1 * frameHeight;
				}	
				else if (isDashing)
				{
					if (npc.frameCounter < 6)
					{
						npc.frame.Y = Frame_9 * frameHeight;
					}
					else if (npc.frameCounter < 12)
					{
						npc.frame.Y = Frame_10 * frameHeight;
					}
					else if (npc.frameCounter < 18)
					{
						npc.frame.Y = Frame_11 * frameHeight;
					}
					else if (npc.frameCounter < 24)
					{
						npc.frame.Y = Frame_12 * frameHeight;
					}
					else if (npc.frameCounter < 30)
					{
						if (npc.frameCounter == 25)
						{
							Main.PlaySound(SoundID.Trackable, (int)npc.position.X, (int)npc.position.Y, 220, 1f, 0f);
							npc.velocity.X = 10f * (float) npc.direction;
							npc.velocity.Y = 0f;
							npc.netUpdate = true;
						}
						npc.frame.Y = Frame_13 * frameHeight;
					}
					else if (npc.frameCounter < 36)
					{
						npc.frame.Y = Frame_14 * frameHeight;
					}
					else if (npc.frameCounter < 42)
					{
						npc.frame.Y = Frame_15 * frameHeight;
					}
					else if (npc.frameCounter < 48)
					{
						npc.frame.Y = Frame_16 * frameHeight;
					}
					else if (npc.frameCounter < 54)
					{
						npc.frame.Y = Frame_17 * frameHeight;
						if (npc.frameCounter == 49)
						{
							dashTimer = 0;
							isDashing = false;
						}	
					}
					else
					{
						npc.frameCounter = 0;
					}
				}
				else 
				{
					if (npc.frameCounter < 7)
					{
						npc.frame.Y = Frame_1 * frameHeight;
					}
					else if (npc.frameCounter < 14)
					{
						npc.frame.Y = Frame_2 * frameHeight;
					}
					else if (npc.frameCounter < 21)
					{
						npc.frame.Y = Frame_3 * frameHeight;
					}
					else if (npc.frameCounter < 28)
					{
						npc.frame.Y = Frame_4 * frameHeight;
					}
					else if (npc.frameCounter < 35)
					{
						npc.frame.Y = Frame_5 * frameHeight;
					}
					else if (npc.frameCounter < 42)
					{
						npc.frame.Y = Frame_6 * frameHeight;
					}
					else if (npc.frameCounter < 49)
					{
						npc.frame.Y = Frame_7 * frameHeight;
					}
					else if (npc.frameCounter < 56)
					{
						npc.frame.Y = Frame_8 * frameHeight;
					}
					else
					{
						npc.frameCounter = 0;
					}
				}
			}
			else
				npc.frame.Y = Frame_1 * frameHeight;
		}
	}
}