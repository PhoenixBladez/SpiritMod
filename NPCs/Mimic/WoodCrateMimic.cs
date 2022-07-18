using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Mimic
{
	public class WoodCrateMimic : ModNPC
	{
		bool jump = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wooden Crate Mimic");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 48;
			NPC.height = 42;
			NPC.damage = 9;
			NPC.defense = 4;
			NPC.lifeMax = 50;
			NPC.HitSound = SoundID.NPCHit3;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 360f;
			NPC.knockBackResist = 0.34f;
			NPC.aiStyle = 3;
			AIType = NPCID.SnowFlinx;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.WoodCrateMimicBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
				new FlavorTextBestiaryInfoElement("These inconspicuous imposters are rather suspicious. Exercise caution before approaching them for what lies within."),
			});
		}

		int frame = 2;
		int mimictimer = 0;

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;

			if (NPC.wet)
				NPC.noGravity = true;
			else
				NPC.noGravity = false;

			if (NPC.collideY && jump && NPC.velocity.Y > 0)
			{
				if (Main.rand.NextBool(8))
				{
					jump = false;
					for (int i = 0; i < 20; i++)
					{
						int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.SpookyWood, NPC.velocity.X * 0.5f, NPC.velocity.Y * 0.5f);
						Main.dust[dust].noGravity = true;
					}
				}
			}

			if (!NPC.collideY)
				jump = true;
		}

		public override void FindFrame(int frameHeight)
		{
			mimictimer++;
			if (mimictimer <= 80)
			{
				frame = 0;
				mimictimer = 81;
			}

			NPC.frameCounter++;
			if (NPC.frameCounter == 4)
			{
				frame++;
				NPC.frameCounter = 0;
			}

			if (frame == 4)
				frame = 1;

			NPC.frame.Y = frameHeight * frame;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 6, 220);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 6, 221);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 6, 222);
			}

			for (int k = 0; k < 30; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.WoodFurniture, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.Add(Terraria.GameContent.ItemDropRules.ItemDropRule.Common(ItemID.WoodenCrate));
	}
}