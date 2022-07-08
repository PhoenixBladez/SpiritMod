using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.InfernonDrops;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Placeable.MusicBox;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Infernon
{
	[AutoloadBossHead]
	public class Infernon : ModNPC, IBCRegistrable
	{
		public int currentSpread;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Infernon");

		public override void SetDefaults()
		{
			NPC.width = 160;
			NPC.height = 250;
			NPC.damage = 36;
			NPC.defense = 13;
			NPC.lifeMax = 13000;
			NPC.knockBackResist = 0;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.CursedInferno] = true;
			Main.npcFrameCount[NPC.type] = 8;
			NPC.boss = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			Music = MusicLoader.GetMusicSlot(Mod,"Sounds/Music/Infernon");
			NPC.npcSlots = 10;

			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath5;
		}

		public override bool PreAI()
		{
			NPC.spriteDirection = NPC.direction;

			if (!Main.player[NPC.target].active || Main.player[NPC.target].dead)
			{
				NPC.TargetClosest(false);
				NPC.velocity.Y = -100;
			}

			if (!NPC.AnyNPCs(ModContent.NPCType<InfernonSkull>()))
			{
				if (Main.expertMode || NPC.life <= 7000)
					NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<InfernonSkull>(), 0, 2, 1, 0, NPC.whoAmI, NPC.target);
			}

			if (NPC.ai[0] == 0)
			{
				// Get the proper direction to move towards the current targeted player.
				if (NPC.ai[2] == 0)
				{
					NPC.TargetClosest(true);
					NPC.ai[2] = NPC.Center.X >= Main.player[NPC.target].Center.X ? -1f : 1f;
				}
				NPC.TargetClosest(true);

				Player player = Main.player[NPC.target];
				if (!player.active || player.dead)
				{
					NPC.TargetClosest(false);
					NPC.velocity.Y = -100;
				}

				float currentXDist = Math.Abs(NPC.Center.X - player.Center.X);
				if (NPC.Center.X < player.Center.X && NPC.ai[2] < 0)
					NPC.ai[2] = 0;
				if (NPC.Center.X > player.Center.X && NPC.ai[2] > 0)
					NPC.ai[2] = 0;

				float accelerationSpeed = 0.13F;
				float maxXSpeed = 9;
				NPC.velocity.X = NPC.velocity.X + NPC.ai[2] * accelerationSpeed;
				NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -maxXSpeed, maxXSpeed);

				float yDist = player.position.Y - (NPC.position.Y + NPC.height);
				if (yDist < 0)
					NPC.velocity.Y = NPC.velocity.Y - 0.2F;
				if (yDist > 150)
					NPC.velocity.Y = NPC.velocity.Y + 0.2F;
				NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y, -6, 6);
				NPC.rotation = NPC.velocity.X * 0.03f;

				// If the NPC is close enough
				if ((currentXDist < 500 || NPC.ai[3] < 0) && NPC.position.Y < player.position.Y)
				{
					++NPC.ai[3];
					int cooldown = 15;
					if (NPC.life < NPC.lifeMax * 0.75)
						cooldown = 154;
					if (NPC.life < NPC.lifeMax * 0.5)
						cooldown = 13;
					if (NPC.life < NPC.lifeMax * 0.25)
						cooldown = 12;
					cooldown++;
					if (NPC.ai[3] > cooldown)
						NPC.ai[3] = -cooldown;

					if (NPC.ai[3] == 0 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						Vector2 position = NPC.Center;
						position.X += NPC.velocity.X * 7;

						float speedX = player.Center.X - NPC.Center.X;
						float speedY = player.Center.Y - NPC.Center.Y;
						float length = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
						float speed = 6;

						if (NPC.life < NPC.lifeMax * 0.25f)
							speed = 10f;
						else if (NPC.life < NPC.lifeMax * 0.5f)
							speed = 8f;
						else if (NPC.life < NPC.lifeMax * 0.75f)
							speed = 6f;

						float num12 = speed / length;
						speedX *= num12;
						speedY *= num12;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), position.X, position.Y, speedX, speedY, ModContent.ProjectileType<InfernalWave>(), 28, 0, Main.myPlayer);
					}
				}
				else if (NPC.ai[3] < 0)
					NPC.ai[3]++;

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.ai[1] += Main.rand.Next(1, 4);
					if (NPC.ai[1] > 800 && currentXDist < 600)
						NPC.ai[0] = -1;
				}
			}
			else if (NPC.ai[0] == 1)
			{
				if (NPC.ai[2] == 0)
				{
					NPC.TargetClosest(true);
					NPC.ai[2] = NPC.Center.X >= Main.player[NPC.target].Center.X ? -1f : 1f;
				}
				NPC.TargetClosest(true);
				Player player = Main.player[NPC.target];

				if (NPC.Center.X < player.Center.X && NPC.ai[2] < 0)
					NPC.ai[2] = 0;
				if (NPC.Center.X > player.Center.X && NPC.ai[2] > 0)
					NPC.ai[2] = 0;

				float accelerationSpeed = 0.1F;
				float maxXSpeed = 7;
				NPC.velocity.X = NPC.velocity.X + NPC.ai[2] * accelerationSpeed;
				NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -maxXSpeed, maxXSpeed);

				float yDist = player.position.Y - (NPC.position.Y + NPC.height);
				if (yDist < 0)
					NPC.velocity.Y = NPC.velocity.Y - 0.2F;
				if (yDist > 150)
					NPC.velocity.Y = NPC.velocity.Y + 0.2F;
				NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y, -6, 6);

				NPC.rotation = NPC.velocity.X * 0.03f;

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.ai[3]++;
					if (NPC.ai[3] % 5 == 0 && NPC.ai[3] <= 25)
					{
						Vector2 pos = new Vector2(NPC.Center.X, (NPC.position.Y + NPC.height - 14));
						if (!WorldGen.SolidTile((int)(pos.X / 16), (int)(pos.Y / 16)))
						{
							Vector2 dir = player.Center - pos;
							dir.Normalize();
							dir *= 12;
							Projectile.NewProjectile(NPC.GetSource_FromAI(), pos.X, pos.Y, dir.X, dir.Y, ModContent.ProjectileType<FireSpike>(), 24, 0, Main.myPlayer);
							currentSpread++;
						}
					}

					int cooldown = 80;
					if (NPC.life < NPC.lifeMax * 0.75)
						cooldown = 70;
					if (NPC.life < NPC.lifeMax * 0.5)
						cooldown = 60;
					if (NPC.life < NPC.lifeMax * 0.25)
						cooldown = 50;
					if (NPC.life < NPC.lifeMax * 0.1)
						cooldown = 35;
					if (NPC.ai[3] >= cooldown)
						NPC.ai[3] = 0;

					NPC.ai[1] += Main.rand.Next(1, 4);
					if (NPC.ai[1] > 600.0)
						NPC.ai[0] = -1f;
				}
			}
			else if (NPC.ai[0] == 2)
			{
				if (NPC.velocity.X > 0)
					NPC.velocity.X -= 0.1f;
				if (NPC.velocity.X < 0)
					NPC.velocity.X += 0.1f;
				if (NPC.velocity.X > -0.2f && NPC.velocity.X < 0.2f)
					NPC.velocity.X = 0;
				if (NPC.velocity.Y > 0)
					NPC.velocity.Y -= 0.1f;
				if (NPC.velocity.Y < 0)
					NPC.velocity.Y += 0.1f;
				if (NPC.velocity.Y > -0.2f && NPC.velocity.Y < 0.2f)
					NPC.velocity.Y = 0;

				NPC.rotation = NPC.velocity.X * 0.03F;

				NPC.ai[3]++;
				if (NPC.ai[3] >= 60)
				{
					if (NPC.ai[3] % 20 == 0)
					{
						int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].scale = 1.9f;
						int dust1 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch);
						Main.dust[dust1].noGravity = true;
						Main.dust[dust1].scale = 1.9f;
						int dust2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch);
						Main.dust[dust2].noGravity = true;
						Main.dust[dust2].scale = 1.9f;
						int dust3 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch);
						Main.dust[dust3].noGravity = true;
						Main.dust[dust3].scale = 1.9f;
						Vector2 direction = Vector2.One.RotatedByRandom(MathHelper.ToRadians(100));
						int newNPC = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<InfernonSkullMini>());
						Main.npc[newNPC].velocity = direction * 8;
					}
					// Shoot mini skulls.
				}

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.ai[1] += Main.rand.Next(1, 4);
					if (NPC.ai[1] > 500)
						NPC.ai[0] = -1f;
				}
			}
			else if (NPC.ai[0] == 3)
			{
				NPC.velocity.Y -= 0.1F;
				NPC.alpha += 2;
				if (NPC.alpha >= 255)
					NPC.active = false;
				if (NPC.velocity.X > 0)
					NPC.velocity.X -= 0.2F;
				if (NPC.velocity.X < 0)
					NPC.velocity.X += 0.2F;
				if (NPC.velocity.X > -0.2F && NPC.velocity.X < 0.2F)
					NPC.velocity.X = 0;

				NPC.rotation = NPC.velocity.X * 0.03f;
			}

			int dust4 = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.Torch, NPC.velocity.X * 1.5f, NPC.velocity.Y * 1.5f);
			int dust5 = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.Torch, NPC.velocity.X * 1.5f, NPC.velocity.Y * 1.5f);
			Main.dust[dust4].velocity *= 0f;
			Main.dust[dust5].velocity *= 0f;

			if (!Main.player[NPC.target].active || Main.player[NPC.target].dead)
			{
				NPC.TargetClosest(true);
				if (!Main.player[NPC.target].active || Main.player[NPC.target].dead)
				{
					NPC.ai[0] = 3;
					NPC.ai[3] = 0;
				}
			}

			if (NPC.ai[0] != -1)
				return false;

			int num = Main.rand.Next(3);
			NPC.TargetClosest(true);
			if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) > 1000)
				num = 0;
			NPC.ai[0] = num;
			NPC.ai[1] = 0;
			NPC.ai[2] = 0;
			NPC.ai[3] = 0;

			return true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, hitDirection, -1f, 0, default, 1f);
			}
			if (NPC.life <= 0)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient && NPC.life <= 0)
				{
					if (Main.expertMode)
					{
						Main.NewText("You have yet to defeat the true master of Hell...", 220, 100, 100);
						Vector2 spawnAt = NPC.Center + new Vector2(0f, (float)NPC.height);
						NPC.NewNPC(NPC.GetSource_Death(), (int)spawnAt.X, (int)spawnAt.Y, ModContent.NPCType<InfernoSkull>());
					}
				}
				NPC.position.X = NPC.position.X + (NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (NPC.height / 2);
				NPC.width = 156;
				NPC.height = 180;
				NPC.position.X = NPC.position.X - (NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (NPC.height / 2);
				for (int num621 = 0; num621 < 200; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Torch, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 400; num623++)
				{
					int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Torch, 0f, 0f, 100, default, 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Torch, 0f, 0f, 100, default, 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/Boss/Infernon/Infernon_Glow").Value, screenPos);

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override bool PreKill()
		{
			if (Main.expertMode)
				return false;

			MyWorld.downedInfernon = true;
			return true;
		}

		public override void OnKill()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				int centerX = (int)(NPC.position.X + (NPC.width / 2)) / 16;
				int centerY = (int)(NPC.position.Y + (NPC.height / 2)) / 16;
				int halfLength = NPC.width / 2 / 16 + 1;
				for (int x = centerX - halfLength; x <= centerX + halfLength; x++)
				{
					for (int y = centerY - halfLength; y <= centerY + halfLength; y++)
					{
						Tile tile = Main.tile[x, y];
						if ((x == centerX - halfLength || x == centerX + halfLength || y == centerY - halfLength || y == centerY + halfLength) && !Main.tile[x, y].HasTile)
						{
							tile.TileType = TileID.HellstoneBrick;
							tile.HasTile = true;
						}
						tile.LiquidType = LiquidID.Lava;
						tile.LiquidAmount = 0;
						if (Main.netMode == NetmodeID.Server)
							NetMessage.SendTileSquare(-1, x, y, 1);
						else
							WorldGen.SquareTileFrame(x, y, true);
					}
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddBossBag<InfernonBag>();
			npcLoot.AddCommon<InfernonMask>(7);
			npcLoot.AddCommon<Trophy4>(10);
			npcLoot.AddOneFromOptions<InfernalJavelin, InfernalSword, DiabolicHorn, SevenSins, InfernalStaff, EyeOfTheInferno, InfernalShield>();
			npcLoot.AddCommon<InfernalAppendage>(1, 25, 36);
		}

		public override void BossLoot(ref string name, ref int potionType) => potionType = ItemID.GreaterHealingPotion;

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Boss;
			progression = 6.8f;
			name = "Infernon";
			downedCondition = () => MyWorld.downedInfernon;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> {
					ModContent.NPCType<Infernon>()
				},
				new List<int> {
					ModContent.ItemType<CursedCloth>()
				},
				new List<int> {
					ModContent.ItemType<Trophy4>(),
					ModContent.ItemType<InfernonMask>(),
					ModContent.ItemType<InfernonBox>()
				},
				new List<int> {
					ModContent.ItemType<HellsGaze>(),
					ModContent.ItemType<InfernalAppendage>(),
					ModContent.ItemType<InfernalJavelin>(),
					ModContent.ItemType<InfernalSword>(),
					ModContent.ItemType<DiabolicHorn>(),
					ModContent.ItemType<SevenSins>(),
					ModContent.ItemType<InfernalStaff>(),
					ModContent.ItemType<EyeOfTheInferno>(),
					ModContent.ItemType<InfernalShield>(),
					ItemID.GreaterHealingPotion
				});
			spawnInfo =
				$"Use a [i:{ModContent.ItemType<CursedCloth>()}] in the Underworld at any time.";
			texture = "SpiritMod/Textures/BossChecklist/InfernonTexture";
			headTextureOverride = "SpiritMod/NPCs/Boss/Infernon/Infernon_Head_Boss";
		}
	}
}