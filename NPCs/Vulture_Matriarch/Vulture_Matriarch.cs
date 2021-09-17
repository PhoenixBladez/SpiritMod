using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.Vulture_Matriarch;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
			Main.npcFrameCount[npc.type] = 8;
			NPCID.Sets.TrailCacheLength[npc.type] = 20;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 3500;
			npc.defense = 24;
			npc.value = 0f;
			npc.damage = 60;
			npc.npcSlots = 6f;
			npc.knockBackResist = 0f;
			npc.width = 40;
			npc.height = 50;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit28;
			npc.DeathSound = SoundID.NPCDeath31;
			npc.friendly = false;
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
			Player player = Main.player[npc.target];
			npc.TargetClosest(true);
			npc.rotation = npc.velocity.X * 0.05f;
			npc.spriteDirection = npc.direction;

			if (npc.position.Y > player.position.Y + 100)
				npc.velocity.Y = -8f;

			if (npc.Distance(player.Center) <= 140 && npc.ai[0] == 0)
			{
				npc.ai[0] = 1;
				npc.netUpdate = true;
			}

			if (npc.ai[0] != 0)
			{
				if (npc.ai[2] != 1)
				{
					npc.ai[2] = 1;
					Main.NewText("The Vulture Matriarch has been disturbed!", 175, 75, 255);
					Main.PlaySound(SoundID.NPCHit, (int)npc.position.X, (int)npc.position.Y, 28, 1.5f, -0.4f);
					npc.netUpdate = true;
				}

				npc.damage = 17;
				npc.noTileCollide = true;

				if (Main.rand.Next(355) == 0 && !isFlashing && npc.ai[1] < 260 && !justFlashed)
				{
					isFlashing = true;
					justFlashed = true;
					npc.netUpdate = true;
				}
				else
				{
					CircularGlideMovement();
					NormalMovement();
				}

				if (isFlashing)
					Flashing();

				if (!player.active || player.dead)
					npc.velocity.Y = -5f;

				npc.ai[3]++;
				if (npc.ai[1] < 260)
				{
					int interval = npc.life < npc.lifeMax / 2 ? 55 : 110;
					if (npc.ai[3] % interval == 0 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 73, 1f, -0.5f);
						Vector2 toPlayer = Vector2.Normalize(player.Center - npc.Center) * 12f;

						int numberProjectiles = 3 + Main.rand.Next(4);
						for (int i = 0; i < numberProjectiles; i++)
						{
							float scale = 1f - (Main.rand.NextFloat() * .3f);
							Vector2 perturbedSpeed = toPlayer.RotatedByRandom(MathHelper.ToRadians(32)) * scale;
							Projectile.NewProjectile(npc.Center.X + 23 * npc.direction, npc.Center.Y + 16, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("Sharp_Feather"), 22, 3f, 0);
						}
					}
				}
			}
			else
			{
				npc.noTileCollide = false;
				npc.velocity.Y = 5f;
				npc.velocity.X = 0f;
				npc.damage = 0;
			}
		}

		public void Flashing()
		{
			npc.ai[1] = 0;
			flashingTimer++;

			Player player = Main.player[npc.target];
			npc.velocity = Vector2.Zero;
			npc.spriteDirection = player.Center.X > npc.Center.X ? 1 : -1;

			if (flashingTimer >= 179)
			{
				flashingTimer = 0;
				isFlashing = false;
			}

			if (flashingTimer % 30 == 0 && flashingTimer != 180)
			{
				if (player.direction == -npc.spriteDirection && !player.HasBuff(ModContent.BuffType<Golden_Curse>()) && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
				{
					player.AddBuff(ModContent.BuffType<Golden_Curse>(), 600);
					Main.PlaySound(SoundID.Trackable, (int)npc.position.X, (int)npc.position.Y, 50, 1f, -0.5f);

					for (int i = 0; i < 50; ++i)
					{
						int bbb = Dust.NewDust(player.Center, player.width, player.height, DustID.GoldFlame, 0.0f, 0.0f, 100, default, Main.rand.NextFloat(1, 3));
						Main.dust[bbb].velocity *= 3f;
						if (Main.dust[bbb].scale > 1.0)
							Main.dust[bbb].noGravity = true;
					}
				}
				Main.PlaySound(SoundID.Trackable, (int)npc.position.X, (int)npc.position.Y, 41, 1f, 0f);
			}
		}

		public void NormalMovement()
		{
			npc.knockBackResist = 0f;

			Player player = Main.player[npc.target];
			if (npc.ai[1] < 260 && npc.ai[1] > 1)
			{
				const float MovementSpeed = 0.25f;

				float x = Main.player[npc.target].Center.X - npc.Center.X;
				float y = Main.player[npc.target].Center.Y - npc.Center.Y - 300f;
				float distSQ = x * x + y * y;

				if (player == Main.player[npc.target])
				{
					float oldVelX = npc.velocity.X;
					float oldVelY = npc.velocity.Y;

					if (distSQ >= 400.0)
					{
						float num8 = 4f / (float)Math.Sqrt(distSQ);
						oldVelX = x * num8;
						oldVelY = y * num8;
					}

					if (npc.velocity.X < oldVelX)
					{
						npc.velocity.X += MovementSpeed;
						if (npc.velocity.X < 0 && oldVelX > 0)
							npc.velocity.X += MovementSpeed * 2f;
					}
					else if (npc.velocity.X > oldVelX)
					{
						npc.velocity.X -= MovementSpeed;
						if (npc.velocity.X > 0 && oldVelX < 0)
							npc.velocity.X -= MovementSpeed * 2f;
					}

					if (npc.velocity.Y < oldVelY)
					{
						npc.velocity.Y += MovementSpeed;
						if (npc.velocity.Y < 0 && oldVelY > 0)
							npc.velocity.Y += MovementSpeed * 2f;
					}
					else if (npc.velocity.Y > oldVelY)
					{
						npc.velocity.Y -= MovementSpeed;
						if (npc.velocity.Y > 0 && oldVelY < 0)
							npc.velocity.Y -= MovementSpeed * 2f;
					}
				}
			}
		}

		public void CircularGlideMovement()
		{
			npc.knockBackResist = 0f;
			npc.ai[1]++;

			if (npc.ai[1] >= 260 && npc.ai[1] < 480)
			{
				float rot = MathHelper.TwoPi / (720 / 2f);
				npc.velocity = npc.velocity.RotatedBy(rot * 2 * npc.direction);
			}

			if (npc.ai[1] >= 480)
				FlyTowardsPlayer();
		}

		public void FlyTowardsPlayer()
		{
			npc.knockBackResist = 0.99f;
			gliding = true;

			if (npc.ai[1] == 481)
				Main.PlaySound(SoundID.NPCHit, (int)npc.position.X, (int)npc.position.Y, 28, 1.5f, -0.4f);

			Vector2 center = npc.Center;
			Vector2 v = Vector2.Normalize(Main.player[npc.target].Center - center) * 10f;

			npc.velocity.X = (npc.velocity.X * (30f - 1f) + v.X) / 30f;
			npc.velocity.Y = (npc.velocity.Y * (30f - 1f) + v.Y) / 30f;

			if (npc.ai[1] >= 580)
			{
				justFlashed = false;
				gliding = false;
				npc.ai[1] = 0;
				npc.netUpdate = true;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (isFlashing)
			{
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (npc.spriteDirection == 1)
					spriteEffects = SpriteEffects.FlipHorizontally;

				Texture2D ripple = mod.GetTexture("Effects/Ripple");

				Color baseCol = new Color(211, 198, 111, 0);

				Rectangle r3 = ripple.Frame(1, 1, 0, 0);
				Vector2 origin = r3.Size() / 2f;
				Vector2 drawPos = (npc.Bottom - Main.screenPosition) + new Vector2(0.0f, -40f);

				Vector2 scale = new Vector2(0.75f, 1.75f) * 1.5f;
				Vector2 scale2 = new Vector2(1.75f, 0.75f) * 1.5f;

				Main.spriteBatch.Draw(ripple, drawPos, r3, baseCol * 0f, npc.rotation + MathHelper.PiOver2, origin, scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
				Main.spriteBatch.Draw(ripple, drawPos, r3, baseCol * 0f, npc.rotation + MathHelper.PiOver2, origin, scale2, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
				Main.spriteBatch.Draw(ripple, drawPos, r3, Color.Lerp(baseCol * 1.9f, Color.White, 0.7f), npc.rotation + MathHelper.PiOver2, origin, 1.5f, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
				Main.spriteBatch.Draw(ripple, drawPos, r3, Color.Lerp(baseCol * 0.3f, Color.White, 0.2f), npc.rotation + MathHelper.PiOver2, origin, 3f, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
			}
			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (npc.ai[1] >= 360 + 80 || flashingTimer > 0)
			{
				const int Repeats = 4;

				float num8 = (float)(Math.Cos(Main.GlobalTime % 2.4f / 2.4f * MathHelper.TwoPi) / 2.0 + 0.5);

				SpriteEffects spriteEffects = SpriteEffects.None;
				if (npc.spriteDirection == 1)
					spriteEffects = SpriteEffects.FlipHorizontally;

				Texture2D texture = Main.npcTexture[npc.type];
				Vector2 texSize = new Vector2(texture.Width, texture.Height / Main.npcFrameCount[npc.type]);
				Vector2 origin = texSize / 2f;

				Vector2 position1 = npc.Center - Main.screenPosition - texSize * npc.scale / 2f + origin * npc.scale + new Vector2(0f, -10 + npc.gfxOffY);
				Color color2 = new Color(sbyte.MaxValue - npc.alpha, sbyte.MaxValue - npc.alpha, sbyte.MaxValue - npc.alpha, 0).MultiplyRGBA(Color.Gold);

				for (int i = 0; i < Repeats; ++i)
				{
					Color color3 = npc.GetAlpha(color2) * .05f;
					Vector2 offset = new Vector2(npc.Center.X + 2 * npc.spriteDirection, npc.Center.Y) + ((i / Repeats * MathHelper.TwoPi) + npc.rotation).ToRotationVector2();
					Vector2 position2 = offset * (float)(4.0 * num8 + 2.0) - Main.screenPosition - texSize * npc.scale / 2f + origin * npc.scale + new Vector2(0f, -10 + npc.gfxOffY);
					Main.spriteBatch.Draw(Main.npcTexture[npc.type], position2, npc.frame, color3, npc.rotation, origin, npc.scale * 1.15f, spriteEffects, 0.0f);
				}
				Main.spriteBatch.Draw(Main.npcTexture[npc.type], position1, npc.frame, color2, npc.rotation, origin, npc.scale * 1.15f, spriteEffects, 0.0f);
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.ai[0] == 0)
			{
				npc.ai[0] = 1;
				npc.netUpdate = true;
			}

			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/VultureMatriarch/VultureMatriarchGore1"), 1f);
				for (int i = 0; i < 2; ++i)
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/VultureMatriarch/VultureMatriarchGore2"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/VultureMatriarch/VultureMatriarchGore3"), 1f);
				}
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/VultureMatriarch/VultureMatriarchGore4"), 1f);
			}
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Golden_Egg>(), 1);

			if (Main.rand.Next(7) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Vulture_Matriarch_Mask>(), 1);
		}

		public override void FindFrame(int frameHeight)
		{
			const int AnimSpeed = 6;

			npc.frameCounter++;
			if (npc.ai[0] != 0)
			{
				if (!gliding)
				{
					if (npc.frameCounter < AnimSpeed * 1)
						npc.frame.Y = 0 * frameHeight;
					else if (npc.frameCounter < AnimSpeed * 2)
						npc.frame.Y = 1 * frameHeight;
					else if (npc.frameCounter < AnimSpeed * 3)
						npc.frame.Y = 2 * frameHeight;
					else if (npc.frameCounter < AnimSpeed * 4)
						npc.frame.Y = 3 * frameHeight;
					else if (npc.frameCounter < AnimSpeed * 5)
					{
						npc.frame.Y = 4 * frameHeight;
						Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 32, 1f, 0f);
					}
					else
						npc.frameCounter = 0;
				}
				else
				{
					if (npc.frameCounter < AnimSpeed * 1)
						npc.frame.Y = 5 * frameHeight;
					else if (npc.frameCounter < AnimSpeed * 2)
						npc.frame.Y = 6 * frameHeight;
					else
						npc.frameCounter = 0;
				}
			}
			else
				npc.frame.Y = 7 * frameHeight;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 1.2f;
			return null;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (Main.tileSand[spawnInfo.spawnTileType] && Main.hardMode && !NPC.AnyNPCs(ModContent.NPCType<Vulture_Matriarch>()))
				return SpawnCondition.OverworldDayDesert.Chance * .145f;
			return 0;
		}
	}
}