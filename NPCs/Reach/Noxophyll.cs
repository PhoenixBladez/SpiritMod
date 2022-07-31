using SpiritMod.Items.Consumable.Fish;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Reach
{
	public class Noxophyll : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Noxophyll");
			Main.npcFrameCount[NPC.type] = 8;
			Main.npcCatchable[NPC.type] = true;
		}

		public override void SetDefaults()
		{
			NPC.width = 34;
			NPC.height = 24;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.catchItem = (short)ModContent.ItemType<Items.Sets.BriarDrops.ReachFishingCatch>();
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = .35f;
			NPC.aiStyle = 16;
			NPC.noGravity = true;
			NPC.npcSlots = 0;
			NPC.dontCountMe = true;
			SpawnModBiomes = new int[1] { ModContent.GetInstance<Biomes.BriarSurfaceBiome>().Type };
			AIType = NPCID.Goldfish;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				new FlavorTextBestiaryInfoElement("A fish with the ability to secrete a toxin that coats its scales. The only way to truly deter the smell is cooking it."),
			});
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GrassBlades, 1.5f * hitDirection, -1.5f, 0, Color.White, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, 7, 1.5f * hitDirection, -1.5f, 0, default, .34f);
			}
		}

		public override void AI() => NPC.spriteDirection = NPC.direction;
		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddCommon<RawFish>(2);

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.GetSpiritPlayer().ZoneReach && spawnInfo.Water ? 0.511f : 0f;
	}
}