using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Accessory.UmbillicalEyeball;
using SpiritMod.Items.Material;
using SpiritMod.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BloodGazer
{
	internal enum BloodGazerAiStates
	{
		Passive = 0,
		EyeSpawn = 1,
		Hostile = 2,
		EyeSwing = 3,
		EyeSpin = 4,
		DetatchingEyes = 5,
		Despawn = 6,
		Phase2Transition = 7,
		EyeMortar = 8,
		RuneEyes = 9,
		SpookyDash = 10
	}

	[AutoloadBossHead]
	public class BloodGazer : ModNPC, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Gazer");
			NPCID.Sets.TrailCacheLength[npc.type] = 10;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.Size = new Vector2(50, 68);
			npc.damage = 45;
			npc.defense = 8;
			npc.lifeMax = 6000;
			npc.knockBackResist = 0.1f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.npcSlots = 8;
			npc.aiStyle = -1;
			Main.npcFrameCount[npc.type] = 4;
			npc.HitSound = SoundID.NPCHit19;
			npc.DeathSound = SoundID.NPCDeath10;
            banner = npc.type;
            bannerItem = ModContent.ItemType<Items.Banners.BloodGazerBanner>();
			npc.netAlways = true;
        }

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * bossLifeScale * 0.66f);
			npc.damage = (int)(npc.damage * bossLifeScale * 0.66f);
		}

		private ref float AiState => ref npc.ai[0];
		private ref float AiTimer => ref npc.ai[1];
		private ref float AttackCounter => ref npc.ai[2];
		private ref float Phase => ref npc.ai[3];
		private ref float GlowOpacity => ref npc.localAI[1];
		private ref float PulseDrawTimer => ref npc.localAI[2];
		public bool trailing = false;

		public bool glowtrail = false;

		private void UpdateAiState(BloodGazerAiStates aistate)
		{
			trailing = false;
			glowtrail = false;
			AiState = (float)aistate;
			AiTimer = 0;
			npc.netUpdate = true;
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			npc.TargetClosest();
			Player player = Main.player[npc.target];
			if (player.active && !player.dead && Collision.CanHit(npc.Center, 0, 0, player.Center, 0, 0) && npc.Distance(player.Center) < 500 && AiState == (float)BloodGazerAiStates.Passive)   //aggro on player if able to reach them, and in its passive state
				UpdateAiState(BloodGazerAiStates.EyeSpawn);

			AiTimer++;
			npc.rotation = npc.velocity.X * 0.05f;
			if (Main.netMode != NetmodeID.Server && Main.rand.Next(60) == 0)
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/HeartbeatFx"));

			if(Phase == 0 && npc.life <= npc.lifeMax / 2)
			{
				Phase++;
				UpdateAiState(BloodGazerAiStates.Phase2Transition);
			}

			switch ((BloodGazerAiStates)AiState) {
				case BloodGazerAiStates.EyeSpawn:
					npc.knockBackResist = 0.1f;
					npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);

					if (AiTimer % 40 == 0) {
						if (Main.netMode != NetmodeID.Server)
							Main.PlaySound(new LegacySoundStyle(SoundID.NPCKilled, 22).WithPitchVariance(0.2f).WithVolume(0.8f), npc.Center);

						if (Main.netMode != NetmodeID.MultiplayerClient) {
							NPC eye = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<BloodGazerEye>(), 0, npc.whoAmI, (int)(AiTimer / 40))];
							Vector2 velocity = Main.rand.NextVector2CircularEdge(1, 1) * Main.rand.NextFloat(2, 4);
							eye.velocity = velocity;
							for (int i = 0; i < 25; i++) {
								Dust dust = Dust.NewDustDirect(npc.Center, 10, 10, ModContent.DustType<Dusts.Blood>(), 0f, -2f, 0, default, Main.rand.NextFloat(0.9f, 1.5f));
								dust.velocity = velocity.RotatedByRandom(MathHelper.Pi / 14) * Main.rand.NextFloat(0.5f, 1f);
							}
							npc.velocity = -velocity / 2;
							if (Main.netMode != NetmodeID.SinglePlayer)
								NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, eye.whoAmI);

							eye.netUpdate = true;
						}
						npc.netUpdate = true;
					}

					if (AiTimer > 120)
						UpdateAiState(BloodGazerAiStates.Hostile);

					break;

				case BloodGazerAiStates.Hostile:
					npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(player.Center) * MathHelper.Clamp(npc.Distance(player.Center) / 200, 1, 4), 0.03f);

					if (AiTimer >= 200)
						UpdateAiState(Main.rand.NextBool() ? BloodGazerAiStates.EyeSwing : BloodGazerAiStates.EyeSpin);

					break;

				case BloodGazerAiStates.EyeSwing:
					npc.knockBackResist = 0f;
					npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(player.Center) * 2, 0.015f);

					if (AiTimer >= 250)
					{
						UpdateAiState(AttackCounter == 0 ? BloodGazerAiStates.EyeSpin : BloodGazerAiStates.DetatchingEyes);
						AttackCounter++;
					}

					break;

				case BloodGazerAiStates.EyeSpin:
					npc.spriteDirection = Math.Sign(npc.velocity.X) < 0 ? -1 : 1;
					npc.knockBackResist = 0f;
					float prespintime = 60;
					float spintime = 60;
					float slowdowntime = 40;
					if(AiTimer < prespintime)
						npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);

					if ((AiTimer > prespintime + spintime) && (Math.Sign(npc.velocity.X) * (player.Center.X - npc.Center.X)) < -300)
						npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.07f);

					float startvel = 1;
					float maxvel = 16;
					if(AiTimer == prespintime)
					{
						npc.velocity.X = (Math.Sign(npc.DirectionTo(player.Center).X) == 0 ? 1 : Math.Sign(npc.DirectionTo(player.Center).X)) * startvel;
						npc.netUpdate = true;

						if (Main.netMode != NetmodeID.Server)
							Main.PlaySound(SoundID.NPCKilled, (int)npc.Center.X, (int)npc.Center.Y, 10, 1, -0.5f);
					}

					if (AiTimer <= prespintime + spintime && Math.Abs(npc.velocity.X) < maxvel)
						npc.velocity.X *= 1.06f;

					if (AiTimer >= prespintime + spintime + slowdowntime && Math.Abs(npc.velocity.X) < 4)
					{
						UpdateAiState(AttackCounter == 0 ? BloodGazerAiStates.EyeSwing : BloodGazerAiStates.DetatchingEyes);
						AttackCounter++;
					}

					break;

				case BloodGazerAiStates.DetatchingEyes:
					npc.knockBackResist = 0.1f;
					npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.03f);

					if (AiTimer >= 180)
					{
						AttackCounter = 0;
						UpdateAiState(BloodGazerAiStates.EyeSpawn);
					}
					break;

				case BloodGazerAiStates.Passive:
					npc.spriteDirection = Math.Sign(npc.velocity.X) < 0 ? -1 : 1;
					npc.velocity.X = (float)Math.Sin(AiTimer / 360) * 2;
					npc.velocity.Y = (float)Math.Cos(AiTimer / 90);
					break;

				case BloodGazerAiStates.Despawn:
					npc.knockBackResist = 0f;
					npc.spriteDirection = Math.Sign(npc.velocity.X) < 0 ? -1 : 1;
					npc.timeLeft = Math.Min(npc.timeLeft - 1, 60);
					npc.velocity.X = MathHelper.Lerp(npc.velocity.X, npc.spriteDirection * 14, 0.00175f);
					npc.velocity.Y = (float)Math.Cos(AiTimer / 90);
					break;

				case BloodGazerAiStates.Phase2Transition:
					npc.knockBackResist = 0f;
					npc.localAI[1] = MathHelper.Lerp(npc.localAI[1], 0.75f, 0.05f);
					npc.spriteDirection = Math.Sign(npc.velocity.X) < 0 ? -1 : 1;

					if (AiTimer <= 31)
						npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionFrom(player.Center), 0.1f);

					if (npc.localAI[1] > 0.6f)
					{
						npc.localAI[1] = 0.75f;
						if (Main.netMode != NetmodeID.Server)
							Main.PlaySound(SoundID.Roar, (int)npc.Center.X, (int)npc.Center.Y, 2, 0.75f, -5f);

						npc.netUpdate = true;
					}

					if (AiTimer > 90)
						UpdateAiState(BloodGazerAiStates.RuneEyes);

					break;

				case BloodGazerAiStates.EyeMortar:
					if (AiTimer < 110)
						npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(player.Center - Vector2.UnitY * 300) * MathHelper.Clamp(npc.Distance(player.Center - Vector2.UnitY * 200) / 75, 3, 12) * 1.5f, 0.07f);
					else
						npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);

					if(AiTimer % 8 == 0 && AiTimer > 130 && AiTimer < 280){
						Vector2 targetPos = player.Center + new Vector2(MathHelper.Lerp(1500 * npc.spriteDirection, 0, Math.Min((AiTimer-130) / 100f, 1)), 0);
						Vector2 vel = npc.GetArcVel(targetPos, 0.35f, 300, 1000, heightabovetarget: 350);
						npc.velocity = -Vector2.Normalize(vel);
						if (Main.netMode != NetmodeID.Server)
							Main.PlaySound(new LegacySoundStyle(SoundID.NPCKilled, 22).WithPitchVariance(0.2f).WithVolume(0.8f), npc.Center);

						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							Projectile eye = Projectile.NewProjectileDirect(npc.Center, vel.RotatedByRandom(MathHelper.Pi / 16), ModContent.ProjectileType<MortarEye>(), npc.damage / 4, 1, Main.myPlayer, npc.whoAmI);
							eye.netUpdate = true;
						}
					}

					if(AiTimer >= 300)
						UpdateAiState(Main.rand.NextBool() ? BloodGazerAiStates.SpookyDash : BloodGazerAiStates.RuneEyes);

					break;

				case BloodGazerAiStates.RuneEyes:
					npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(player.Center) * MathHelper.Clamp(npc.Distance(player.Center) / 200, 0.5f, 2), 0.075f);

					if (AiTimer % 15 == 0 && AiTimer > 15 && AiTimer < 105)
					{
						Vector2 spawnPos = npc.DirectionTo(player.Center).RotatedBy(MathHelper.Pi * Main.rand.NextFloat(0.08f, 0.12f) * (Main.rand.NextBool() ? -1 : 1)) * AiTimer * 12;
						spawnPos += npc.Center;

						if (Main.netMode != NetmodeID.Server)
							Main.PlaySound(new LegacySoundStyle(SoundID.Item, 104).WithPitchVariance(0.3f).WithVolume(0.5f), spawnPos);

						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							Projectile eye = Projectile.NewProjectileDirect(spawnPos, Vector2.Zero, ModContent.ProjectileType<RunicEye>(), npc.damage / 4, 1, Main.myPlayer, 0, npc.whoAmI);
							eye.netUpdate = true;
						}
					}

					if (AiTimer >= 115)
						UpdateAiState(Main.rand.NextBool() ? BloodGazerAiStates.EyeMortar : BloodGazerAiStates.SpookyDash);

					break;

				case BloodGazerAiStates.SpookyDash:
					if (AiTimer >= 360)
					{
						UpdateAiState(Main.rand.NextBool() ? BloodGazerAiStates.EyeMortar : BloodGazerAiStates.RuneEyes);
						return;
					}
					npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(player.Center) * 4, 0.04f);
					glowtrail = true;
					npc.rotation = npc.velocity.ToRotation() + (npc.spriteDirection < 0 ? MathHelper.PiOver2 : MathHelper.PiOver2);
					if (AiTimer % 60 == 0 && AiTimer > 60){
						trailing = true;
						if (Main.netMode != NetmodeID.Server)
							Main.PlaySound(SoundID.DD2_WyvernDiveDown.WithVolume(1.5f), npc.Center);
						npc.velocity = npc.DirectionTo(player.Center) * 40;
						npc.netUpdate = true;
					}

					break;
			}

			if((npc.Distance(player.Center) > 2000) && AiState != (float)BloodGazerAiStates.Passive && AiState != (float)BloodGazerAiStates.Despawn && Phase == 0)  //deaggro if player is too far away
				UpdateAiState(BloodGazerAiStates.Passive);

			if (!player.active || player.dead || ((npc.Distance(player.Center) > 3000 && AiState != (float)BloodGazerAiStates.Phase2Transition) || Main.dayTime) && AiState != (float)BloodGazerAiStates.Despawn)  //despawn if day or player dead
				UpdateAiState(BloodGazerAiStates.Despawn);
		}

		public override bool CheckActive()
		{
			if(AiState == (float)BloodGazerAiStates.Passive || AiState == (float)BloodGazerAiStates.Despawn)
				return true;

			return false;
		}


		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D tex = ModContent.GetTexture(Texture + "_mask");
			Texture2D bloom = mod.GetTexture("Effects/Masks/Extra_49");
			spriteBatch.Draw(bloom, npc.Center - Main.screenPosition, null, Color.Red * npc.localAI[1] * npc.Opacity, npc.rotation, bloom.Size() / 2, 
				npc.scale * 0.9f * ((float)Math.Sin(Main.GlobalTime * 5)/8 + 1f), SpriteEffects.None, 0);

			if (glowtrail)
			{
				for (int i = 0; i < NPCID.Sets.TrailCacheLength[npc.type]; i++)
				{
					float opacity = 0.5f * (float)(NPCID.Sets.TrailCacheLength[npc.type] - i) / NPCID.Sets.TrailCacheLength[npc.type];
					spriteBatch.Draw(tex, npc.oldPos[i] + npc.Size / 2 - Main.screenPosition, npc.frame, Color.White * opacity, npc.rotation, npc.frame.Size() / 2, npc.scale * 1.35f, SpriteEffects.None, 0);
				}
			}
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => trailing;

		public override bool? CanHitNPC(NPC target) => trailing;

		public override void SendExtraAI(BinaryWriter writer) {
			writer.Write(npc.knockBackResist);
			writer.Write(trailing);
			writer.Write(glowtrail);
		}

		public override void ReceiveExtraAI(BinaryReader reader){
			npc.knockBackResist = reader.ReadSingle();
			trailing = reader.ReadBoolean();
			glowtrail = reader.ReadBoolean();
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0 || npc.life >= 0) {
				int d = 5;
				for (int k = 0; k < 25; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .97f);
				}
			}

			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gazer/Gazer1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gazer/Gazer2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gazer/Gazer3"), 1f);
				int d = 5;
				for (int k = 0; k < 25; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.97f);
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 1.27f);
				}
			}
		}
		public override bool PreNPCLoot()
		{
			if(Main.netMode != NetmodeID.Server)
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/DownedMiniboss"));
			MyWorld.downedGazer = true;
			return true;
		}

		public override void NPCLoot()
		{
			npc.DropItem(ModContent.ItemType<UmbillicalEyeball>());
			npc.DropItem(ModContent.ItemType<BloodFire>(), 12 + Main.rand.Next(3, 5));
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D tex = Main.npcTexture[npc.type];
			if (trailing) {
				for (int i = 0; i < NPCID.Sets.TrailCacheLength[npc.type]; i++) {
					float opacity = 0.25f * (float)(NPCID.Sets.TrailCacheLength[npc.type] - i) / NPCID.Sets.TrailCacheLength[npc.type];
					spriteBatch.Draw(tex, npc.oldPos[i] + npc.Size/2 - Main.screenPosition, npc.frame, drawColor * opacity, npc.rotation, npc.frame.Size() / 2, npc.scale, SpriteEffects.None, 0);
				}
			}

			return true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.spawnTileY < Main.rockLayer && (Main.bloodMoon) && Main.hardMode && !NPC.AnyNPCs(ModContent.NPCType<BloodGazer>()) ? SpawnCondition.OverworldNightMonster.Chance * 0.05f : 0f;
	}
}
