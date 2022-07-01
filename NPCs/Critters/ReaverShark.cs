using SpiritMod.Items.Consumable.Fish;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
namespace SpiritMod.NPCs.Critters
{
	public class ReaverShark : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reaver Shark");
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override void SetDefaults()
		{
			NPC.width = 54;
			NPC.height = 24;
			NPC.damage = 36;
			NPC.defense = 0;
			NPC.lifeMax = 320;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.2f;
			NPC.aiStyle = 16;
			NPC.dontCountMe = true;
			NPC.noGravity = true;
			NPC.npcSlots = 0;
			AIType = NPCID.Shark;
		}
		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

        public override void AI()
        {
            NPC.spriteDirection = NPC.direction;
        }
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/ReaverSharkGore").Type, 1f);
			}
			for (int k = 0; k < 11; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, NPC.direction, -1f, 1, default, .61f);
			}
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, NPC.direction, -1f, 1, default, .91f);
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if(Main.rand.NextBool(4)) {
				target.AddBuff(BuffID.Bleeding, 200);
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon(ItemID.SharkFin);
			npcLoot.AddCommon<RawFish>(2);
			npcLoot.AddCommon(ItemID.ReaverShark, 2);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.Player.ZoneBeach && spawnInfo.Water ? 0.0035f : 0f;
		}

	}
}
