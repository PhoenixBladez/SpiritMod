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
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 28;
			npc.dontCountMe = true;
			npc.height = 24;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)ModContent.ItemType<GaleSnailItem>();
			npc.knockBackResist = .45f;
			npc.aiStyle = 67;
			npc.npcSlots = 0;
			aiType = NPCID.Snail;
            animationType = NPCID.Snail;
			npc.dontTakeDamageFromHostiles = false;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, 13);
                Gore.NewGore(npc.position, npc.velocity, 12);
                Gore.NewGore(npc.position, npc.velocity, 11);
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return Main.raining && spawnInfo.player.ZoneOverworldHeight && !spawnInfo.player.ZoneSnow && !spawnInfo.player.ZoneCorrupt && !spawnInfo.player.ZoneCrimson && !spawnInfo.player.ZoneJungle && !spawnInfo.player.ZoneBeach? 0.0125f : 0f;
        }
	}
}
