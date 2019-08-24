using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class Wrath : ModNPC
	{
		public int frameNumber = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wrath");
			Main.npcFrameCount[npc.type] = 3;
		}

		public override void SetDefaults()
		{
			npc.width = 68;
			npc.height = 64;
			npc.damage = 75;
			npc.defense = 24;
			npc.lifeMax = 2000;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath4;
			npc.value = 5960f;
			npc.knockBackResist = .5f;
			npc.aiStyle = 14;
			aiType = NPCID.Wraith;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe || !NPC.downedMoonlord)
			{
				return 0f;
			}
			return SpawnCondition.OverworldDay.Chance * 0.07f;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(mod.BuffType("Toxify"), 1200);
		}

		public override void AI()
		{
			if (npc.velocity.X >= 0)
				npc.spriteDirection = 1;
			else
				npc.spriteDirection = -1;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter++;
			if (npc.frameCounter % 6 == 0)
			{
				frameNumber++;
				if (frameNumber >= 3)
				{
					frameNumber = 0;
				}
				npc.frame.Y = frameHeight * frameNumber;
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Wrath"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Wrath_Head"), 1f);
			}
		}
	}
}
