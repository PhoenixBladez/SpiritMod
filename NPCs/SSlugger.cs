using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class SSlugger : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Slugger");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.HellArmoredBonesSword];
		}

		public override void SetDefaults()
		{
			npc.width = 50;
			npc.height = 40;
			npc.damage = 29;
			npc.defense = 13;
			npc.lifeMax = 200;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 8060f;
			npc.knockBackResist = .40f;
			npc.aiStyle = 26;
			aiType = NPCID.Unicorn;
			animationType = NPCID.HellArmoredBonesSword;
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Bone, 12);
			if (Main.rand.Next(6) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Slugger"), 1);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneDungeon ? 0.03f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Shadow_Blade"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Shadow_Slugger_Head"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Shadow_Slugger_Body"), 1f);
			}
		}
	}
}
