using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs;
using SpiritMod.Buffs.DoT;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.BottomFeeder
{
	public class BottomFeeder : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bottom Feeder");
			Main.npcFrameCount[NPC.type] = 11;
		}

		public override void SetDefaults()
		{
			NPC.width = 60;
			NPC.height = 68;
			NPC.damage = 24;
			NPC.defense = 9;
			NPC.lifeMax = 175;
			NPC.HitSound = SoundID.NPCHit18;
			NPC.DeathSound = SoundID.NPCDeath5;
			NPC.buffImmune[ModContent.BuffType<BloodCorrupt>()] = true;
			NPC.buffImmune[ModContent.BuffType<BloodInfusion>()] = true;
			NPC.value = 800f;
			NPC.knockBackResist = 0.34f;
			NPC.aiStyle = 3;
			NPC.noGravity = false;
			AIType = NPCID.WalkingAntlion;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.BottomFeederBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon,
				new FlavorTextBestiaryInfoElement("A nasty bile sits in their guts, waiting to be unleashed. Exercise caution as they shamble toward you, and be prepared for a flood of sludge and soot."),
			});
		}

		int frame = 1;
		int timer = 0;
		int shoottimer = 0;

		public override void AI()
		{
			NPC.spriteDirection = -NPC.direction;

			Player target = Main.player[NPC.target];
			float distance = NPC.DistanceSQ(target.Center);

			if (distance < 178 * 178)
			{
				shoottimer++;

				if (!NPC.wet)
				{
					NPC.velocity.X = .01f * NPC.spriteDirection;
					NPC.spriteDirection = -NPC.direction;
					NPC.velocity.Y = 10f;
				}

				if (shoottimer >= 40 && shoottimer < 96)
				{
					if (Main.rand.NextBool(3)&& Main.netMode != NetmodeID.MultiplayerClient)
					{
						int bloodproj = Main.rand.Next(new int[] { ModContent.ProjectileType<Feeder1>(), ModContent.ProjectileType<Feeder2>(), ModContent.ProjectileType<Feeder3>() });
						int damage = Main.expertMode ? 10 : 15;
						float sqrtDist = (float)Math.Sqrt(distance);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + (7 * NPC.direction), NPC.Center.Y - 10, -(NPC.position.X - target.position.X) / sqrtDist * 8, -(NPC.position.Y - target.position.Y + Main.rand.Next(-50, 50)) / sqrtDist * 8, bloodproj, damage, 0);
					}
				}

				if (shoottimer >= 96)
					shoottimer = 0;
			}
		}

		public override void FindFrame(int frameHeight)
		{
			timer++;
			if (timer == 4)
			{
				frame++;
				timer = 0;
			}

			if (!NPC.IsABestiaryIconDummy)
			{
				float distance = NPC.DistanceSQ(Main.player[NPC.target].Center);
				if (frame >= 8 && distance >= 180 * 180)
					frame = 1;
				else if (frame == 11 && distance < 180 * 180)
					frame = 8;
			}

			NPC.frame.Y = frameHeight * frame;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Sets.GunsMisc.Belcher.BottomFeederGun>(), 20));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Consumable.Food.FishFingers>(), 20));
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0 || NPC.life >= 0)
			{
				for (int k = 0; k < 25; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, Color.White, .97f);
				}
			}

			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity * 1.11f, Mod.Find<ModGore>("FeederGore").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity * 1.11f, Mod.Find<ModGore>("FeederGore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity * 1.11f, Mod.Find<ModGore>("FeederGore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity * 1.11f, Mod.Find<ModGore>("FeederGore2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity * 1.11f, Mod.Find<ModGore>("FeederGore2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity * 1.11f, Mod.Find<ModGore>("FeederGore2").Type, 1f);
			}
		}
	}
}