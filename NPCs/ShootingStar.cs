using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;


namespace SpiritMod.NPCs
{
	public class ShootingStar : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shooting Star");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ChaosBall];
		}

		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 30;
			npc.damage = 50;
			npc.defense = 20;
			npc.lifeMax = 400;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 5060f;
			npc.knockBackResist = 0f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.aiStyle = 56;
			aiType = NPCID.StardustCellBig;
			animationType = NPCID.ChaosBall;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.sky && Main.hardMode && NPC.downedMechBossAny ? 0.1f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, 13);
				Gore.NewGore(npc.position, npc.velocity, 12);
				Gore.NewGore(npc.position, npc.velocity, 11);
				Gore.NewGore(npc.position, npc.velocity, 13);
				Gore.NewGore(npc.position, npc.velocity, 12);
				Gore.NewGore(npc.position, npc.velocity, 11);

				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 160);
			}
		}

		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 2.06f, 1.74f, 0.84f);

			int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.GoldCoin);
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StarPiece"), Main.rand.Next(1) + 1);
		}

	}
}
