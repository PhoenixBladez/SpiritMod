using Microsoft.Xna.Framework;
using SpiritMod.Items.Consumable;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs;
using SpiritMod.Buffs.DoT;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.AncientApostle
{
	public class BoneHarpy : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Apostle");
			Main.npcFrameCount[NPC.type] = 7;
		}

		public override void SetDefaults()
		{
			NPC.width = 32;
			NPC.height = 34;
			NPC.damage = 16;
			NPC.defense = 10;
			NPC.lifeMax = 70;
			NPC.noGravity = true;
			NPC.value = 90f;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			NPC.buffImmune[ModContent.BuffType<BloodCorrupt>()] = true;
			NPC.buffImmune[ModContent.BuffType<BloodInfusion>()] = true;
			NPC.noTileCollide = false;
			NPC.HitSound = SoundID.NPCHit2;
			NPC.DeathSound = SoundID.NPCDeath6;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.AncientApostleBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
				new FlavorTextBestiaryInfoElement("The reanimated bones of a long extinct bird. Though dead, don’t lower your guard near them, for these fowl fossils still have fight in them!"),
			});
		}

		private ref float MoveSpeed => ref NPC.ai[1];
		private ref float MoveSpeedY => ref NPC.ai[2];
		private ref float Counter => ref NPC.ai[3];

		public override void AI()
		{
			if (Counter == 0)
				NPC.ai[0] = 150;

			Counter++;
			Player player = Main.player[NPC.target];
			NPC.rotation = NPC.velocity.X * 0.1f;
			NPC.spriteDirection = NPC.direction;

			if (NPC.Center.X >= player.Center.X && MoveSpeed >= -60) // flies to players x position
				MoveSpeed--;

			if (NPC.Center.X <= player.Center.X && MoveSpeed <= 60)
				MoveSpeed++;

			NPC.velocity.X = MoveSpeed * 0.06f;

			if (NPC.Center.Y >= player.Center.Y - NPC.ai[0] && MoveSpeedY >= -50) //Flies to players Y position
			{
				MoveSpeedY--;
				NPC.ai[0] = 150f;
			}

			if (NPC.Center.Y <= player.Center.Y - NPC.ai[0] && MoveSpeedY <= 50)
				MoveSpeedY++;

			NPC.velocity.Y = MoveSpeedY * 0.12f;
			if (Main.rand.Next(220) == 8 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				NPC.ai[0] = -25f;
				NPC.netUpdate = true;
			}

			if (Counter >= 240) //Fires desert feathers like a shotgun
			{
				Counter = 0;
				SoundEngine.PlaySound(SoundID.Item73, NPC.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
					direction.Normalize();
					direction.X *= 11f;
					direction.Y *= 11f;

					int amountOfProjectiles = 3;
					for (int i = 0; i < amountOfProjectiles; ++i)
					{
						float A = (float)Main.rand.Next(-150, 150) * 0.01f;
						float B = (float)Main.rand.Next(-150, 150) * 0.01f;
						int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<NPCs.Boss.DesertFeather>(), 11, 1, Main.myPlayer, 0, 0);
						Main.projectile[p].scale = .6f;
					}
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.SpawnTileY > Main.rockLayer && !Main.LocalPlayer.GetSpiritPlayer().ZoneAsteroid ? 0.16f : 0f;

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<JewelCrown>(), 6));
			npcLoot.Add(ItemDropRule.Common(ItemID.Feather, 2));
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Stone, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.2f, .8f));
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Stone, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			if (NPC.life <= 0)
			{
				for (int i = 1; i < 6; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Apostle" + i).Type, 1f);
			}
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.21f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}
	}
}