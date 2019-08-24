using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class BScourge : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubonic Scourge");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.NebulaBeast];
		}

		public override void SetDefaults()
		{
			npc.width = 64;
			npc.height = 36;
			npc.damage = 85;
			npc.defense = 28;
			npc.lifeMax = 700;
			npc.HitSound = SoundID.NPCHit19;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.value = 80000f;
			npc.knockBackResist = 0f;
			npc.aiStyle = 26;
			aiType = NPCID.Unicorn;
			animationType = NPCID.NebulaBeast;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (!NPC.downedMoonlord)
			{
				return 0f;
			}
			return SpawnCondition.OverworldNightMonster.Chance * 0.1f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ScourgeGore"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ScourgeGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ScourgeGore2"), 1f);
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Venom, 420);
		}
	}
}
