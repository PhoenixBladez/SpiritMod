using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.MarbleSet;
using SpiritMod.Items.Weapon.Yoyo;
using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;
using SpiritMod.Utilities;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Beholder
{
	[AutoloadBossHead]
	public class Beholder : ModNPC, IBCRegistrable
	{
		public int dashTimer;
		int frame = 0;
		int shootTimer = 0;
		bool manaSteal = false;
		int manaStealTimer;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beholder");
			Main.npcFrameCount[NPC.type] = 9;
			NPCID.Sets.TrailCacheLength[NPC.type] = 3;
			NPCID.Sets.TrailingMode[NPC.type] = 0;

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0) { Rotation = MathHelper.PiOver2 };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults()
		{
			NPC.width = 72;
			NPC.height = 68;
			NPC.damage = 33;
			NPC.defense = 15;
			NPC.lifeMax = 490;
			NPC.HitSound = SoundID.DD2_CrystalCartImpact;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.value = 760f;
			NPC.knockBackResist = 0.35f;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.aiStyle = 14;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.rarity = 3;
			AIType = NPCID.Slimer;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.BeholderBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Marble,
				new FlavorTextBestiaryInfoElement("These beasts roam the marble pits found within the world, judging you with each one of their many eyes."),
			});
		}

		public override bool PreAI()
		{
			Player player = Main.player[NPC.target];
			float num5 = NPC.position.X + (float)(NPC.width / 2) - player.position.X - (float)(player.width / 2);
			float num6 = NPC.position.Y + (float)NPC.height - 59f - player.position.Y - (float)(player.height / 2);
			float num7 = (float)Math.Atan2((double)num6, (double)num5) + 1.57f;

			if (num7 < 0f)
				num7 += 6.283f;
			else if ((double)num7 > 6.283)
				num7 -= 6.283f;

			const float RotationSpeed = 0.1f;

			if (NPC.rotation < num7)
			{
				if (num7 - NPC.rotation > 3.1415)
					NPC.rotation -= RotationSpeed;
				else
					NPC.rotation += RotationSpeed;
			}
			else if (NPC.rotation > num7)
			{
				if (NPC.rotation - num7 > 3.1415)
					NPC.rotation += RotationSpeed;
				else
					NPC.rotation -= RotationSpeed;
			}

			if (NPC.rotation > num7 - RotationSpeed && NPC.rotation < num7 + RotationSpeed)
				NPC.rotation = num7;

			if (NPC.rotation < 0f)
				NPC.rotation += 6.283f;

			else if (NPC.rotation > 6.283)
				NPC.rotation -= 6.283f;

			if (NPC.rotation > num7 - RotationSpeed && NPC.rotation < num7 + RotationSpeed)
				NPC.rotation = num7;
			NPC.spriteDirection = NPC.direction;
			return true;
		}

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
			Player player = Main.player[NPC.target];

			NPC.TargetClosest(true);
			dashTimer++;
			if (dashTimer == 210 || dashTimer == 420 || dashTimer == 630)
			{
				Vector2 direction = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center);
				SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, NPC.Center);
				direction.X *= Main.rand.Next(6, 9);
				direction.Y *= Main.rand.Next(6, 9);
				NPC.velocity = direction * 0.98f;
			}

			if (dashTimer == 770 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				SoundEngine.PlaySound(SoundID.DD2_WitherBeastAuraPulse, NPC.Center);
				NPC.position.X = player.position.X + Main.rand.NextFloat(-200f, 200f);
				NPC.position.Y = player.position.Y + Main.rand.NextFloat(-100f, -200f);

				NPC.netUpdate = true;
			}

			if (dashTimer == 771)
			{
				for (int i = 0; i < 30; i++)
				{
					Vector2 vector23 = Vector2.UnitY.RotatedByRandom(6.28318548202515) * new Vector2(100f, 20f) * NPC.scale * 1.85f / 2f;
					int index1 = Dust.NewDust(NPC.Center + vector23, 0, 0, DustID.GoldCoin, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index1].position = NPC.Center + vector23;
					Main.dust[index1].velocity = Vector2.Zero;
					Vector2 vector25 = Vector2.UnitY.RotatedByRandom(6.28318548202515) * new Vector2(20f, 100f) * NPC.scale * 1.85f / 2f;
					int index3 = Dust.NewDust(NPC.Center + vector25, 0, 0, DustID.GoldCoin, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index3].position = NPC.Center + vector25;
					Main.dust[index3].velocity = Vector2.Zero;
				}
			}

			if (dashTimer == 800)
				SoundEngine.PlaySound(SoundID.DD2_WyvernScream, NPC.Center);

			if (dashTimer >= 800 && dashTimer <= 1000)
			{
				frame = 8;
				NPC.velocity.X *= .008f * NPC.direction;
				NPC.velocity.Y *= 0f;
				shootTimer++;
				if (shootTimer >= 40 && Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 direction = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 5f;
					shootTimer = 0;
					int amountOfProjectiles = 1;
					bool expertMode = Main.expertMode;
					int damage = expertMode ? 11 : 16;
					for (int i = 0; i < amountOfProjectiles; ++i)
					{
						float A = (float)Main.rand.Next(-50, 50) * 0.02f;
						float B = (float)Main.rand.Next(-50, 50) * 0.02f;
						int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + (NPC.direction * 12), NPC.Center.Y + 20, direction.X + A, direction.Y + B, ProjectileID.Fireball, damage, 1, Main.myPlayer, 0, 0);
						for (int k = 0; k < 11; k++)
							Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, direction.X + A, direction.Y + B, 0, default, .61f);
						Main.projectile[p].hostile = true;
					}
				}
			}
			else
				shootTimer = 0;

			if (dashTimer >= 1020)
				dashTimer = 0;

			if (manaSteal)
			{
				manaStealTimer++;
				int distance = (int)Math.Sqrt((NPC.Center.X - player.Center.X) * (NPC.Center.X - player.Center.X) + (NPC.Center.Y - player.Center.Y) * (NPC.Center.Y - player.Center.Y));
				if (distance < 300 && player.statMana > 0)
				{
					player.statMana--;
					for (int i = 0; i < 2; i++)
					{
						int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.PurificationPowder);
						Main.dust[dust].velocity *= -1f;
						Main.dust[dust].scale *= 1.4f;
						Main.dust[dust].noGravity = true;
						Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
						vector2_1.Normalize();
						Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
						Main.dust[dust].velocity = vector2_2;
						vector2_2.Normalize();
						Vector2 vector2_3 = vector2_2 * 64f;
						Main.dust[dust].position = NPC.Center - vector2_3;
					}
				}
			}
			else
				manaStealTimer = 0;

			if (manaStealTimer >= 120)
			{
				manaSteal = false;
				manaStealTimer = 0;
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(dashTimer);
			writer.Write(frame);
			writer.Write(shootTimer);
			writer.Write(manaSteal);
			writer.Write(manaStealTimer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			dashTimer = reader.ReadInt32();
			frame = reader.ReadInt32();
			shootTimer = reader.ReadInt32();
			manaSteal = reader.ReadBoolean();
			manaStealTimer = reader.ReadInt32();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.AnyNPCs(ModContent.NPCType<Beholder>()) || !spawnInfo.Player.GetSpiritPlayer().ZoneMarble || !NPC.downedBoss2 || spawnInfo.SpawnTileY <= Main.rockLayer)
				return 0;
			if (QuestManager.GetQuest<SlayerQuestMarble>().IsActive)
				return 0.15f;
			return 0.002765f;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;
			if (NPC.frameCounter >= 6)
			{
				frame++;
				NPC.frameCounter = 0;
			}

			if (frame >= 7)
				frame = 0;

			NPC.frame.Y = frameHeight * frame;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height * 0.5f));
			for (int k = 0; k < NPC.oldPos.Length; k++)
			{
				var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
				Color color = NPC.GetAlpha(drawColor) * (float)(((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
				spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
			}
			return true;
		}

		public override bool PreKill()
		{
			SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/DownedMiniboss"), NPC.position);
			MyWorld.downedBeholder = true;
			return true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				SoundEngine.PlaySound(SoundID.DD2_WyvernScream, NPC.Center);

				for (int i = 1; i < 9; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Beholder" + i).Type, 1f);
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.NextBool(4))
				target.AddBuff(BuffID.Bleeding, 180);
			manaSteal = true;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MarbleChunk>(), 1, 5, 7));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BeholderYoyo>(), 2));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TofuSatay>(), 5));
		}

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Miniboss;
			progression = 3.2f;
			name = "Beholder";
			downedCondition = () => MyWorld.downedBeholder;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> {
					ModContent.NPCType<Beholder>()
				},
				null,
				null,
				new List<int> {
					ModContent.ItemType<BeholderYoyo>(),
					ModContent.ItemType<MarbleChunk>()
				});
			spawnInfo =
				"The Beholder spawns rarely in Marble Caverns after the Eater of Worlds or Brain of Cthulhu has been defeated.";
			texture = "SpiritMod/Textures/BossChecklist/BeholderTexture";
			headTextureOverride = "SpiritMod/NPCs/Beholder_Head_Boss";
		}
	}
}