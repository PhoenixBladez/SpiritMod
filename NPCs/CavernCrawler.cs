using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class CavernCrawler : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cavern Crawler");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Snail];
		}

		public override void SetDefaults()
		{
			npc.width = 45;
			npc.height = 45;
			npc.damage = 22;
			npc.defense = 9;
			npc.lifeMax = 40;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 860f;
			npc.aiStyle = 3;
			npc.knockBackResist = 0.95f;
			aiType = NPCID.AnomuraFungus;
			animationType = NPCID.Snail;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneUndergroundDesert && NPC.downedBoss1 ? 0.18f : 0f;
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(100) <= 4)
			{

				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("CrawlerockStaff")));
			}
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("Carapace")), Main.rand.Next(2) + 1);
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/CaveCrawler_1"));
			}
		}
	}
}
