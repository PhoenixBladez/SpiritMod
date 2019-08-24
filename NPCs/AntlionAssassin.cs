using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class AntlionAssassin : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Antlion Assassin");
			Main.npcFrameCount[npc.type] = 15;
		}

		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 44;
			npc.damage = 16;
			npc.defense = 8;
			npc.lifeMax = 59;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 329f;
			npc.knockBackResist = .65f;
			npc.aiStyle = 3;
			aiType = NPCID.AngryBones;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (SpawnHelper.SupressSpawns(spawnInfo, SpawnFlags.Daytime, SpawnZones.Desert | SpawnZones.Overworld))
				return 0;

			if (Main.tileSand[spawnInfo.spawnTileType])
				return SpawnCondition.OverworldDayDesert.Chance * 0.04f;
			return 0;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, 825);
				Gore.NewGore(npc.position, npc.velocity, 826);
				Gore.NewGore(npc.position, npc.velocity, 827);
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

		public override void NPCLoot()
		{
			if (Main.rand.Next(150) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AntlionClaws"));
		}
	}
}
