using SpiritMod.Items.Consumable;
using SpiritMod.Items.Consumable.Fish;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.Critters
{
	public class Swordfish : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Swordfish");
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override void SetDefaults()
		{
			NPC.width = 44;
			NPC.height = 20;
			NPC.damage = 40;
			NPC.defense = 0;
			NPC.dontCountMe = true;
			NPC.lifeMax = 165;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = .35f;
			NPC.aiStyle = 16;
			NPC.noGravity = true;
			NPC.npcSlots = 0;
			AIType = NPCID.CorruptGoldfish;
		}
		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SwordfishGore").Type);
			}
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, NPC.direction, -1f, 1, default, .61f);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.PlayerSafe)
			{
				return 0f;
			}
			return SpawnCondition.OceanMonster.Chance * 0.0131f;
		}
		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<RawFish>(2);
			npcLoot.AddCommon(ItemID.Swordfish, 2);
		}
	}
}
