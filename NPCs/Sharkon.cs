using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class Sharkon : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sharkon");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Shark];
		}

		public override void SetDefaults()
		{
			npc.width = 118;
			npc.height = 42;
			npc.damage = 60;
			npc.defense = 12;
			npc.lifeMax = 600;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.noGravity = true;
			npc.value = 60f;
			npc.knockBackResist = .55f;
			npc.aiStyle = 16;
			aiType = NPCID.Arapaima;
			animationType = NPCID.Shark;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe || !Main.hardMode)
			{
				return 0f;
			}
			return SpawnCondition.OceanMonster.Chance * 0.08f;
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(25) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Sharkon"), 1);
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Shark_Gore"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gore_577"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gore_578"), 1f);
			}
		}
	}
}
