using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class MarbleSlime : ModNPC
	{
		public override void SetDefaults()
		{
			npc.width = 16;
			npc.height = 12;
			npc.damage = 24;
			npc.defense = 6;
			npc.lifeMax = 90;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.value = 60f;
			npc.knockBackResist = .45f;
			npc.aiStyle = 1;
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueSlime];
			aiType = NPCID.BlueSlime;
			animationType = NPCID.BlueSlime;
		}

		public override void AI()
		{
			npc.direction = npc.spriteDirection;
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.5f, 0.41f, .15f);
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MarbleChunk>(), 1);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			int x = spawnInfo.spawnTileX;
			int y = spawnInfo.spawnTileY;
			int tile = (int)Main.tile[x, y].type;
			return (tile == 367) && NPC.downedBoss2 && spawnInfo.spawnTileY > Main.rockLayer ? 0.1f : 0f;
		
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 11; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.GoldCoin, hitDirection, -1f, 0, default(Color), .61f);
			}
		}		
	}
}
