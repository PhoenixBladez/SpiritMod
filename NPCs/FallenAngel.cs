using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class FallenAngel : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fallen Angel");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.FlyingFish];
		}

		public override void SetDefaults()
		{
			npc.width = 44;
			npc.height = 60;
			npc.damage = 50;
			npc.defense = 26;
			npc.lifeMax = 200;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 60f;
			npc.knockBackResist = 0f;
			npc.aiStyle = 44;
			npc.noGravity = true;
			npc.noTileCollide = true;
			aiType = NPCID.FlyingFish;
			animationType = NPCID.FlyingFish;
			npc.stepSpeed = 2f;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.sky && Main.hardMode ? 0.06f : 0f;
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StarPiece"), Main.rand.Next(1) + 1);

			if (Main.rand.Next(100) == 20)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FallenAngel"));
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, 825);
				Gore.NewGore(npc.position, npc.velocity, 826);
				Gore.NewGore(npc.position, npc.velocity, 827);
			}
		}
	}
}
