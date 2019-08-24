using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class DiseasedBat : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diseased Bat");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.CaveBat];
		}

		public override void SetDefaults()
		{
			npc.width = 26;
			npc.height = 18;
			npc.damage = 16;
			npc.defense = 5;
			npc.lifeMax = 21;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath4;
			npc.value = 60f;
			npc.knockBackResist = .45f;
			npc.aiStyle = 14;
			aiType = NPCID.CaveBat;
			animationType = NPCID.CaveBat;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe)
			{
				return 0f;
			}
			return SpawnCondition.Cavern.Chance * 0.049f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DBat1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Dbat2"), 1f);
			}
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BismiteCrystal"), Main.rand.Next(2) + 1);
		}

	}
}
