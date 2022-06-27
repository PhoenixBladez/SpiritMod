using SpiritMod.Items.Consumable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Critters
{
	public class HailSnail : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hail Snail");
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
			NPC.catchItem = (short)ModContent.ItemType<HailSnailItem>();
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
                Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/HailSnail1").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/HailSnail2").Type, 1f);
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.ZoneSnow && Main.raining && spawnInfo.Player.ZoneOverworldHeight ? 0.0135f : 0f;
        }
	}
}
