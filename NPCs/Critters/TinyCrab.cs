using SpiritMod.Items.Consumable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Critters
{
	public class TinyCrab : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Palecrab");

			Main.npcFrameCount[npc.type] = 4;
			Main.npcCatchable[npc.type] = true;
		}

		public override void SetDefaults()
		{
			npc.dontCountMe = true;
			npc.width = 18;
			npc.height = 18;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.catchItem = (short)ModContent.ItemType<TinyCrabItem>();
			npc.knockBackResist = .45f;
			npc.aiStyle = 67;
			npc.npcSlots = 0;
			aiType = NPCID.Bunny;
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
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/TinyCrabGore"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/TinyCrabGore"), Main.rand.NextFloat(.5f, .7f));
			}
		}
	}
}
