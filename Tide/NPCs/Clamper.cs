using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Tide.NPCs
{
	public class Clamper : ModNPC
	{
		int timer = 0;
		int moveSpeed = 0;
		int moveSpeedY = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clamper");
			Main.npcFrameCount[npc.type] = 10;
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 38;
			npc.damage = 31;
			npc.defense = 5;
			npc.lifeMax = 150;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 329f;
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
					if (Main.rand.Next(50) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BabyClamper"), 1);
					}
				}
			}

		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (TideWorld.TheTide && TideWorld.InBeach && !NPC.downedMechBossAny)
				return 12f;

			return 0;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			{
				timer++;
				if (timer < 200) //Fires desert feathers like a shotgun
				{
					npc.defense = 1000;

				}

				if (timer >= 200) //sets velocity to 0, creates dust
				{
					npc.velocity.X = 0f;
					npc.defense = 0;

					if (Main.rand.Next(2) == 0)
					{
						int dust = Dust.NewDust(npc.position, npc.width, npc.height, 108);
						Main.dust[dust].scale = 0.9f;
					}

				}
				if (timer >= 400)
				{
					timer = 0;
				}
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Clampshell"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Clampshell"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Clampeye"), 1f);
				if (TideWorld.TheTide)
				{
					TideWorld.TidePoints2 += 1;
				}
			}

		}
	}
}

