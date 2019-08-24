using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class CElemental : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crimtane Elemental");
			Main.npcFrameCount[npc.type] = 10;
		}

		public override void SetDefaults()
		{
			npc.width =30;
			npc.height = 32;
			npc.damage = 27;
			npc.defense = 11;
			npc.lifeMax = 45;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 1290f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.knockBackResist = .5f;
			npc.aiStyle = 91;
			aiType = NPCID.GraniteFlyer;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneCrimson && spawnInfo.spawnTileY > Main.rockLayer ? 0.02f : 0f;
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
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), .27f, 0.1f, 0.06f);
			
			npc.spriteDirection = npc.direction;
		}
	}
}
