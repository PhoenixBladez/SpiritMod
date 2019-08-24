using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Tide.NPCs
{
	public class MindFlayer : ModNPC
	{
		int timer = 0;
		int moveSpeed = 0;
		int moveSpeedY = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mind Flayer");
			Main.npcFrameCount[npc.type] = 9;
		}

		public override void SetDefaults()
		{
			npc.width = 48;
			npc.height = 54;
			npc.damage = 26;
			npc.defense = 9;
			npc.lifeMax = 90;
			npc.HitSound = SoundID.NPCHit25;
			npc.DeathSound = SoundID.NPCDeath28;
			npc.value = 929f;
			npc.knockBackResist = 0f;
			npc.aiStyle = 3;
			aiType = NPCID.CyanBeetle;

		}

		public override void NPCLoot()
		{
			{
				if (Main.rand.Next(2) == 0 && !NPC.downedMechBossAny)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PearlFragment"), 1);
				}
				{
					if (Main.rand.Next(33) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FlayerStaff"), 1);
					}
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (TideWorld.TheTide && TideWorld.InBeach)
				return 3.4f;

			return 0;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/JellyLegs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/JellyLegs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/JellyLegs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/JellyLegs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Jellyhead"), 1f);
				if (TideWorld.TheTide)
				{
					TideWorld.TidePoints2 += 1;
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		private int Counter;
		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			{
				timer++;
				if (timer == 100 || timer == 200)
				{

				}

				{
					if (timer >= 300) //sets velocity to 0, creates dust
					{
						npc.velocity.X = 0f;
						npc.velocity.Y = 0f;
						npc.spriteDirection = npc.direction;
						Counter++;
						if (Counter > 33)
						{
							Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 14f, mod.ProjectileType("Flay"), 16, 1, Main.myPlayer, 0, 0);
							Counter = 0;
						}

						if (Main.rand.Next(2) == 0)
						{
							int dust = Dust.NewDust(npc.position, npc.width, npc.height, 187);
							Main.dust[dust].scale = 2f;
						}
					}
					if (timer >= 350)
					{
						timer = 0;
					}
				}
			}
		}
	}
}

