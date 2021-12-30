using SpiritMod.Items.Consumable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Critters
{
	public class Cleft : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cleft Hopper");
			Main.npcFrameCount[npc.type] = 7;
		}

		public override void SetDefaults()
		{
			npc.width = 28;
			npc.dontCountMe = true;
			npc.height = 24;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath4;
			Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)ModContent.ItemType<CleftItem>();
			npc.knockBackResist = .45f;
			npc.aiStyle = 67;
			npc.npcSlots = 0;
			aiType = NPCID.Snail;
			Main.npcFrameCount[npc.type] = 7;
			npc.dontTakeDamageFromHostiles = false;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Cleft/Cleft1"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Cleft/Cleft2"));
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneUndergroundDesert && spawnInfo.spawnTileY > Main.rockLayer ? 0.11f : 0f;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		public override void AI()
		{
			npc.spriteDirection = npc.direction;
		}
	}
}
