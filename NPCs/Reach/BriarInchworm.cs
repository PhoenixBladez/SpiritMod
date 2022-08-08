using Microsoft.Xna.Framework;
using SpiritMod.Items.Consumable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Reach
{
	public class BriarInchworm : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Briar Inchworm");
			Main.npcFrameCount[NPC.type] = 2;
			Main.npcCatchable[NPC.type] = true;
		}

		public override void SetDefaults()
		{
			NPC.width = 16;
			NPC.height = 12;
			NPC.damage = 0;
			NPC.defense = 999;
			NPC.lifeMax = 5;
			NPC.dontCountMe = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.catchItem = (short)ModContent.ItemType<BriarInchwormItem>();
			NPC.knockBackResist = .45f;
			NPC.aiStyle = 66;
			NPC.npcSlots = 0;
            NPC.noGravity = false;
			AIType = NPCID.Grubby;
			SpawnModBiomes = new int[1] { ModContent.GetInstance<Biomes.BriarSurfaceBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				new FlavorTextBestiaryInfoElement("A long, thin worm that evolved to thrive in the murky mud of the Briar. It primarily feeds on the decomposed tall grasses found on the surface."),
			});
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
				for (int k = 0; k < 10; k++)
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Scarecrow, 2.75f * hitDirection, -2.75f, 0, new Color(), 0.6f);
		}

		public override void AI()
        {
            NPC.spriteDirection = NPC.direction;
            if (NPC.life == NPC.lifeMax)
                NPC.defense = 999;
            else
                NPC.defense = 0;
        }

		public override void FindFrame(int frameHeight)
		{
			if (NPC.velocity != Vector2.Zero || NPC.IsABestiaryIconDummy)
            {
                NPC.frameCounter += 0.12f;
                NPC.frameCounter %= Main.npcFrameCount[NPC.type];
                int frame = (int)NPC.frameCounter;
                NPC.frame.Y = frame * frameHeight;
            }
        }
    }
}
