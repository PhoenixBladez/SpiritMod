using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class Snail : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gargantuan Snail");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.GiantTortoise];
		}

		public override void SetDefaults()
		{
			npc.width = 62;
			npc.height = 36;
			npc.damage = 20;
			npc.defense = 17;
			npc.lifeMax = 95;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 660f;
			npc.knockBackResist = .10f;
			npc.aiStyle = 39;
			aiType = 153;
			animationType = 153;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (SpawnHelper.SupressSpawns(spawnInfo, SpawnFlags.None, SpawnZones.Jungle))
				return 0;

			return spawnInfo.spawnTileY > Main.rockLayer && NPC.downedBoss1 ? 0.0368f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Snail1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Snail2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Snail1"), 1f);
			}
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Carapace"), Main.rand.Next(1) + 2);
		}
	}
}
