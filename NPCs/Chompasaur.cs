using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class Chompasaur : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chompasaur");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 64;
			npc.height = 46;
			npc.damage = 55;
			npc.defense = 18;
			npc.lifeMax = 240;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.value = 9260f;
			npc.knockBackResist = 0f;
			npc.aiStyle = 26;
			aiType = NPCID.Unicorn;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return Main.hardMode && spawnInfo.player.ZoneUndergroundDesert ? 0.16f : 0f;
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(25) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FossilFlower"));
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Chompasaur1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Chompasaur2"), 1f);
			}
		}
	}
}
