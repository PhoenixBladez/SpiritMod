using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class Snapper : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snapper");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 52;
			npc.damage = 15;
			npc.defense = 8;
			npc.lifeMax = 70;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 1060f;
			npc.knockBackResist = 0.75f;
			npc.aiStyle = 41;
			aiType = NPCID.Herpling;
			animationType = 530;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe || !NPC.downedBoss1)
			{
				return 0f;
			}
			return SpawnCondition.Cavern.Chance * 0.06f;
		}

		public override void NPCLoot()
		{
			int Techs = Main.rand.Next(2, 5);
			for (int J = 0; J <= Techs; J++)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Carapace"));
			}
			if (Main.rand.Next(22) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MagicConch"), 1);
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Snapper_Head"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Snapper_Body"), 1f);

			}
		}
	}
}
