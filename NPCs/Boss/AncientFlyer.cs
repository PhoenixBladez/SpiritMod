using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.AvianDrops.ApostleArmor;
using SpiritMod.Items.Sets.AvianDrops;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using SpiritMod.Items.Placeable.MusicBox;
using SpiritMod.Items.Consumable;

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
			Main.npcFrameCount[npc.type] = 6;
			NPCID.Sets.TrailCacheLength[npc.type] = 3;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 148;
			npc.height = 120;
			npc.damage = 23;
			npc.defense = 14;
			npc.value = 33000;
			npc.lifeMax = 3100;
			npc.knockBackResist = 0;
			npc.boss = true;
			npc.noGravity = true;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/AncientAvian");
			npc.noTileCollide = true;
			npc.npcSlots = 15;
			bossBag = ModContent.ItemType<FlyerBag>();
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.scale = 1.1f;
		}

		bool displayCircle = false;
		float frameNum = .2f;

		public override void AI()
		{
			npc.spriteDirection = -npc.direction;
			npc.rotation = npc.velocity.X * 0.07f;

			Player player = Main.player[npc.target];
			MyPlayer modPlayer = player.GetSpiritPlayer();

			if (npc.Center.X >= player.Center.X && moveSpeed >= -120) // flies to players x position
				moveSpeed--;
			else if (npc.Center.X <= player.Center.X && moveSpeed <= 120)
				moveSpeed++;

			npc.velocity.X = moveSpeed * 0.10f;

			if (npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -30) //Flies to players Y position
			{
				moveSpeedY--;
				HomeY = 350f;
			}
			else if (npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 30)
				moveSpeedY++;

			npc.velocity.Y = moveSpeedY * 0.13f;

			timer++;

			if (timer == 200 || timer == 400 && npc.life >= (npc.lifeMax / 2)) //Fires desert feathers like a shotgun
			{
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 73);

				Main.PlaySound(SoundID.NPCHit, (int)npc.position.X, (int)npc.position.Y, 2);
				Main.PlaySound(SoundID.DD2_LightningBugZap, npc.position);

				Vector2 direction = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 8.5f;
				int amountOfProjectiles = Main.rand.Next(8, 11);

				for (int i = 0; i < amountOfProjectiles; ++i)
				{
					float offsetX = Main.rand.Next(-200, 200) * 0.01f;
					float offsetY = Main.rand.Next(-200, 200) * 0.01f;
					int damage = Main.expertMode ? 15 : 17;
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + offsetX, direction.Y + offsetY, ModContent.ProjectileType<DesertFeather>(), damage, 1, Main.myPlayer, 0, 0);
				}
			}
			else if (timer == 300 || timer == 400 || timer == 500 || timer == 550)
			{
				if (Main.expertMode && npc.life >= (npc.lifeMax / 2))
				{
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 8);

					Main.PlaySound(SoundID.NPCHit, (int)npc.position.X, (int)npc.position.Y, 2);
					Main.PlaySound(SoundID.DD2_LightningBugZap, npc.position);

					Vector2 direction = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 11.5f;

					int amountOfProjectiles = Main.rand.Next(5, 9);
					for (int i = 0; i < amountOfProjectiles; ++i)
					{
						float offsetX = Main.rand.Next(-300, 300) * 0.01f;
						float offsetY = Main.rand.Next(-300, 300) * 0.01f;
						int damage = Main.expertMode ? 18 : 20;
						if (Main.netMode != NetmodeID.MultiplayerClient)
							Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + offsetX, direction.Y + offsetY, ModContent.ProjectileType<ExplodingFeather>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}
			}
			else if (timer == 600 || timer == 650 || timer == 700 || timer == 800 || timer == 850 || timer == 880) // Fires bone waves
			{
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 8);
				Main.PlayTrackedSound(SoundID.DD2_EtherianPortalSpawnEnemy, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 direction = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 14;
					int damage = Main.expertMode ? 15 : 19;
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X, direction.Y, ModContent.ProjectileType<BoneWave>(), damage, 1, Main.myPlayer, 0, 0);
				}
			}

			if (timer == 500 || timer == 700)
				HomeY = -35f;
			else if (timer == 900)
				Main.PlaySound(SoundLoader.customSoundType, player.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/AvianScreech"));
			else if ((timer >= 900 && timer <= 1400)) //Rains red comets
			{
				npc.defense = 26;

				if (Main.expertMode)
				{
					player.AddBuff(BuffID.WindPushed, 90);
					modPlayer.windEffect = true;
				}

				frameNum = .4f;
				npc.velocity = Vector2.Zero;

				if (npc.life >= 1000)
				{
					if (Main.rand.Next(8) == 0 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						int offsetX = Main.rand.Next(-200, 200) * 6;
						int damage = Main.expertMode ? 18 : 22;
						Projectile.NewProjectile(player.Center.X + offsetX, player.Center.Y - 1200, 0f, 10f, ModContent.ProjectileType<RedComet>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}
				else
				{
					if (Main.rand.Next(9) == 0 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						int offsetX = 1000 * (int)(Main.windSpeed / Main.windSpeed);
						int offsetY = Main.rand.Next(-460, 460);
						int damage = Main.expertMode ? 18 : 22;
						Projectile.NewProjectile(player.Center.X - offsetX, player.Center.Y + offsetY, 10f, 0f, ModContent.ProjectileType<RedComet>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}
				displayCircle = true;
			}
			else
			{
				npc.defense = 14;
				frameNum = .2f;
				displayCircle = false;
				modPlayer.windEffect = false;
			}

			if (timer >= 1400)
				timer = 0;

			if (npc.life <= npc.lifeMax * .6f) //Fires comets when low on health in expert
			{
				if (Main.expertMode)
				{
					modPlayer.windEffect = true;
					if (Main.rand.Next(22) == 0 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						int offsetX = Main.rand.Next(-2500, 2500) * 2;
						int offsetY = Main.rand.Next(-1000, 1000) - 700;
						int damage = Main.expertMode ? 15 : 17;
						Projectile.NewProjectile(player.Center.X + offsetX, player.Center.Y + offsetY, 0f, 10f, ModContent.ProjectileType<RedComet>(), damage, 1, Main.myPlayer, 0, 0);
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

						if (npc.life <= 1000)
						{
							SilverDust(player, new Vector2(500, 10));
							SilverDust(player, new Vector2(500, 10));
						}
					}
				}
				if (timer == 120)
				{
					Main.PlaySound(SoundID.Item82, new Vector2(tornado.X, tornado.Y));
					Main.PlaySound(SoundID.Item82, new Vector2(tornado.X - 260, tornado.Y - 400));
					Main.PlaySound(SoundID.Item82, new Vector2(tornado.X + 200, tornado.Y - 400));

					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int damage = Main.expertMode ? 16 : 20;
						Projectile.NewProjectile(tornado.X, tornado.Y - 32, 0f, 0f, ModContent.ProjectileType<AvianNado>(), damage, 1, Main.myPlayer, 0, 0);
						Projectile.NewProjectile(tornado.X - 360, tornado.Y - 400, -5f, 0f, ModContent.ProjectileType<AvianNado>(), damage, 1, Main.myPlayer, 0, 0);
						Projectile.NewProjectile(tornado.X + 300, tornado.Y - 400, 5f, 0f, ModContent.ProjectileType<AvianNado>(), damage, 1, Main.myPlayer, 0, 0);
						if (npc.life <= 1000)
						{
							Projectile.NewProjectile(tornado.X - 500, tornado.Y - 32, -5f, 0f, ModContent.ProjectileType<AvianNado>(), damage, 1, Main.myPlayer, 0, 0);
							Projectile.NewProjectile(tornado.X + 500, tornado.Y - 32, 5f, 0f, ModContent.ProjectileType<AvianNado>(), damage, 1, Main.myPlayer, 0, 0);
						}
					}
				}
			}

			if (!player.active || player.dead) //despawns when player is ded
			{
				npc.TargetClosest(false);
				npc.velocity.Y = -50;
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

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => npc.lifeMax = (int)(npc.lifeMax * 0.8f * bossLifeScale);

		public override bool PreNPCLoot()
		{
			MyWorld.downedAncientFlier = true;
			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.WorldData);

			npc.PlayDeathSound("AvianDeathSound");
			return true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var effects = npc.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			var drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
			if (npc.velocity != Vector2.Zero)
			{
				for (int k = 0; k < npc.oldPos.Length; k++)
				{
					Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
					Color color = npc.GetAlpha(lightColor) * ((npc.oldPos.Length - k) / (float)npc.oldPos.Length / 2);
					spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
				}
			}

			if (displayCircle)
			{
				drawOrigin.Y += 30f;
				drawOrigin.Y += 8f;
				--drawOrigin.X;

				Vector2 position1 = npc.Bottom - Main.screenPosition;

				Texture2D glowMask = Main.glowMaskTexture[239];
				Rectangle r2 = glowMask.Frame(1, 1, 0, 0);
				drawOrigin = r2.Size() / 2f;
				Vector2 position3 = position1 + new Vector2(0.0f, -40f);

				Color drawColor = new Color(252, 3, 50) * 1.6f;

				for (int i = 0; i < 3; ++i)
					Main.spriteBatch.Draw(glowMask, position3, r2, drawColor, npc.rotation, drawOrigin, npc.scale * 0.75f, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
			}
			return true;
		}

		public override void NPCLoot()
		{
			if (Main.expertMode)
			{
				npc.DropBossBags();
				return;
			}

			int[] lootTable = {
				ModContent.ItemType<TalonBlade>(),
				ModContent.ItemType<Talonginus>(),
				ModContent.ItemType<SoaringScapula>(),
				ModContent.ItemType<TalonPiercer>(),
				ModContent.ItemType<SkeletalonStaff>()
			};

			int[] lootTable2 = {
				ModContent.ItemType<TalonHeaddress>(),
				ModContent.ItemType<TalonGarb>()
			};

			npc.DropItem(lootTable[Main.rand.Next(lootTable.Length)]);
			npc.DropItem(lootTable2[Main.rand.Next(lootTable2.Length)]);

			npc.DropItem(ModContent.ItemType<FlierMask>(), 1f / 7);
			npc.DropItem(ModContent.ItemType<Trophy2>(), 1f / 10);
		}


		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Stone, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.2f, .8f));
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Stone, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			if (npc.life <= 0)
			{
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(42, 39));

				for (int i = 1; i < 5; ++i)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/AncientAvianGore" + i), 1f);
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += frameNum;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			npc.frame.Y = (int)npc.frameCounter * frameHeight;
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
			spawnInfo =
				$"Use a[i:{ModContent.ItemType<JewelCrown>()}] in the sky biome at any time.Alternatively, smash a Giant Avian Egg, which is found on Avian Islands near the sky layer.The Ancient Avian can be fought at any time and any place in progression.";
			texture = "SpiritMod/Textures/BossChecklist/AvianTexture";
			headTextureOverride = "SpiritMod/NPCs/Boss/AncientFlyer_Head_Boss";
		}
	}
}