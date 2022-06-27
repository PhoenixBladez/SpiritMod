using SpiritMod.Items.Consumable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Critters
{
	public class GaleSnail : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gale Snail");
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override void SetDefaults()
		{
			NPC.width = 28;
			NPC.dontCountMe = true;
			NPC.height = 24;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			Main.npcCatchable[NPC.type] = true;
			NPC.catchItem = (short)ModContent.ItemType<GaleSnailItem>();
			NPC.knockBackResist = .45f;
			NPC.aiStyle = 67;
			NPC.npcSlots = 0;
			AIType = NPCID.Snail;
            AnimationType = NPCID.Snail;
			NPC.dontTakeDamageFromHostiles = false;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, 13);
                Gore.NewGore(NPC.position, NPC.velocity, 12);
                Gore.NewGore(NPC.position, NPC.velocity, 11);
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return Main.raining && spawnInfo.Player.ZoneOverworldHeight && !spawnInfo.Player.ZoneSnow && !spawnInfo.Player.ZoneCorrupt && !spawnInfo.Player.ZoneCrimson && !spawnInfo.Player.ZoneJungle && !spawnInfo.Player.ZoneBeach? 0.0125f : 0f;
        }
	}
}
