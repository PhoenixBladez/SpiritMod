using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;
using SpiritMod.Items.Sets.EvilBiomeDrops.Heartillery;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;

namespace SpiritMod.NPCs.ArterialGrasper
{
	public class CrimsonTrapper : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arterial Grasper");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 34;
			NPC.height = 34;
			NPC.damage = 35;
			NPC.defense = 8;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.lifeMax = 150;
			NPC.noGravity = true;
			NPC.HitSound = SoundID.NPCHit19;
			NPC.DeathSound = SoundID.DD2_BetsyHurt;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.value = 220f;
			NPC.aiStyle = -1;
			NPC.knockBackResist = 0f;
			NPC.behindTiles = true;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.ArterialGrasperBanner>();
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => NPC.lifeMax = 250;

		bool spawnedHooks = false;
		//bool attack = false;
		public override void AI()
		{
			NPC.TargetClosest(false);

			if (NPC.localAI[0] == 0f)
			{
				NPC.localAI[0] = NPC.Center.Y;
				NPC.netUpdate = true; //localAI probably isnt affected by this... buuuut might as well play it safe
			}

			if (NPC.Center.Y >= NPC.localAI[0])
			{
				NPC.localAI[1] = -1f;
				NPC.netUpdate = true;
			}

			if (NPC.Center.Y <= NPC.localAI[0] - 2f)
			{
				NPC.localAI[1] = 1f;
				NPC.netUpdate = true;
			}

			NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y + 0.009f * NPC.localAI[1], -.85f, .85f);

			if (!spawnedHooks)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int i = 0; i < Main.rand.Next(2, 4); i++)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y - 10, Main.rand.Next(-10, 10), -6, ModContent.ProjectileType<TendonEffect>(), 0, 0, Main.myPlayer, 0, NPC.whoAmI);

					for (int i = 0; i < Main.rand.Next(2, 3); i++)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y - 10, Main.rand.Next(-10, 10), -6, ModContent.ProjectileType<TendonEffect1>(), 0, 0, Main.myPlayer, 0, NPC.whoAmI);
				}

				spawnedHooks = true;
				NPC.netUpdate = true;
			}

			NPC.spriteDirection = -NPC.direction;
			Player target = Main.player[NPC.target];
			int distance = (int)Math.Sqrt((NPC.Center.X - target.Center.X) * (NPC.Center.X - target.Center.X) + (NPC.Center.Y - target.Center.Y) * (NPC.Center.Y - target.Center.Y));

			if (distance < 560)
			{
				float num395 = Main.mouseTextColor / 200f - 0.35f;
				num395 *= 0.2f;
				NPC.scale = num395 + 0.95f;
				//attack = true;
				NPC.ai[2]++;
				if (NPC.ai[2] == 30 || NPC.ai[2] == 60 || NPC.ai[2] == 90 || NPC.ai[2] == 120 || NPC.ai[2] == 150)
				{
					Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), .153f * 1, .028f * 1, 0.055f * 1);
					SoundEngine.PlaySound(new SoundStyle("SpiritMod/HeartBeatFx"), NPC.Center);
				}
				if (NPC.ai[2] >= 180)
				{
					NPC.ai[2] = 0;
					SoundEngine.PlaySound(SoundID.Item95, NPC.Center);
					SoundEngine.PlaySound(new SoundStyle("SpiritMod/HeartBeatFx"), NPC.Center);
					Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), .153f * 1, .028f * 1, 0.055f * 1);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						for (int i = 0; i < 5; i++)
						{
							float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
							Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
							int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ArterialBloodClump>(), 12, 1, Main.myPlayer, 0, 0);
							Main.projectile[proj].friendly = false;
							Main.projectile[proj].hostile = true;
							Main.projectile[proj].velocity *= 5f;
						}
					}
				}
			}

			if (distance == 580)
				NPC.netUpdate = true;

			if (distance > 580)
			{
				float num395 = Main.mouseTextColor / 200f - 0.35f;
				num395 *= 0.2f;
				NPC.scale = num395 + 0.95f;
				NPC.ai[2]++;

				if (NPC.ai[2] >= 90)
				{
					NPC.ai[2] = 0;
					NPC.netUpdate = true;
					Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), .153f * .5f, .028f * .5f, 0.055f * .5f);
					if (Main.netMode != NetmodeID.Server)
						SoundEngine.PlaySound(new SoundStyle("SpiritMod/HeartBeatFx"), NPC.Center);
				}
			}
		}

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(spawnedHooks);
		public override void ReceiveExtraAI(BinaryReader reader) => spawnedHooks = reader.ReadBoolean();

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			bool wall = Framing.GetTileSafely(spawnInfo.SpawnTileX, spawnInfo.SpawnTileY).WallType > 0;
			bool valid = wall && spawnInfo.Player.ZoneCrimson && (spawnInfo.Player.ZoneRockLayerHeight || spawnInfo.Player.ZoneDirtLayerHeight);
			if (!valid)
				return 0;
			return SpawnCondition.Crimson.Chance * 0.1f;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, Color.Purple, 0.3f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Grasper/Grasper1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Grasper/Grasper2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Grasper/Grasper3").Type, 1f);
			}
		}

		//public override void OnKill()
		//{
		//	if (QuestManager.GetQuest<StylistQuestCrimson>().IsActive)
		//		Item.NewItem(NPC.Center, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.CrimsonDyeMaterial>());
		//}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Vertebrae, 3));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeartilleryBeacon>(), 33));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Meatballs>(), 16));
		}
	}
}