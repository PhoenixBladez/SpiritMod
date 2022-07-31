using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.TideDrops;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Tides
{
	public class KakamoraParachuter : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kakamora Windglider");
			Main.npcFrameCount[NPC.type] = 3;
		}

		public override void SetDefaults()
		{
			NPC.width = 46;
			NPC.height = 60;
			NPC.damage = 18;
			NPC.defense = 6;
			NPC.lifeMax = 160;
			NPC.noGravity = true;
			NPC.knockBackResist = .9f;
			NPC.value = 200f;
			NPC.noTileCollide = false;
			NPC.HitSound = SoundID.NPCHit2;
			NPC.DeathSound = SoundID.NPCDeath1;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.KakamoraGliderBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
				new FlavorTextBestiaryInfoElement("Any army needs air support! Only the strongest, glidiest palm leaves are selected for use by Kakamora Windgliders."),
			});
		}

		public override void AI()
		{
			if (NPC.ai[3] == 1 && NPC.velocity.Y == 0)
			{
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(NPC.direction * -4, -0.5f), ModContent.ProjectileType<StrayGlider>(), 0, 0);
				switch (Main.rand.Next(4))
				{
					case 0:
						NPC.Transform(ModContent.NPCType<KakamoraRunner>());
						break;
					case 1:
						NPC.Transform(ModContent.NPCType<SpearKakamora>());
						break;
					case 2:
						NPC.Transform(ModContent.NPCType<SwordKakamora>());
						break;
					case 3:
						NPC.Transform(ModContent.NPCType<KakamoraShielder>());
						break;
				}
				NPC.netUpdate = true;
			}
			if (NPC.ai[3] == 0)
			{
				NPC.ai[3] = 1;
				NPC.position.Y -= Main.rand.Next(1300, 1700);
				NPC.netUpdate = true;
			}
			if (NPC.wet)
			{
				NPC.noGravity = true;
				NPC.velocity.Y -= .0965f;
			}
			else
				NPC.noGravity = false;

			NPC.spriteDirection = NPC.direction;
			Player player = Main.player[NPC.target];

			if (player.position.X > NPC.position.X)
				NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X + 0.2f, -4, 4);
			else
				NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X - 0.2f, -4, 4);

			NPC.velocity.Y = 1;

			if (NPC.collideY)
			{
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(NPC.direction * -4, -0.5f), ModContent.ProjectileType<StrayGlider>(), 0, 0);
				switch (Main.rand.Next(4))
				{
					case 0:
						NPC.Transform(ModContent.NPCType<KakamoraRunner>());
						NPC.life = NPC.life;
						break;
					case 1:
						NPC.Transform(ModContent.NPCType<SpearKakamora>());
						NPC.life = NPC.life;
						break;
					case 2:
						NPC.Transform(ModContent.NPCType<SwordKakamora>());
						NPC.life = NPC.life;
						break;
					case 3:
						NPC.Transform(ModContent.NPCType<KakamoraShielder>());
						NPC.life = NPC.life;
						break;
				}
				NPC.netUpdate = true;
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<CoconutGun>(50);
			npcLoot.AddCommon<TikiJavelin>(50);
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.25f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 10; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.DynastyWood, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.DynastyWood, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			if (NPC.life <= 0)
			{
				SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/Kakamora/KakamoraDeath"), NPC.Center);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Kakamora_Gore").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Kakamora_Gore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Kakamora_GoreGlider").Type, 1f);
			}
		}
	}
}