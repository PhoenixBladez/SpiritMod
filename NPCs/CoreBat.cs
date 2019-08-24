using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class CoreBat : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Core Bat");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 26;
			npc.height = 18;
			npc.damage = 22;
			npc.defense = 16;
			npc.lifeMax = 80;
			npc.HitSound = SoundID.NPCHit41;
			npc.DeathSound = SoundID.NPCDeath4;
			npc.value = 660f;
			npc.knockBackResist = .24f;
			npc.aiStyle = 14;
			aiType = NPCID.CaveBat;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			int x = spawnInfo.spawnTileX;
			int y = spawnInfo.spawnTileY;
			int tile = (int)Main.tile[x, y].type;
			return (tile == 368) && spawnInfo.spawnTileY > Main.rockLayer && NPC.downedBoss2 ? 0.1f : 0f;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/CBat1"), 1f);
			}
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GraniteChunk"), 1);
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			int dust = Dust.NewDust(npc.position, npc.width, npc.height, 187);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(6) == 1)
			{
				target.AddBuff(BuffID.Confused, 160);
			}
		}
	}
}
