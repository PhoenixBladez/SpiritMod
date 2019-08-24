using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class LostMime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lost Mime");
			Main.npcFrameCount[npc.type] = 17;
		}

		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 42;
			npc.damage = 30;
			npc.defense = 10;
			npc.lifeMax = 200;
			npc.HitSound = SoundID.NPCHit48;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 2060f;
			npc.knockBackResist = .65f;
			npc.aiStyle = 3;
			aiType = NPCID.AngryBones;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe)
			{
				return 0f;
			}
			return SpawnCondition.Cavern.Chance * 0.007f;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Confused, 60);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, 13);
				Gore.NewGore(npc.position, npc.velocity, 12);
				Gore.NewGore(npc.position, npc.velocity, 11);
			}
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MimeMask"), 1);
		}

	}
}
