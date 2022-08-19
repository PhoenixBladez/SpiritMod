using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Items.BossLoot.AvianDrops.ApostleArmor;
using SpiritMod.Items.BossLoot.AvianDrops;
using SpiritMod.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using SpiritMod.Items.Placeable.MusicBox;
using SpiritMod.Items.Consumable;
using SpiritMod.Buffs.DoT;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Boss
{
	[AutoloadBossHead]
	public class AncientFlyer : ModNPC, IBCRegistrable
	{
		private Point tornado = new Point();

		int timer = 0;
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 330f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Avian");
			Main.npcFrameCount[NPC.type] = 6;
			NPCID.Sets.TrailCacheLength[NPC.type] = 3;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.width = 148;
			NPC.height = 120;
			NPC.damage = 23;
			NPC.defense = 14;
			NPC.value = 33000;
			NPC.lifeMax = 3100;
			NPC.knockBackResist = 0;
			NPC.boss = true;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			NPC.buffImmune[ModContent.BuffType<BloodCorrupt>()] = true;
			NPC.buffImmune[ModContent.BuffType<BloodInfusion>()] = true;

			NPC.noGravity = true;
			Music = MusicLoader.GetMusicSlot(Mod,"Sounds/Music/AncientAvian");
			NPC.noTileCollide = true;
			NPC.npcSlots = 15;
			NPC.HitSound = SoundID.NPCHit2;
			NPC.DeathSound = SoundID.NPCDeath5;
			NPC.scale = 1.1f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
				new FlavorTextBestiaryInfoElement("The matriarch of an extinct avian predator species, brought back to life through sheer will to protect the yolk of its last remaining spawn."),
			});
		}

		bool displayCircle = false;
		float frameNum = .2f;

		public override void AI()
		{
			NPC.spriteDirection = -NPC.direction;
			NPC.rotation = NPC.velocity.X * 0.07f;

			Player player = Main.player[NPC.target];
			MyPlayer modPlayer = player.GetSpiritPlayer();

			if (NPC.Center.X >= player.Center.X && moveSpeed >= -120) // flies to players x position
				moveSpeed--;
			else if (NPC.Center.X <= player.Center.X && moveSpeed <= 120)
				moveSpeed++;

			NPC.velocity.X = moveSpeed * 0.10f;

			if (NPC.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -30) //Flies to players Y position
			{
				moveSpeedY--;
				HomeY = 350f;
			}
			else if (NPC.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 30)
				moveSpeedY++;

			NPC.velocity.Y = moveSpeedY * 0.13f;

			timer++;

			if (timer == 200 || timer == 400 && NPC.life >= (NPC.lifeMax / 2)) //Fires desert feathers like a shotgun
			{
				SoundEngine.PlaySound(SoundID.Item73, NPC.Center);
				SoundEngine.PlaySound(SoundID.NPCHit2, NPC.Center);
				SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, NPC.position);

				Vector2 direction = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 8.5f;
				int amountOfProjectiles = Main.rand.Next(8, 11);

				for (int i = 0; i < amountOfProjectiles; ++i)
				{
					float offsetX = Main.rand.Next(-200, 200) * 0.01f;
					float offsetY = Main.rand.Next(-200, 200) * 0.01f;
					int damage = Main.expertMode ? 15 : 17;
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + offsetX, direction.Y + offsetY, ModContent.ProjectileType<DesertFeather>(), damage, 1, Main.myPlayer, 0, 0);
				}
			}
			else if (timer == 300 || timer == 400 || timer == 500 || timer == 550)
			{
				if (Main.expertMode && NPC.life >= (NPC.lifeMax / 2))
				{
					SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
					SoundEngine.PlaySound(SoundID.NPCHit2, NPC.Center);
					SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, NPC.position);

					Vector2 direction = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 11.5f;

					int amountOfProjectiles = Main.rand.Next(5, 9);
					for (int i = 0; i < amountOfProjectiles; ++i)
					{
						float offsetX = Main.rand.Next(-300, 300) * 0.01f;
						float offsetY = Main.rand.Next(-300, 300) * 0.01f;
						int damage = Main.expertMode ? 18 : 20;
						if (Main.netMode != NetmodeID.MultiplayerClient)
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + offsetX, direction.Y + offsetY, ModContent.ProjectileType<ExplodingFeather>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}
			}
			else if (timer == 600 || timer == 650 || timer == 700 || timer == 800 || timer == 850 || timer == 880) // Fires bone waves
			{
				SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
				SoundEngine.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy, NPC.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 direction = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 14;
					int damage = Main.expertMode ? 15 : 19;
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X, direction.Y, ModContent.ProjectileType<BoneWave>(), damage, 1, Main.myPlayer, 0, 0);
				}
			}

			if (timer == 500 || timer == 700)
				HomeY = -35f;
			else if (timer == 900)
				SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/AvianScreech"), player.position);
			else if ((timer >= 900 && timer <= 1400)) //Rains red comets
			{
				NPC.defense = 26;

				if (Main.expertMode)
				{
					player.AddBuff(BuffID.WindPushed, 90);
					modPlayer.windEffect = true;
				}

				frameNum = .4f;
				NPC.velocity = Vector2.Zero;

				if (NPC.life >= 1000)
				{
					if (Main.rand.NextBool(8) && Main.netMode != NetmodeID.MultiplayerClient)
					{
						int offsetX = Main.rand.Next(-200, 200) * 6;
						int damage = Main.expertMode ? 18 : 22;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X + offsetX, player.Center.Y - 1200, 0f, 10f, ModContent.ProjectileType<RedComet>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}
				else
				{
					if (Main.rand.Next(9) == 0 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						int offsetX = 1000 * (int)(Main.windSpeedCurrent / Main.windSpeedCurrent);
						int offsetY = Main.rand.Next(-460, 460);
						int damage = Main.expertMode ? 18 : 22;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X - offsetX, player.Center.Y + offsetY, 10f, 0f, ModContent.ProjectileType<RedComet>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}
				displayCircle = true;
			}
			else
			{
				NPC.defense = 14;
				frameNum = .2f;
				displayCircle = false;
				modPlayer.windEffect = false;
			}

			if (timer >= 1400)
				timer = 0;

			if (NPC.life <= NPC.lifeMax * .6f) //Fires comets when low on health in expert
			{
				if (Main.expertMode)
				{
					modPlayer.windEffect = true;
					if (Main.rand.Next(22) == 0 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						int offsetX = Main.rand.Next(-2500, 2500) * 2;
						int offsetY = Main.rand.Next(-1000, 1000) - 700;
						int damage = Main.expertMode ? 15 : 17;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X + offsetX, player.Center.Y + offsetY, 0f, 10f, ModContent.ProjectileType<RedComet>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}
				if (timer == 60)
				{
					tornado.X = (int)player.Center.X;
					tornado.Y = (int)player.Center.Y;
				}
				if (timer > 60 && timer < 120)
				{
					for (int k = 0; k < 3; k++)
					{
						SilverDust(player, new Vector2(50, 10));
						SilverDust(player, new Vector2(360, 250));
						SilverDust(player, new Vector2(300, 250));

						if (NPC.life <= 1000)
						{
							SilverDust(player, new Vector2(500, 10));
							SilverDust(player, new Vector2(500, 10));
						}
					}
				}
				if (timer == 120)
				{
					SoundEngine.PlaySound(SoundID.Item82, new Vector2(tornado.X, tornado.Y));
					SoundEngine.PlaySound(SoundID.Item82, new Vector2(tornado.X - 260, tornado.Y - 400));
					SoundEngine.PlaySound(SoundID.Item82, new Vector2(tornado.X + 200, tornado.Y - 400));

					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int damage = Main.expertMode ? 16 : 20;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), tornado.X, tornado.Y - 32, 0f, 0f, ModContent.ProjectileType<AvianNado>(), damage, 1, Main.myPlayer, 0, 0);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), tornado.X - 360, tornado.Y - 400, -5f, 0f, ModContent.ProjectileType<AvianNado>(), damage, 1, Main.myPlayer, 0, 0);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), tornado.X + 300, tornado.Y - 400, 5f, 0f, ModContent.ProjectileType<AvianNado>(), damage, 1, Main.myPlayer, 0, 0);
						if (NPC.life <= 1000)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), tornado.X - 500, tornado.Y - 32, -5f, 0f, ModContent.ProjectileType<AvianNado>(), damage, 1, Main.myPlayer, 0, 0);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), tornado.X + 500, tornado.Y - 32, 5f, 0f, ModContent.ProjectileType<AvianNado>(), damage, 1, Main.myPlayer, 0, 0);
						}
					}
				}
			}

			if (!player.active || player.dead) //despawns when player is ded
			{
				NPC.TargetClosest(false);
				NPC.velocity.Y = -50;
				timer = 0;
			}
		}

		private void SilverDust(Player player, Vector2 offset)
		{
			int dust = Dust.NewDust(new Vector2(tornado.X - offset.X, tornado.Y + player.height - offset.Y), 50, 10, DustID.SilverCoin, 0, -15);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = true;
			Main.dust[dust].scale = .85f;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * bossLifeScale);

		public override bool PreKill()
		{
			MyWorld.downedAncientFlier = true;
			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.WorldData);

			NPC.PlayDeathSound("AvianDeathSound");
			return true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			var drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height / Main.npcFrameCount[NPC.type]) * 0.5f);
			if (NPC.velocity != Vector2.Zero)
			{
				for (int k = 0; k < NPC.oldPos.Length; k++)
				{
					Vector2 drawPos = NPC.oldPos[k] - screenPos + drawOrigin + new Vector2(0f, NPC.gfxOffY);
					Color color = NPC.GetAlpha(drawColor) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length / 2);
					spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
				}
			}

			if (displayCircle)
			{
				drawOrigin.Y += 30f;
				drawOrigin.Y += 8f;
				--drawOrigin.X;

				Vector2 position1 = NPC.Bottom - screenPos;

				Texture2D glowMask = TextureAssets.GlowMask[239].Value;
				Rectangle r2 = glowMask.Frame(1, 1, 0, 0);
				drawOrigin = r2.Size() / 2f;
				Vector2 position3 = position1 + new Vector2(0.0f, -40f);

				Color drawCol = new Color(252, 3, 50) * 1.6f;

				for (int i = 0; i < 3; ++i)
					Main.spriteBatch.Draw(glowMask, position3, r2, drawCol, NPC.rotation, drawOrigin, NPC.scale * 0.75f, SpriteEffects.FlipHorizontally, 0.0f);
			}
			return true;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			int[] weapons =
			{
				ModContent.ItemType<TalonBlade>(),
				ModContent.ItemType<Talonginus>(),
				ModContent.ItemType<SoaringScapula>(),
				ModContent.ItemType<TalonPiercer>(),
				ModContent.ItemType<SkeletalonStaff>()
			};

			int[] gear =
			{
				ModContent.ItemType<TalonHeaddress>(),
				ModContent.ItemType<TalonGarb>()
			};

			npcLoot.Add(ItemDropRule.OneFromOptions(1, weapons));
			npcLoot.Add(ItemDropRule.OneFromOptions(1, gear));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FlierMask>(), 7));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Trophy2>(), 10));
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<FlyerBag>()));
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
				SoundEngine.PlaySound(SoundID.DD2_BetsyHurt);

				for (int i = 1; i < 5; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AncientAvianGore" + i).Type, 1f);
			}
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += frameNum;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			NPC.frame.Y = (int)NPC.frameCounter * frameHeight;
		}

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Boss;
			progression = 4.2f;
			name = "Ancient Avian";
			downedCondition = () => MyWorld.downedAncientFlier;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> {
					ModContent.NPCType<AncientFlyer>()
				},
				new List<int> {
					ModContent.ItemType<JewelCrown>()
				},
				new List<int> {
					ModContent.ItemType<Trophy2>(), ModContent.ItemType<FlierMask>(),
					ModContent.ItemType<AvianBox>()
				},
				new List<int> {
					ModContent.ItemType<AvianHook>(),
					ModContent.ItemType<TalonBlade>(),
					ModContent.ItemType<Talonginus>(),
					ModContent.ItemType<SoaringScapula>(),
					ModContent.ItemType<TalonPiercer>(),
					ModContent.ItemType<SkeletalonStaff>(),
					ModContent.ItemType<TalonHeaddress>(),
					ModContent.ItemType<TalonGarb>(),
					ItemID.LesserHealingPotion
				});
			spawnInfo = $"Use a[i:{ModContent.ItemType<JewelCrown>()}] in the sky biome at any time. Alternatively, smash a Giant Avian Egg, which is found on Avian Islands near the sky layer. The Ancient Avian can be fought at any time and any place in progression.";
			texture = "SpiritMod/Textures/BossChecklist/AvianTexture";
			headTextureOverride = "SpiritMod/NPCs/Boss/AncientFlyer_Head_Boss";
		}
	}
}