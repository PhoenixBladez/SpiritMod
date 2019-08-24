using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class Crimslimer : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crimslimer");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 66;
			npc.height = 36;
			npc.damage = 50;
			npc.defense = 18;
			npc.lifeMax = 65;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 2460f;
			npc.knockBackResist = .34f;
			npc.aiStyle = 14;
			npc.noGravity = true;
			aiType = NPCID.Slimer;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneCrimson && spawnInfo.spawnTileY < Main.rockLayer && Main.hardMode ? 0.02f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Slime_Wing_2"), 1f);
			}
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

		public override bool CheckDead()
		{
			npc.Transform(NPCID.Crimslime);
			npc.life = npc.lifeMax;
			return false;
		}
	}
}
