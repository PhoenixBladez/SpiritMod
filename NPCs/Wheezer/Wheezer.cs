using Microsoft.Xna.Framework;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Armor.ClatterboneArmor;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.Wheezer
{
	public class Wheezer : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wheezer");
			Main.npcFrameCount[NPC.type] = 16;
		}

		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 36;
			NPC.damage = 18;
			NPC.defense = 9;
			NPC.lifeMax = 50;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath53;
			NPC.value = 120f;
			NPC.knockBackResist = .35f;
			NPC.aiStyle = 3;
			NPC.buffImmune[BuffID.Poisoned] = true;

			AIType = NPCID.Skeleton;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.WheezerBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.PlayerSafe || !NPC.downedBoss1)
				return 0f;
			if (Main.hardMode)
				return SpawnCondition.Cavern.Chance * 0.03f;				
			return SpawnCondition.Cavern.Chance * 0.17f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 11; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hitDirection, -1f, 0, default, .61f);
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("WheezerGore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("WheezerGore2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("WheezerGore3").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("WheezerGore4").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("WheezerGore4").Type, 1f);
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WheezerScale>(), 15));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Sets.FlailsMisc.ClatterMace.ClatterMace>(), 60));
			npcLoot.Add(ItemDropRule.Common(ItemID.DepthMeter, 80));
			npcLoot.Add(ItemDropRule.Common(ItemID.Compass, 80));
			npcLoot.Add(ItemDropRule.Common(ItemID.Rally, 20));
			npcLoot.Add(ItemDropRule.Common(ItemID.Bezoar, 100));
			npcLoot.Add(ItemDropRule.OneFromOptions(55, ModContent.ItemType<ClatterboneBreastplate>(), ModContent.ItemType<ClatterboneFaceplate>(), ModContent.ItemType<ClatterboneLeggings>()));

			LeadingConditionRule snow = new LeadingConditionRule(new DropRuleConditions.InBiome(DropRuleConditions.InBiome.Biome.Snow));
			snow.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.IceSculpture.IceWheezerSculpture>(), 100));
			npcLoot.Add(snow);
		}

		int frame = 0;
		int timer = 0;
		int shootTimer = 0;

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
			Player target = Main.player[NPC.target];
			int distance = (int)Math.Sqrt((NPC.Center.X - target.Center.X) * (NPC.Center.X - target.Center.X) + (NPC.Center.Y - target.Center.Y) * (NPC.Center.Y - target.Center.Y));
			if (distance < 200) {
				NPC.velocity = Vector2.Zero;
				if (NPC.velocity == Vector2.Zero) {
					NPC.velocity.X = .008f * NPC.direction;
					NPC.velocity.Y = 12f;
				}
				shootTimer++;
				if (shootTimer >= 80) {
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						SoundEngine.PlaySound(SoundID.Item95, NPC.Center);
						Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
						direction.Normalize();
						direction.X *= 5f;
						direction.Y *= 5f;

						float A = Main.rand.Next(-50, 50) * 0.02f;
						float B = Main.rand.Next(-50, 50) * 0.02f;
						int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + (NPC.direction * 20), NPC.Center.Y - 10, direction.X + A, direction.Y + B, ModContent.ProjectileType<WheezerCloud>(), NPC.damage / 3, 1, Main.myPlayer, 0, 0);
						for (int k = 0; k < 11; k++)
							Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, direction.X + A, direction.Y + B, 0, default, .61f);
						Main.projectile[p].hostile = true;
					}
					shootTimer = 0;
				}
				timer++;
				if (timer == 4) {
					frame++;
					timer = 0;
				}
				if (frame >= 15)
					frame = 11;
			}
			else {
				shootTimer = 0;
				timer++;
				if (timer == 4) {
					frame++;
					timer = 0;
				}
				if (frame >= 9)
					frame = 1;
			}
			if (shootTimer > 120) {
				shootTimer = 120;
			}
			if (shootTimer < 0) {
				shootTimer = 0;
			}
		}

		public override void FindFrame(int frameHeight) => NPC.frame.Y = frameHeight * frame;
	}
}
