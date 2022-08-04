using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using System.IO;
using SpiritMod.World.Sepulchre;
using System.Linq;
using SpiritMod.Buffs.DoT;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Enchanted_Armor
{
	public class Enchanted_Armor : ModNPC
	{
		ref float FlashTimer => ref NPC.ai[1];

		ref float DespawnTimer => ref NPC.localAI[0];

		private int TileX
		{
			get => (int)NPC.localAI[1] / 16;
			set => NPC.localAI[1] = 16 * value;
		}
		private int TileY
		{
			get => (int)NPC.localAI[2] / 16;
			set => NPC.localAI[2] = 16 * value;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Draugr");
			Main.npcFrameCount[NPC.type] = 12;
			NPCID.Sets.TrailCacheLength[NPC.type] = 10;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}
		public override void SetDefaults()
		{
			NPC.aiStyle = -1;
			NPC.lifeMax = 85;
			NPC.defense = 15;
			NPC.value = 300f;
			NPC.knockBackResist = 0.3f;
			NPC.width = 30;
			NPC.height = 56;
			NPC.damage = 27;

			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;

			NPC.lavaImmune = true;
			NPC.noTileCollide = false;
			NPC.noGravity = false;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath6;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.DraugrBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
				new FlavorTextBestiaryInfoElement("These suits of armor were given life by cursed souls connected to the dark sepulchers’ treasure. Now they wait frozen and inanimate for a brave adventurer to wander inside."),
			});
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			for (int i = 0; i < NPC.localAI.Length; i++)
				writer.Write(NPC.localAI[i]);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			for (int i = 0; i < NPC.localAI.Length; i++)
				NPC.localAI[i] = reader.ReadSingle();
		}

		public override void AI()
		{
			Player player = Main.player[NPC.target];
			NPC.TargetClosest(true);
			NPC.spriteDirection = NPC.direction;
			if ((NPC.localAI[3] += 2) == 2)
			{ //localai3 was decreasing every tick and i couldnt pinpoint why so i just took the lazy route out and made it increase more to counter it out
				DespawnTimer = 10;
				NPC.localAI[1] = NPC.Left.X;
				NPC.localAI[2] = NPC.Left.Y + 16;
				NPC.netUpdate = true;
			}
			if (!player.active || player.dead || NPC.Distance(player.Center) > 2000)
			{
				Movement();
				DespawnTimer--;
				if (DespawnTimer <= 0)
				{
					for (int i = 0; i < 6; i++)
						Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Main.rand.NextVector2Circular(2, 2), 99);

					bool placed = false;
					Point CheckTile = new Point((int)NPC.Left.X / 16, (int)(NPC.Left.Y + 16) / 16);
					bool CanPlaceStatue(Point CheckFrom)
					{
						return (Collision.SolidTiles(CheckFrom.X, CheckFrom.X + 1, CheckFrom.Y + 1, CheckFrom.Y + 1) || //check if the bottom two tiles are solid
							(TileID.Sets.Platforms[Framing.GetTileSafely(CheckFrom.X, CheckFrom.Y + 1).TileType] && TileID.Sets.Platforms[Framing.GetTileSafely(CheckFrom.X + 1, CheckFrom.Y + 1).TileType])) //or if they're platforms
							&& !Framing.GetTileSafely(CheckFrom.X, CheckFrom.Y).HasTile && !Framing.GetTileSafely(CheckFrom.X + 1, CheckFrom.Y).HasTile //then check if the space to put a statue in is empty, probably a better way to do this??
							&& !Framing.GetTileSafely(CheckFrom.X, CheckFrom.Y - 1).HasTile && !Framing.GetTileSafely(CheckFrom.X + 1, CheckFrom.Y - 1).HasTile
							&& !Framing.GetTileSafely(CheckFrom.X, CheckFrom.Y - 2).HasTile && !Framing.GetTileSafely(CheckFrom.X + 1, CheckFrom.Y - 2).HasTile
							&& !Framing.GetTileSafely(CheckFrom.X, CheckFrom.Y - 3).HasTile && !Framing.GetTileSafely(CheckFrom.X + 1, CheckFrom.Y - 3).HasTile;
					}

					if (CanPlaceStatue(CheckTile))
					{
						WorldGen.PlaceObject(CheckTile.X, CheckTile.Y, ModContent.TileType<CursedArmor>(), direction: NPC.spriteDirection);
						if (Main.netMode != NetmodeID.SinglePlayer)
							NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, ModContent.TileType<CursedArmor>(), CheckTile.X, CheckTile.Y);

						for (int i = 0; i < 6; i++)
							Gore.NewGore(NPC.GetSource_Death(), CheckTile.ToWorldCoordinates(), Main.rand.NextVector2Circular(2, 2), 99);
						placed = true;
					}
					else if (CanPlaceStatue(new Point(TileX, TileY)))
					{
						WorldGen.PlaceObject(TileX, TileY, ModContent.TileType<CursedArmor>(), direction: NPC.spriteDirection);
						if (Main.netMode != NetmodeID.SinglePlayer)
							NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, ModContent.TileType<CursedArmor>(), TileX, TileY);

						for (int i = 0; i < 6; i++)
							Gore.NewGore(NPC.GetSource_Death(), new Point(TileX, TileY).ToWorldCoordinates(), Main.rand.NextVector2Circular(2, 2), 99);
						placed = true;
					}
					int tries = 0;
					while (!placed)
					{
						for (int indexX = -20; indexX <= 20; indexX++)
						{
							for (int indexY = -20; indexY <= 20; indexY++)
							{
								var checkFrom = new Point(indexX + CheckTile.X, indexY + CheckTile.Y);
								if (CanPlaceStatue(checkFrom) && !placed && Main.rand.NextBool(7))
								{
									WorldGen.PlaceObject(checkFrom.X, checkFrom.Y, ModContent.TileType<CursedArmor>(), direction: NPC.spriteDirection);
									if (Main.netMode != NetmodeID.SinglePlayer)
										NetMessage.SendData(MessageID.TileChange, -1, -1, null, ModContent.TileType<CursedArmor>(), checkFrom.X, checkFrom.Y);

									for (int i = 0; i < 6; i++)
										Gore.NewGore(NPC.GetSource_Death(), checkFrom.ToWorldCoordinates(), Main.rand.NextVector2Circular(2, 2), 99);
									placed = true;
								}
							}
						}
						tries++;
						if (tries >= 8)
							break;
					}

					NPC.active = false;
				}
				return;
			}

			DespawnTimer = 10;

			if (Vector2.Distance(player.Center, NPC.Center) > 60f)
				Movement();
			else
				NPC.velocity.X = 0f;
			FlashTimer = Math.Max(FlashTimer - 1, 0);

			Lighting.AddLight(new Vector2(NPC.Center.X, NPC.Center.Y), 72 * 0.002f, 175 * 0.002f, 206 * 0.002f);
			CheckPlatform(player);
		}

		private void CheckPlatform(Player player)
		{
			bool onPlatform = true;
			for (int i = (int)NPC.position.X; i < NPC.position.X + NPC.width; i += NPC.width / 4)
			{
				Tile tile = Framing.GetTileSafely(new Point((int)NPC.position.X / 16, (int)(NPC.position.Y + NPC.height + 8) / 16));
				if (!TileID.Sets.Platforms[tile.TileType])
					onPlatform = false;
			}
			if (onPlatform && NPC.Bottom.Y < player.Top.Y)
				NPC.noTileCollide = true;
			else
				NPC.noTileCollide = false;
		}

		public void Movement()
		{
			int num1 = 30;
			int num2 = 10;
			bool flag1 = false;
			bool flag2 = false;
			bool flag3 = false;
			if (NPC.velocity.Y == 0.0 && (NPC.velocity.X > 0.0 && NPC.direction < 0 || NPC.velocity.X < 0.0 && NPC.direction > 0))
			{
				flag2 = true;
				++NPC.ai[3];
			}
			if (NPC.position.X == NPC.oldPosition.X || NPC.ai[3] >= num1 || flag2)
			{
				++NPC.ai[3];
				flag3 = true;
			}
			else if (NPC.ai[3] > 0.0)
			{
				--NPC.ai[3];
			}

			if (NPC.ai[3] > (num1 * num2))
			{
				NPC.ai[3] = 0.0f;
				NPC.netUpdate = true;
			}

			if (NPC.justHit)
			{
				NPC.ai[3] = 0.0f;
				NPC.netUpdate = true;
			}

			if (NPC.ai[3] == num1)
				NPC.netUpdate = true;

			Vector2 vector2_1 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
			float num3 = Main.player[NPC.target].position.X + Main.player[NPC.target].width * 0.5f - vector2_1.X;
			float num4 = Main.player[NPC.target].position.Y - vector2_1.Y;
			float num5 = (float)Math.Sqrt(num3 * num3 + num4 * num4);
			if (num5 < 200.0 && !flag3)
			{
				NPC.ai[3] = 0.0f;
			}

			if (NPC.ai[3] < num1)
				NPC.TargetClosest(true);
			else
			{
				if (NPC.velocity.X == 0.0)
				{
					if (NPC.velocity.Y == 0.0)
					{
						++NPC.ai[0];
						if (NPC.ai[0] >= 2.0)
						{
							NPC.direction *= -1;
							if (NPC.velocity.X < 0f)
								NPC.spriteDirection = -1;
							else if (NPC.velocity.X > 0f)
								NPC.spriteDirection = 1;

							NPC.ai[0] = 0.0f;
						}
					}
				}
				else
					NPC.ai[0] = 0.0f;

				NPC.directionY = -1;
				if (NPC.direction == 0)
					NPC.direction = 1;
			}
			float num6 = 2f; //walking speed
			float num7 = 0.5f; //regular speed (x)
			if (!flag1 && (NPC.velocity.Y == 0.0 || NPC.wet || NPC.velocity.X <= 0.0 && NPC.direction < 0 || NPC.velocity.X >= 0.0 && NPC.direction > 0))
			{
				if (NPC.velocity.X < -num6 || NPC.velocity.X > num6)
				{
					if (NPC.velocity.Y == 0.0)
					{
						Vector2 vector2_2 = NPC.velocity * 0.5f; // Slide speed
						NPC.velocity = vector2_2;
					}
				}
				else if (NPC.velocity.X < num6 && NPC.direction == 1)
				{
					NPC.velocity.X += num7;
					if (NPC.velocity.X > num6)
					{
						NPC.velocity.X = num6;
					}
				}
				else if (NPC.velocity.X > -num6 && NPC.direction == -1)
				{
					NPC.velocity.X -= num7;
					if (NPC.velocity.X < -num6)
					{
						NPC.velocity.X = -num6;
					}
				}
			}
			if (NPC.velocity.Y >= 0)
			{
				int num8 = 0;
				if (NPC.velocity.X < 0.0)
					num8 = -1;

				if (NPC.velocity.X > 0.0)
					num8 = 1;

				Vector2 position = NPC.position;
				position.X += NPC.velocity.X;
				int index1 = (int)((position.X + (NPC.width / 2) + ((NPC.width / 2 + 1) * num8)) / 16.0);
				int index2 = (int)((position.Y + NPC.height - 1.0) / 16.0);

				if ((index1 * 16) < position.X + NPC.width && (index1 * 16 + 16) > position.X && (Main.tile[index1, index2].HasUnactuatedTile && !Main.tile[index1, index2].TopSlope && (!Main.tile[index1, index2 - 1].TopSlope && Main.tileSolid[(int)Main.tile[index1, index2].TileType]) && !Main.tileSolidTop[(int)Main.tile[index1, index2].TileType] || Main.tile[index1, index2 - 1].IsHalfBlock && Main.tile[index1, index2 - 1].HasUnactuatedTile) && ((!Main.tile[index1, index2 - 1].HasUnactuatedTile || !Main.tileSolid[(int)Main.tile[index1, index2 - 1].TileType] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 1].TileType] || Main.tile[index1, index2 - 1].IsHalfBlock && (!Main.tile[index1, index2 - 4].HasUnactuatedTile || !Main.tileSolid[(int)Main.tile[index1, index2 - 4].TileType] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 4].TileType])) && ((!Main.tile[index1, index2 - 2].HasUnactuatedTile || !Main.tileSolid[(int)Main.tile[index1, index2 - 2].TileType] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 2].TileType]) && (!Main.tile[index1, index2 - 3].HasUnactuatedTile || !Main.tileSolid[(int)Main.tile[index1, index2 - 3].TileType] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 3].TileType]) && (!Main.tile[index1 - num8, index2 - 3].HasUnactuatedTile || !Main.tileSolid[(int)Main.tile[index1 - num8, index2 - 3].TileType]))))
				{
					float num9 = (float)(index2 * 16);
					if (Main.tile[index1, index2].IsHalfBlock)
						num9 += 8f;

					if (Main.tile[index1, index2 - 1].IsHalfBlock)
						num9 -= 8f;

					if (num9 < position.Y + NPC.height)
					{
						float num10 = position.Y + NPC.height - num9;
						if (num10 <= 16.1)
						{
							NPC.gfxOffY += NPC.position.Y + NPC.height - num9;
							NPC.position.Y = num9 - NPC.height;
							NPC.stepSpeed = num10 >= 9.0 ? 2f : 1f;
						}
					}
				}
			}
			if (NPC.velocity.Y == 0.0)
			{
				int index1 = (int)((NPC.position.X + (NPC.width / 2) + ((NPC.width / 2 + 2) * NPC.direction) + NPC.velocity.X * 5.0) / 16.0);
				int index2 = (int)((NPC.position.Y + NPC.height - 15.0) / 16.0);

				int spriteDirection = NPC.spriteDirection;
				if (NPC.velocity.X < 0.0 && spriteDirection == -1 || NPC.velocity.X > 0.0 && spriteDirection == 1)
				{
					float num8 = 3f;
					if (Main.tile[index1, index2 - 2].HasUnactuatedTile && Main.tileSolid[Main.tile[index1, index2 - 2].TileType])
					{
						if (Main.tile[index1, index2 - 3].HasUnactuatedTile && Main.tileSolid[Main.tile[index1, index2 - 3].TileType])
						{
							NPC.velocity.Y = -8.5f;
							NPC.netUpdate = true;
						}
						else
						{
							NPC.velocity.Y = -8.5f;
							NPC.netUpdate = true;
						}
					}
					else if (Main.tile[index1, index2 - 1].HasUnactuatedTile && !Main.tile[index1, index2 - 1].TopSlope && Main.tileSolid[(int)Main.tile[index1, index2 - 1].TileType])
					{
						NPC.velocity.Y = -8.5f;
						NPC.netUpdate = true;
					}
					else if (NPC.position.Y + NPC.height - (index2 * 16) > 20.0 && Main.tile[index1, index2].HasUnactuatedTile && (!Main.tile[index1, index2].TopSlope && Main.tileSolid[Main.tile[index1, index2].TileType]))
					{
						NPC.velocity.Y = -8.5f;
						NPC.netUpdate = true;
					}
					else if ((NPC.directionY < 0 || Math.Abs(NPC.velocity.X) > num8) && ((!Main.tile[index1, index2 + 2].HasUnactuatedTile || !Main.tileSolid[Main.tile[index1, index2 + 2].TileType]) && (!Main.tile[index1 + NPC.direction, index2 + 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[index1 + NPC.direction, index2 + 3].TileType])))
					{
						NPC.velocity.Y = -8.5f;
						NPC.netUpdate = true;
					}
				}
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("EnchantedArmorGore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("EnchantedArmorGore2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("EnchantedArmorGore3").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("EnchantedArmorGore4").Type, 1f);
			}
			for (int k = 0; k < 10; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Iron, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Iron, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Clentaminator_Green, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}
		}

		delegate void OnTileCheck(int x, int y);

		public override void OnKill()
		{
			void tilecheck(int i, int j, int maxX, int maxY, int type, OnTileCheck onTileCheck)
			{
				for (int indexX = -maxX; indexX <= maxX; indexX++)
					for (int indexY = -maxY; indexY <= maxY; indexY++)
						if (Framing.GetTileSafely(indexX + i, indexY + j).TileType == type && Framing.GetTileSafely(indexX + i, indexY + j).TileFrameX == 0 && Framing.GetTileSafely(indexX + i, indexY + j).TileFrameY == 0)
							onTileCheck(indexX + i, indexY + j);
			}

			if (Main.npc.Where(x => x.active && x.type == NPC.type).Count() <= 1)
			{ //only run if no cursed armors are left after this dies
				tilecheck(NPC.position.ToTileCoordinates().X, NPC.position.ToTileCoordinates().Y, 90, 90, ModContent.TileType<SepulchreChestTile>(), delegate (int x, int y) //first, check if any chests are nearby
				{//if so, run a check from the chest's position to see if there are any cursed armor tiles near it
					bool anyarmor = false;
					tilecheck(x, y, 70, 90, ModContent.TileType<CursedArmor>(), delegate
					{
						anyarmor = true;
					});
					if (!anyarmor)
					{ //if not, display text and play miniboss jingle
						CombatText.NewText(new Rectangle(x * 16, y * 16, 20, 10), Color.GreenYellow, "Unlocked!");

						if (Main.netMode != NetmodeID.Server) //custom sounds bad on server
							SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/DownedMiniboss"), NPC.Center);
					}
				});
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => false;

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, new Vector2(NPC.Center.X, NPC.Center.Y - 8) - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);

			spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Enchanted_Armor/Enchanted_Armor_Glow").Value, new Vector2(NPC.Center.X, NPC.Center.Y - 8) - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);

			float maskopacity = (FlashTimer / 30) * 0.5f;
			spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Enchanted_Armor/Enchanted_Armor_Mask").Value, new Vector2(NPC.Center.X, NPC.Center.Y - 8) - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 Color.White * maskopacity, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
		}

		public override void FindFrame(int frameHeight)
		{
			Player player = Main.player[NPC.target];
			NPC.frameCounter++;
			NPC.frame.Width = 90;
			if ((double)Vector2.Distance(player.Center, NPC.Center) > 60f || !player.active || player.dead)
			{
				if (NPC.frameCounter < 6)
				{
					NPC.frame.Y = 0 * frameHeight;
					NPC.frame.X = 0;
				}
				else if (NPC.frameCounter < 12)
				{
					NPC.frame.Y = 1 * frameHeight;
					NPC.frame.X = 0;
				}
				else if (NPC.frameCounter < 18)
				{
					NPC.frame.Y = 2 * frameHeight;
					NPC.frame.X = 0;
				}
				else if (NPC.frameCounter < 24)
				{
					NPC.frame.Y = 3 * frameHeight;
					NPC.frame.X = 0;
				}
				else if (NPC.frameCounter < 30)
				{
					NPC.frame.Y = 4 * frameHeight;
					NPC.frame.X = 0;
				}
				else
					NPC.frameCounter = 0;
			}
			else
			{
				if (NPC.frameCounter < 5)
				{
					NPC.frame.Y = 5 * frameHeight;
					NPC.frame.X = 0;
				}
				else if (NPC.frameCounter < 10)
				{
					NPC.frame.Y = 6 * frameHeight;
					NPC.frame.X = 0;
				}
				else if (NPC.frameCounter < 15)
				{
					NPC.frame.Y = 7 * frameHeight;
					NPC.frame.X = 0;
				}
				else if (NPC.frameCounter < 20)
				{
					NPC.frame.Y = 8 * frameHeight;
					NPC.frame.X = 0;
				}
				else if (NPC.frameCounter < 25)
				{
					NPC.frame.Y = 9 * frameHeight;
					NPC.frame.X = 0;
				}
				else if (NPC.frameCounter < 30)
				{
					NPC.frame.Y = 10 * frameHeight;
					NPC.frame.X = 0;
				}
				else if (NPC.frameCounter < 35)
				{
					if (NPC.frameCounter == 30 && Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0))
					{
						player.Hurt(PlayerDeathReason.LegacyDefault(), (int)(NPC.damage * 1.5f), NPC.direction, false, false, false, -1);
						NPC.frame.Y = 9 * frameHeight;
					}
					NPC.frame.Y = 11 * frameHeight;
					NPC.frame.X = 0;
				}
				else
					NPC.frameCounter = 0;
			}
		}
	}
}