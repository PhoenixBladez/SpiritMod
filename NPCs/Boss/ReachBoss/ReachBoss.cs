using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.VinewrathDrops;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Placeable.MusicBox;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.ReachBoss
{
	[AutoloadBossHead]
	public class ReachBoss : ModNPC, IBCRegistrable
	{
		int moveSpeed = 0;
		bool text = false;
		int moveSpeedY = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vinewrath Bane");
			Main.npcFrameCount[NPC.type] = 5;
			NPCID.Sets.TrailCacheLength[NPC.type] = 4;
			NPCID.Sets.TrailingMode[NPC.type] = 1;

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0) { Hide = true };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults()
		{
			NPC.Size = new Vector2(80, 120);
			NPC.damage = 28;
			NPC.boss = true;
			NPC.lifeMax = 3400;
			NPC.knockBackResist = 0;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.npcSlots = 30;
			NPC.defense = 9;
			NPC.aiStyle = -1;
			Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ReachBoss");
			NPC.buffImmune[20] = true;
			NPC.buffImmune[31] = true;
			NPC.buffImmune[70] = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
		}

		bool pulseTrail;
		bool pulseTrailPurple;
		bool pulseTrailYellow;
		bool trailbehind = false;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(pulseTrail);
			writer.Write(pulseTrailPurple);
			writer.Write(pulseTrailYellow);
			writer.Write(trailbehind);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			pulseTrail = reader.ReadBoolean();
			pulseTrailPurple = reader.ReadBoolean();
			pulseTrailYellow = reader.ReadBoolean();
			trailbehind = reader.ReadBoolean();
		}
		public override void AI()
		{
			Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), 0.301f, 0.110f, 0.126f);
			Player player = Main.player[NPC.target];
			NPC.rotation = MathHelper.Lerp(NPC.rotation, 0, 0.06f);
			NPC.noTileCollide = true;

			if (!player.active || player.dead)
			{
				NPC.TargetClosest(false);
				NPC.velocity.Y = -2000;
			}
			if (!player.GetSpiritPlayer().ZoneReach)
			{
				NPC.defense = 25;
				NPC.damage = 45;
			}
			else
			{
				NPC.defense = 9;
				NPC.damage = 28;
			}

			if (NPC.life <= (NPC.lifeMax / 10 * 4) && NPC.ai[3] == 0)
			{
				NPC.ai[0] = 0;
				DustHelper.DrawStar(NPC.Center, 235, pointAmount: 7, mainSize: 2.7425f, dustDensity: 6, dustSize: .65f, pointDepthMult: 3.6f, noGravity: true);
				SoundEngine.PlaySound(SoundID.NPCDeath55 with { PitchVariance = 0.2f }, NPC.Center);
				SoundEngine.PlaySound(SoundID.DD2_WitherBeastDeath, NPC.Center);
				NPC.netUpdate = true;
				NPC.ai[3]++;
			}

			if (NPC.life <= (NPC.lifeMax / 10 * 4))
				NPC.ai[0] += 1.5f;
			else
				NPC.ai[0]++;

			if (NPC.ai[0] < 470 || NPC.ai[0] > 730 && NPC.ai[0] < 900 || NPC.ai[0] > 1051 && NPC.ai[0] < 1120 || NPC.ai[0] > 1750)
				GeneralMovement(player);

			float[] stoptimes = new float[] { 471, 540, 669, 900, 1051 };
			if (stoptimes.Contains(NPC.ai[0]))
			{
				NPC.velocity = Vector2.Zero;
				NPC.netUpdate = true;
			}

			if (NPC.ai[0] >= 480 && NPC.ai[0] < 730)
			{
				SideFloat(player);
				pulseTrail = true;
			}
			else
				pulseTrail = false;

			if (NPC.ai[0] == 880)
			{
				DustHelper.DrawStar(NPC.Center, 272, pointAmount: 8, mainSize: 3.7425f, dustDensity: 6, dustSize: .65f, pointDepthMult: 3.6f, noGravity: true);
				SoundEngine.PlaySound(SoundID.NPCDeath55 with { PitchVariance = 0.2f }, NPC.Center);
			}

			if (NPC.ai[0] > 900 && NPC.ai[0] < 1050)
			{
				pulseTrailPurple = true;
				FlowerAttack(player);
			}
			else
				pulseTrailPurple = false;

			if (NPC.ai[0] >= 1120 && NPC.ai[0] < 1740)
			{
				DashAttack(player);
				pulseTrailYellow = true;
			}
			else
			{
				pulseTrailYellow = false;
				trailbehind = false;
				NPC.TargetClosest(true);
				NPC.spriteDirection = NPC.direction;
			}

			if (NPC.ai[0] > 1800 && NPC.ai[0] < 1970)
			{
				SummonSpores();
				pulseTrailYellow = true;
			}
			else
				pulseTrailYellow = false;

			if (NPC.ai[0] > 2000)
			{
				pulseTrailPurple = false;
				pulseTrailYellow = false;
				pulseTrail = false;
				NPC.ai[0] = 0;
				NPC.ai[1] = 0;
				NPC.ai[2] = 0;
				NPC.netUpdate = true;
			}
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => NPC.ai[0] < 480 || NPC.ai[0] >= 730;

		public void GeneralMovement(Player player)
		{
			float value12 = (float)Math.Cos((double)(Main.GlobalTimeWrappedHourly % 2.4f / 2.4f * 6.28318548f)) * 60f;

			if (NPC.Center.X >= player.Center.X && moveSpeed >= -37) // flies to players x position
				moveSpeed--;
			else if (NPC.Center.X <= player.Center.X && moveSpeed <= 37)
				moveSpeed++;

			NPC.velocity.X = moveSpeed * 0.13f;
			NPC.rotation = NPC.velocity.X * 0.04f;

			if (NPC.Center.Y >= player.Center.Y - 140f + value12 && moveSpeedY >= -20) //Flies to players Y position
				moveSpeedY--;
			else if (NPC.Center.Y <= player.Center.Y - 140f + value12 && moveSpeedY <= 20)
				moveSpeedY++;

			NPC.velocity.Y = moveSpeedY * 0.1f;
		}
		public void SideFloat(Player player)
		{
			bool expertMode = Main.expertMode;
			Vector2 homepos = Main.player[NPC.target].Center;

			if ((NPC.ai[0] >= 480 && NPC.ai[0] < 540) || (NPC.ai[0] >= 580 && NPC.ai[0] < 670))
			{
				homepos += (NPC.ai[0] < 540) ? new Vector2(150, -250f) : new Vector2(-150, -250);
				NPC.TargetClosest(true);
				float vel = MathHelper.Clamp(NPC.Distance(homepos) / 8, 6, 38);
				NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(homepos) * vel, 0.05f);
			}

			if (NPC.ai[0] == 561 || NPC.ai[0] == 690)
			{
				SoundEngine.PlaySound(SoundID.Grass, NPC.Center);
				SoundEngine.PlaySound(SoundID.DD2_WitherBeastDeath, NPC.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
					direction.Normalize();
					direction *= 3f;

					const int AmountOfProjectiles = 5;
					for (int i = 0; i < AmountOfProjectiles; ++i)
					{
						int damage = expertMode ? 14 : 19;

						if (i == 0)
							Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, direction, ModContent.ProjectileType<BossRedSpike>(), damage, 1, Main.myPlayer, 0, 0).netUpdate = true;
						else
							Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, direction.RotatedByRandom(MathHelper.Pi / 4) * Main.rand.NextFloat(0.5f, 1f), ModContent.ProjectileType<BossRedSpike>(), damage, 1, Main.myPlayer, 0, 0).netUpdate = true;
					}
				}
			}
		}
		public void FlowerAttack(Player player)
		{
			bool expertMode = Main.expertMode;
			int damage = expertMode ? 13 : 16;
			if (NPC.ai[0] % 15 == 0)
			{
				SoundEngine.PlaySound(SoundID.Item104 with { PitchVariance = 0.2f }, NPC.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + Main.rand.Next(-60, 60), NPC.Center.Y + Main.rand.Next(-60, 60), Main.rand.NextFloat(-5.3f, 5.3f), Main.rand.NextFloat(-5.3f, 5.3f), ModContent.ProjectileType<ReachBossFlower>(), damage, 1, Main.myPlayer, 0, 0);
					Main.projectile[p].scale = Main.rand.NextFloat(.6f, .8f);
					DustHelper.DrawStar(Main.projectile[p].Center, 272, pointAmount: 6, mainSize: .9425f, dustDensity: 2, dustSize: .5f, pointDepthMult: 0.3f, noGravity: true);

					if (Main.projectile[p].velocity == Vector2.Zero)
						Main.projectile[p].velocity = new Vector2(2.25f, 2.25f);

					if (Main.projectile[p].velocity.X < 2.25f && Math.Sign(Main.projectile[p].velocity.X) == Math.Sign(1) || Main.projectile[p].velocity.X > -2.25f && Math.Sign(Main.projectile[p].velocity.X) == Math.Sign(-1))
						Main.projectile[p].velocity.X *= 2.15f;

					Main.projectile[p].netUpdate = true;
				}
			}
		}

		void DashAttack(Player player) //basically just copy pasted from scarabeus mostly
		{
			pulseTrailYellow = true;
			NPC.direction = Math.Sign(player.Center.X - NPC.Center.X);

			if (NPC.ai[0] < 1280 || NPC.ai[0] > 1420 && NPC.ai[0] < 1600)
			{
				NPC.ai[1] = 0;
				NPC.ai[2] = 0;
				Vector2 homeCenter = player.Center;
				NPC.spriteDirection = NPC.direction;
				homeCenter.X += (NPC.Center.X < player.Center.X) ? -280 : 280;
				homeCenter.Y -= 30;

				float vel = MathHelper.Clamp(NPC.Distance(homeCenter) / 12, 8, 30);
				NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(homeCenter) * vel, 0.08f);
			}
			else
			{
				NPC.rotation = NPC.velocity.X * 0.04f;

				if (NPC.ai[0] < 1320 || NPC.ai[0] > 1600 && NPC.ai[0] < 1641)
				{
					NPC.velocity.X = -NPC.spriteDirection;
					NPC.velocity.Y = 0;
				}
				else if (NPC.ai[0] == 1320 || NPC.ai[0] == 1641)
				{
					SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
					NPC.velocity.X = MathHelper.Clamp(Math.Abs((player.Center.X - NPC.Center.X) / 10), 24, 36) * NPC.spriteDirection;
					NPC.netUpdate = true;
					trailbehind = true;
				}
				else if (NPC.direction != NPC.spriteDirection || NPC.ai[1] > 0)
				{
					NPC.ai[1]++; //ai 1 is used here to store this being triggered at least once, so if direction is equal to sprite direction again after this it will continue this part of the ai
					NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, 0, 0.06f);
					NPC.noTileCollide = false;

					if (NPC.collideX && NPC.ai[2] == 0)
					{
						NPC.ai[2]++; //ai 2 is used here as a flag to make sure the tile collide effects only trigger once
						Collision.HitTiles(NPC.position, NPC.velocity, NPC.width, NPC.height);
						SoundEngine.PlaySound(SoundID.Dig, NPC.Center);
						NPC.velocity.X *= -0.5f;
						//put other things here for on tile collision effects
					}
				}
			}
		}

		public void SummonSpores()
		{
			if (NPC.ai[0] % 9 == 0)
			{
				SoundEngine.PlaySound(SoundID.DD2_KoboldFlyerDeath with { Volume = 0.8f }, NPC.Center);
				SoundEngine.PlaySound(SoundID.Grass with { PitchVariance = 0.2f }, NPC.Center);
				SoundEngine.PlaySound(SoundID.NPCDeath55 with { PitchVariance = 0.2f }, NPC.Center);
				int p = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X + Main.rand.Next(-100, 100), (int)NPC.Center.Y + Main.rand.Next(-200, -100), ModContent.NPCType<ExplodingSpore>());
				DustHelper.DrawStar(new Vector2(Main.npc[p].Center.X, Main.npc[p].Center.Y), DustID.GoldCoin, pointAmount: 4, mainSize: .9425f, dustDensity: 2, dustSize: .5f, pointDepthMult: 0.3f, noGravity: true);
				Main.npc[p].ai[1] = NPC.whoAmI;
				Main.npc[p].netUpdate = true;
			}
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += .15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.66f * bossLifeScale);
			NPC.damage = (int)(NPC.damage * 0.6f);
		}

		Vector2 Drawoffset => new Vector2(0, NPC.gfxOffY) + Vector2.UnitX * NPC.spriteDirection * 12;

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.2f;
			float num366 = num395 + .85f;

			if ((NPC.ai[0] > 1290 && NPC.ai[0] < 1360 || NPC.ai[0] > 1600 && NPC.ai[0] < 1690) || NPC.life <= (NPC.lifeMax / 10 * 4))
				DrawAfterImage(Main.spriteBatch, new Vector2(0f, 0f), 0.75f, Color.OrangeRed * .7f, Color.Firebrick * .05f, 0.75f, num366, .65f, screenPos);

			var effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos + Drawoffset, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);

			if (trailbehind)
			{
				for (int i = 0; i < NPCID.Sets.TrailCacheLength[NPC.type]; i++)
				{
					Vector2 drawpos = NPC.oldPos[i] + NPC.Size / 2 - screenPos;
					float opacity = 0.5f * ((NPCID.Sets.TrailCacheLength[NPC.type] - i) / (float)NPCID.Sets.TrailCacheLength[NPC.type]);
					spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawpos + Drawoffset, NPC.frame, drawColor * opacity, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
				}
			}
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			float num108 = 4;
			float num107 = (float)Math.Cos((double)(Main.GlobalTimeWrappedHourly % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
			float num106 = 0f;
			Color color1 = Color.White * num107 * .8f;
			var effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(
				Mod.Assets.Request<Texture2D>("NPCs/Boss/ReachBoss/ReachBoss_Glow").Value,
				NPC.Center - screenPos + Drawoffset,
				NPC.frame,
				color1,
				NPC.rotation,
				NPC.frame.Size() / 2,
				NPC.scale,
				effects,
				0
			);

			if (pulseTrail)
			{
				SpriteEffects spriteEffects3 = (NPC.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				Color color29 = new Color(127 - NPC.alpha, 127 - NPC.alpha, 127 - NPC.alpha, 0).MultiplyRGBA(Color.Tomato);
				for (int num103 = 0; num103 < 4; num103++)
				{
					Color color28 = color29;
					color28 = NPC.GetAlpha(color28);
					color28 *= 1f - num107;
					Vector2 vector29 = NPC.Center + ((float)num103 / (float)num108 * 6.28318548f + NPC.rotation + num106).ToRotationVector2() * (4f * num107 + 2f) - screenPos + Drawoffset - NPC.velocity * (float)num103;
					Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/ReachBoss/ReachBoss_Glow").Value, vector29, NPC.frame, color28, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects3, 0f);
				}
			}

			if (pulseTrailPurple || pulseTrailYellow)
			{
				Color glowcolor = (pulseTrailYellow) ? Color.Gold : Color.Orchid;
				float num1072 = (float)Math.Cos((double)(Main.GlobalTimeWrappedHourly % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
				SpriteEffects spriteEffects3 = (NPC.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				Color color29 = new Color(127 - NPC.alpha, 127 - NPC.alpha, 127 - NPC.alpha, 0).MultiplyRGBA(glowcolor);
				for (int num103 = 0; num103 < 4; num103++)
				{
					Color color28 = color29;
					color28 = NPC.GetAlpha(color28);
					color28 *= 1f - num1072;
					Vector2 vector29 = NPC.Center + ((float)num103 / (float)num108 * 6.28318548f + NPC.rotation + num106).ToRotationVector2() * (4f * num1072 + 2f) - screenPos + Drawoffset - NPC.velocity * (float)num103;
					Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/ReachBoss/ReachBoss_PurpleGlow").Value, vector29, NPC.frame, color28 * .9f, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects3, 0f);
				}
			}
		}

		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color color, float opacity, float startScale, float endScale, Vector2 screenPos) => DrawAfterImage(spriteBatch, offset, trailLengthModifier, color, color, opacity, startScale, endScale, screenPos);

		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color startColor, Color endColor, float opacity, float startScale, float endScale, Vector2 screenPos)
		{
			SpriteEffects spriteEffects = (NPC.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			for (int i = 1; i < 10; i++)
			{
				Color color = Color.Lerp(startColor, endColor, i / 10f) * opacity;
				spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/ReachBoss/ReachBoss_Afterimage").Value, new Vector2(NPC.Center.X, NPC.Center.Y) + offset - screenPos + new Vector2(0, NPC.gfxOffY) - NPC.velocity * (float)i * trailLengthModifier, NPC.frame, color, NPC.rotation, NPC.frame.Size() * 0.5f, MathHelper.Lerp(startScale, endScale, i / 10f), spriteEffects, 0f);
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient && NPC.life <= 0)
			{
				if (!text)
				{
					CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), new Color(0, 200, 80, 100),
					"You cannot stop the wrath of nature!");
					text = true;
				}

				Vector2 spawnAt = NPC.Center + new Vector2(0f, (float)NPC.height / 2f);
				NPC.NewNPC(NPC.GetSource_Death(), (int)spawnAt.X, (int)spawnAt.Y, ModContent.NPCType<ReachBoss1>());

				for (int i = 0; i < 8; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ReachBoss").Type, 1f);

				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ReachBoss1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ReachBoss1").Type, 1f);

				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Grass, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;

					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}

			for (int j = 0; j < 2; j++)
			{
				float goreScale = 0.01f * Main.rand.Next(20, 70);
				int a = Gore.NewGore(NPC.GetSource_Death(), NPC.Center + new Vector2(Main.rand.Next(-50, 50), Main.rand.Next(-50, 50)), NPC.velocity, 386, goreScale);
				Main.gore[a].timeLeft = 15;
				Main.gore[a].rotation = 10f;
				Main.gore[a].velocity = new Vector2(hitDirection * 2.5f, Main.rand.NextFloat(1f, 2f));

				int a1 = Gore.NewGore(NPC.GetSource_Death(), NPC.Center + new Vector2(Main.rand.Next(-50, 50), Main.rand.Next(-50, 50)), NPC.velocity, 911, goreScale);
				Main.gore[a1].timeLeft = 15;
				Main.gore[a1].rotation = 1f;
				Main.gore[a1].velocity = new Vector2(hitDirection * 2.5f, Main.rand.NextFloat(10f, 20f));
			}

			for (int k = 0; k < 12; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Plantera_Green, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LavaMoss, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}
		}

		public override bool PreKill() => false;

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Boss;
			progression = 3.5f;
			name = "Vinewrath Bane";
			downedCondition = () => MyWorld.downedReachBoss;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> {
					ModContent.NPCType<ReachBoss>()
				},
				new List<int> {
					ModContent.ItemType<ReachBossSummon>()
				},
				new List<int> {
					ModContent.ItemType<Trophy5>(),
					ModContent.ItemType<ReachMask>(),
					ModContent.ItemType<VinewrathBox>()
				},
				new List<int> {
					ModContent.ItemType<DeathRose>(),
					ModContent.ItemType<SunbeamStaff>(),
					ModContent.ItemType<ThornBow>(),
					ModContent.ItemType<ReachVineStaff>(),
					ModContent.ItemType<ReachBossSword>(),
					ItemID.LesserHealingPotion
				});
			spawnInfo =
				$"Right-click the Bloodblossom, a glowing flower found at the bottom of the Briar. The Vinewrath Bane can be fought at any time and any place in progression. If a Bloodblossom is not present, use a [i:{ModContent.ItemType<ReachBossSummon>()}] in the Briar below the surface at any time.";
			texture = "SpiritMod/Textures/BossChecklist/ReachBossTexture";
			headTextureOverride = "SpiritMod/NPCs/Boss/ReachBoss/ReachBoss/ReachBoss_Head_Boss";
		}
	}
}