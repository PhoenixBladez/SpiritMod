using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class Spitfly : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spitfly");
			Main.npcFrameCount[npc.type] = 2;
		}

		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 24;
			npc.damage = 50;
			npc.defense = 9;
			npc.lifeMax = 130;
			npc.HitSound = SoundID.NPCHit44;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 3060f;
			npc.knockBackResist = .45f;
			npc.aiStyle = 2;
			npc.noGravity = true;
			aiType = NPCID.TheHungryII;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe || !Main.hardMode)
			{
				return 0f;
			}
			return SpawnCondition.Cavern.Chance * 0.0453f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 107, 0f, 0f, 100, default(Color), 2f);
			for (int i = 0; i < 10; i++)
				;
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Spitfly_winga"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Spitfly_Body"));
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(2) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Acid"));
		}

	}
}
