using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Tide.NPCs
{
	public class GreenFinTrapper : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Greenfin Trapper");
			Main.npcFrameCount[npc.type] = 7;
		}

		public override void SetDefaults()
		{
			npc.width = 80;
			npc.height = 52;
			npc.damage = 32;
			npc.defense = 13;
			npc.lifeMax = 203;
			npc.HitSound = SoundID.NPCHit12;
			npc.DeathSound = SoundID.NPCDeath8;
			npc.value = 2329f;
			npc.knockBackResist = .30f;
			npc.aiStyle = 26;
			aiType = NPCID.Wolf;

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
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GaleJavelin"), 1);
					}

				}
				{
					if (Main.rand.Next(33) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StrangeKelp"), 1);
					}

				}
			}

		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (TideWorld.TheTide && TideWorld.InBeach)
				return 6.1f;
			else if (TideWorld.TheTide && TideWorld.InBeach && NPC.downedMechBossAny)
				return 5f;

			return 0;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(8) == 1)
			{
				target.AddBuff(mod.BuffType("Trapped"), 120);
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Trapperhead"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Trapperlegs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Trapperlegs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Trapperlegs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Trapperlegs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Trappertail"), 1f);
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

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
		}
	}
}

