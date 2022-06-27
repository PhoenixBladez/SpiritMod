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
			Main.npcFrameCount[NPC.type] = 7;
		}

		public override void SetDefaults()
		{
			NPC.width = 28;
			NPC.dontCountMe = true;
			NPC.height = 24;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath4;
			Main.npcCatchable[NPC.type] = true;
			NPC.catchItem = (short)ModContent.ItemType<CleftItem>();
			NPC.knockBackResist = .45f;
			NPC.aiStyle = 67;
			NPC.npcSlots = 0;
			AIType = NPCID.Snail;
			Main.npcFrameCount[NPC.type] = 7;
			NPC.dontTakeDamageFromHostiles = false;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Cleft/Cleft1").Type);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Cleft/Cleft2").Type);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.Player.ZoneUndergroundDesert && spawnInfo.SpawnTileY > Main.rockLayer ? 0.11f : 0f;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}
		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
		}
	}
}
