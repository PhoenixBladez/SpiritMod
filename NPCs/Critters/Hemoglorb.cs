using SpiritMod.Items.Consumable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Critters
{
	public class Hemoglorb : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hemoglorb");
			Main.npcFrameCount[NPC.type] = 7;
			Main.npcCatchable[NPC.type] = true;
		}

		public override void SetDefaults()
		{
			NPC.width = 16;
			NPC.height = 12;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.dontCountMe = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.catchItem = (short)ModContent.ItemType<HemoglorbItem>();
			NPC.knockBackResist = .45f;
			NPC.aiStyle = 67;
			NPC.npcSlots = 0;
            NPC.noGravity = false;
			AIType = NPCID.Snail;
			NPC.dontTakeDamageFromHostiles = false;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
				new FlavorTextBestiaryInfoElement("The forgotten kin of the Crimson, casted aside due to his abnormal shape and size. He never could, and likely never will, find love nor acceptance in this cruel world."),
			});
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
                for (int k = 0; k < 20; k++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 1.75f * hitDirection, -1.75f, 0, new Color(), 0.86f);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneCrimson && spawnInfo.Player.ZoneOverworldHeight ? .03f : 0f;
		public override void AI() => NPC.spriteDirection = NPC.direction;

		public override void FindFrame(int frameHeight)
		{
			if (NPC.velocity != Vector2.Zero || NPC.IsABestiaryIconDummy)
			{
				NPC.frameCounter += 0.15f;
				NPC.frameCounter %= Main.npcFrameCount[NPC.type];
				int frame = (int)NPC.frameCounter;
				NPC.frame.Y = frame * frameHeight;
			}
		}
	}
}
