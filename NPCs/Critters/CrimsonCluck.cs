using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SpiritMod.NPCs.Critters
{
	public class CrimsonCluck : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crimson Cluck");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			npc.width = 30;
			npc.height = 30;
			npc.damage = 0;
			npc.dontCountMe = true;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath4;
			Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)ModContent.ItemType<CluckItem>();
			npc.knockBackResist = .45f;
			npc.aiStyle = 24;
			npc.npcSlots = 0;
			npc.noGravity = true;
			aiType = NPCID.Bird;
			animationType = NPCID.Bird;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Cluck1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Cluck2"), 1f);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
            Player player = spawnInfo.player;
            if (!player.GetSpiritPlayer().ZoneReach)
            {
                return 0f;
            }
            return SpawnCondition.OverworldDayBirdCritter.Chance * 0.08f;
        }
		public override void AI()
		{
			npc.spriteDirection = npc.direction;
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(2) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CrimsonFeather>(), 1);
			}
		}
	}
}
