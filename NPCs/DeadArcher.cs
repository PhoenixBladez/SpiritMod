using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class DeadArcher : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deadeye Marksman");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.GoblinArcher];
		}

		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 46;
			npc.damage = 22;
			npc.defense = 13;
			npc.lifeMax = 80;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 60f;
			npc.knockBackResist = .30f;
			npc.aiStyle = 3;
			aiType = NPCID.GoblinArcher;
			animationType = NPCID.GoblinArcher;
		}

		public override void NPCLoot()
		{
			int Techs = Main.rand.Next(8, 16);
			if (Main.rand.Next(2) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BloodFire"));
			}
			if (Main.rand.Next(25) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("VitalityStone"));
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.spawnTileY < Main.rockLayer && (Main.bloodMoon) && NPC.downedBoss2 ? 0.08f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Archer2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Archer1"), 1f);
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(4) == 0)
			{
				target.AddBuff(mod.BuffType("BCorrupt"), 180);
			}
		}
	}
}
