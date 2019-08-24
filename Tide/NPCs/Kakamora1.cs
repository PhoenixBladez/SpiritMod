using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Tide.NPCs
{
	public class Kakamora1 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cnidarian Kakamora");
			Main.npcFrameCount[npc.type] = 8;
		}

		public override void SetDefaults()
		{
			npc.width = 38;
			npc.height = 38;
			npc.damage = 55;
			npc.defense = 26;
			npc.lifeMax = 410;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 832f;
			npc.knockBackResist = .20f;
			npc.aiStyle = 3;
			aiType = NPCID.AngryBones;

		}

		public override void NPCLoot()
		{
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DepthShard"), 1);
			}
			if (Main.rand.Next(33) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("JellyStaff"), 1);
			}

		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (TideWorld.TheTide && TideWorld.InBeach && NPC.downedMechBossAny)
				return 5f;

			return 0;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(2) == 1)
			{
				target.AddBuff(BuffID.Electrified, 150);
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/KakamoraHead"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Jellylegs"), 1f);
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

