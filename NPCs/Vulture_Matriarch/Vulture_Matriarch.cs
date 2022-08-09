using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.Vulture_Matriarch;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Vulture_Matriarch
{
	[AutoloadBossHead]
	public class Vulture_Matriarch : ModNPC
	{
		public bool gliding = false;

		public bool isFlashing = false;
		public bool justFlashed = false;
		public int flashingTimer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vulture Matriarch");
			Main.npcFrameCount[NPC.type] = 8;
			NPCID.Sets.TrailCacheLength[NPC.type] = 20;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.aiStyle = -1;
			NPC.lifeMax = 3500;
			NPC.defense = 24;
			NPC.value = 0f;
			NPC.damage = 60;
			NPC.npcSlots = 6f;
			NPC.knockBackResist = 0f;
			NPC.width = 40;
			NPC.height = 50;
			NPC.lavaImmune = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.HitSound = SoundID.NPCHit28;
			NPC.DeathSound = SoundID.NPCDeath31;
			NPC.friendly = false;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				new FlavorTextBestiaryInfoElement("The powerful elder of the vulture tribe, the Matriarch has a hold of ancient magics and abilities in order to destroy its prey."),
			});
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(flashingTimer);
			writer.Write(gliding);
			writer.Write(isFlashing);
			writer.Write(justFlashed);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			flashingTimer = reader.ReadInt32();
			gliding = reader.ReadBoolean();
			isFlashing = reader.ReadBoolean();
			justFlashed = reader.ReadBoolean();
		}

		public override void AI()
		{
			Player player = Main.player[NPC.target];
			NPC.TargetClosest(true);
			NPC.rotation = NPC.velocity.X * 0.05f;
			NPC.spriteDirection = NPC.direction;
			NPC.defDamage = 0;

			if (NPC.Distance(player.Center) <= 140 && NPC.ai[0] == 0)
			{
				NPC.ai[0] = 1;
				NPC.netUpdate = true;
			}

			if (NPC.ai[0] != 0)
			{
				if (NPC.position.Y > player.position.Y + 100)
					NPC.velocity.Y = -8f;

				if (NPC.ai[2] != 1)
				{
					NPC.ai[2] = 1;
					Main.NewText("The Vulture Matriarch has been disturbed!", 175, 75, 255);
					SoundEngine.PlaySound(SoundID.NPCHit28, NPC.Center);
					NPC.netUpdate = true;
				}

				NPC.noTileCollide = true;

				if (Main.rand.Next(355) == 0 && !isFlashing && NPC.ai[1] < 260 && !justFlashed)
				{
					isFlashing = true;
					justFlashed = true;
					NPC.netUpdate = true;
				}
				else
				{
					CircularGlideMovement();
					NormalMovement();
				}

				if (isFlashing)
					Flashing();

				if (!player.active || player.dead)
					NPC.velocity.Y = -5f;

				NPC.ai[3]++;
				if (NPC.ai[1] < 260)
				{
					int interval = NPC.life < NPC.lifeMax / 2 ? 55 : 110;
					if (NPC.ai[3] % interval == 0)
					{
						SoundEngine.PlaySound(SoundID.Item73, NPC.Center);
						Vector2 toPlayer = Vector2.Normalize(player.Center - NPC.Center) * 12f;

						int numberProjectiles = 3 + Main.rand.Next(4);
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							for (int i = 0; i < numberProjectiles; i++)
							{
								float scale = 1f - (Main.rand.NextFloat() * .3f);
								Vector2 perturbedSpeed = toPlayer.RotatedByRandom(MathHelper.ToRadians(32)) * scale;
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + 23 * NPC.direction, NPC.Center.Y + 16, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<Sharp_Feather>(), 22, 3f, Main.myPlayer);
							}
						}
					}
				}
			}
			else
			{
				NPC.noTileCollide = false;
				NPC.velocity.Y = 5f;
				NPC.velocity.X = 0f;
			}
		}

		public void Flashing()
		{
			NPC.ai[1] = 0;
			flashingTimer++;

			Player player = Main.player[NPC.target];
			NPC.velocity = Vector2.Zero;
			NPC.spriteDirection = player.Center.X > NPC.Center.X ? 1 : -1;

			if (flashingTimer >= 179)
			{
				flashingTimer = 0;
				isFlashing = false;
			}

			if (flashingTimer % 30 == 0 && flashingTimer != 180)
			{
				if (player.direction == -NPC.spriteDirection && !player.HasBuff(ModContent.BuffType<Golden_Curse>()) && Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0))
				{
					player.AddBuff(ModContent.BuffType<Golden_Curse>(), 600);
					SoundEngine.PlaySound(SoundID.NPCDeath27 with { Volume = 0.8f });

					for (int i = 0; i < 50; ++i)
					{
						int bbb = Dust.NewDust(player.Center, player.width, player.height, DustID.GoldFlame, 0.0f, 0.0f, 100, default, Main.rand.NextFloat(1, 3));
						Main.dust[bbb].velocity *= 3f;
						if (Main.dust[bbb].scale > 1.0)
							Main.dust[bbb].noGravity = true;
					}
				}
				SoundEngine.PlaySound(SoundID.NPCHit28);
			}
		}

		public void NormalMovement()
		{
			NPC.knockBackResist = 0f;

			Player player = Main.player[NPC.target];
			if (NPC.ai[1] < 260 && NPC.ai[1] > 1)
			{
				const float MovementSpeed = 0.25f;

				float x = Main.player[NPC.target].Center.X - NPC.Center.X;
				float y = Main.player[NPC.target].Center.Y - NPC.Center.Y - 300f;
				float distSQ = x * x + y * y;

				if (player == Main.player[NPC.target])
				{
					float oldVelX = NPC.velocity.X;
					float oldVelY = NPC.velocity.Y;

					if (distSQ >= 400.0)
					{
						float num8 = 4f / (float)Math.Sqrt(distSQ);
						oldVelX = x * num8;
						oldVelY = y * num8;
					}

					if (NPC.velocity.X < oldVelX)
					{
						NPC.velocity.X += MovementSpeed;
						if (NPC.velocity.X < 0 && oldVelX > 0)
							NPC.velocity.X += MovementSpeed * 2f;
					}
					else if (NPC.velocity.X > oldVelX)
					{
						NPC.velocity.X -= MovementSpeed;
						if (NPC.velocity.X > 0 && oldVelX < 0)
							NPC.velocity.X -= MovementSpeed * 2f;
					}

					if (NPC.velocity.Y < oldVelY)
					{
						NPC.velocity.Y += MovementSpeed;
						if (NPC.velocity.Y < 0 && oldVelY > 0)
							NPC.velocity.Y += MovementSpeed * 2f;
					}
					else if (NPC.velocity.Y > oldVelY)
					{
						NPC.velocity.Y -= MovementSpeed;
						if (NPC.velocity.Y > 0 && oldVelY < 0)
							NPC.velocity.Y -= MovementSpeed * 2f;
					}
				}
			}
		}

		public void CircularGlideMovement()
		{
			NPC.knockBackResist = 0f;
			NPC.ai[1]++;

			if (NPC.ai[1] >= 260 && NPC.ai[1] < 480)
			{
				float rot = MathHelper.TwoPi / (720 / 2f);
				NPC.velocity = NPC.velocity.RotatedBy(rot * 2 * NPC.direction);
			}

			if (NPC.ai[1] >= 480)
				FlyTowardsPlayer();
		}

		public void FlyTowardsPlayer()
		{
			NPC.knockBackResist = 0.99f;
			NPC.defDamage = 50;
			gliding = true;

			if (NPC.ai[1] == 481)
				SoundEngine.PlaySound(SoundID.NPCHit28, NPC.Center);

			Vector2 center = NPC.Center;
			Vector2 v = Vector2.Normalize(Main.player[NPC.target].Center - center) * 10f;

			NPC.velocity.X = (NPC.velocity.X * (30f - 1f) + v.X) / 30f;
			NPC.velocity.Y = (NPC.velocity.Y * (30f - 1f) + v.Y) / 30f;

			if (NPC.ai[1] >= 580)
			{
				justFlashed = false;
				gliding = false;
				NPC.ai[1] = 0;
				NPC.netUpdate = true;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (isFlashing)
			{
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (NPC.spriteDirection == 1)
					spriteEffects = SpriteEffects.FlipHorizontally;

				Texture2D ripple = Mod.Assets.Request<Texture2D>("Effects/Ripple").Value;

				Color baseCol = new Color(211, 198, 111, 0);

				Rectangle r3 = ripple.Frame(1, 1, 0, 0);
				Vector2 origin = r3.Size() / 2f;
				Vector2 drawPos = (NPC.Bottom - Main.screenPosition) + new Vector2(0.0f, -40f);

				Vector2 scale = new Vector2(0.75f, 1.75f) * 1.5f;
				Vector2 scale2 = new Vector2(1.75f, 0.75f) * 1.5f;

				Main.spriteBatch.Draw(ripple, drawPos, r3, baseCol * 0f, NPC.rotation + MathHelper.PiOver2, origin, scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
				Main.spriteBatch.Draw(ripple, drawPos, r3, baseCol * 0f, NPC.rotation + MathHelper.PiOver2, origin, scale2, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
				Main.spriteBatch.Draw(ripple, drawPos, r3, Color.Lerp(baseCol * 1.9f, Color.White, 0.7f), NPC.rotation + MathHelper.PiOver2, origin, 1.5f, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
				Main.spriteBatch.Draw(ripple, drawPos, r3, Color.Lerp(baseCol * 0.3f, Color.White, 0.2f), NPC.rotation + MathHelper.PiOver2, origin, 3f, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
			}
			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.ai[1] >= 360 + 80 || flashingTimer > 0)
			{
				const int Repeats = 4;

				float num8 = (float)(Math.Cos(Main.GlobalTimeWrappedHourly % 2.4f / 2.4f * MathHelper.TwoPi) / 2.0 + 0.5);

				SpriteEffects spriteEffects = SpriteEffects.None;
				if (NPC.spriteDirection == 1)
					spriteEffects = SpriteEffects.FlipHorizontally;

				Texture2D texture = TextureAssets.Npc[NPC.type].Value;
				Vector2 texSize = new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]);
				Vector2 origin = texSize / 2f;

				Vector2 position1 = NPC.Center - Main.screenPosition - texSize * NPC.scale / 2f + origin * NPC.scale + new Vector2(0f, -10 + NPC.gfxOffY);
				Color color2 = new Color(sbyte.MaxValue - NPC.alpha, sbyte.MaxValue - NPC.alpha, sbyte.MaxValue - NPC.alpha, 0).MultiplyRGBA(Color.Gold);

				for (int i = 0; i < Repeats; ++i)
				{
					Color color3 = NPC.GetAlpha(color2) * .05f;
					Vector2 offset = new Vector2(NPC.Center.X + 2 * NPC.spriteDirection, NPC.Center.Y) + ((i / Repeats * MathHelper.TwoPi) + NPC.rotation).ToRotationVector2();
					Vector2 position2 = offset * (float)(4.0 * num8 + 2.0) - Main.screenPosition - texSize * NPC.scale / 2f + origin * NPC.scale + new Vector2(0f, -10 + NPC.gfxOffY);
					Main.spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, position2, NPC.frame, color3, NPC.rotation, origin, NPC.scale * 1.15f, spriteEffects, 0.0f);
				}
				Main.spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, position1, NPC.frame, color2, NPC.rotation, origin, NPC.scale * 1.15f, spriteEffects, 0.0f);
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.ai[0] == 0)
			{
				NPC.ai[0] = 1;
				NPC.netUpdate = true;
			}

			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("VultureMatriarchGore1").Type, 1f);
				for (int i = 0; i < 2; ++i)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("VultureMatriarchGore2").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("VultureMatriarchGore3").Type, 1f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("VultureMatriarchGore4").Type, 1f);
			}
		}

		public override void OnKill()
		{
			NPC.DropItemInstanced(NPC.position, NPC.Size, ModContent.ItemType<GoldenEgg>(), 1, true);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddCommon<Vulture_Matriarch_Mask>(12);

		public override void FindFrame(int frameHeight)
		{
			const int AnimSpeed = 6;

			NPC.frameCounter++;
			if (NPC.ai[0] != 0)
			{
				if (!gliding)
				{
					if (NPC.frameCounter < AnimSpeed * 1)
						NPC.frame.Y = 0 * frameHeight;
					else if (NPC.frameCounter < AnimSpeed * 2)
						NPC.frame.Y = 1 * frameHeight;
					else if (NPC.frameCounter < AnimSpeed * 3)
						NPC.frame.Y = 2 * frameHeight;
					else if (NPC.frameCounter < AnimSpeed * 4)
						NPC.frame.Y = 3 * frameHeight;
					else if (NPC.frameCounter < AnimSpeed * 5)
					{
						NPC.frame.Y = 4 * frameHeight;
						SoundEngine.PlaySound(SoundID.Item32, NPC.Center);
					}
					else
						NPC.frameCounter = 0;
				}
				else
				{
					if (NPC.frameCounter < AnimSpeed * 1)
						NPC.frame.Y = 5 * frameHeight;
					else if (NPC.frameCounter < AnimSpeed * 2)
						NPC.frame.Y = 6 * frameHeight;
					else
						NPC.frameCounter = 0;
				}
			}
			else
				NPC.frame.Y = 7 * frameHeight;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 1.2f;
			return null;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (Main.tileSand[spawnInfo.SpawnTileType] && Main.hardMode && !NPC.AnyNPCs(ModContent.NPCType<Vulture_Matriarch>()))
				return SpawnCondition.OverworldDayDesert.Chance * .145f;
			return 0;
		}
	}
}