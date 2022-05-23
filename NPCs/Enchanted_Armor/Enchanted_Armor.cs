using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using System.IO;
using SpiritMod.World.Sepulchre;
using System.Linq;
using SpiritMod.Buffs;
using SpiritMod.Buffs.DoT;

namespace SpiritMod.NPCs.Enchanted_Armor
{
	public class Enchanted_Armor : ModNPC
	{
		ref float FlashTimer => ref npc.ai[1];

		ref float DespawnTimer => ref npc.localAI[0];

		private int TileX
		{
			get => (int)npc.localAI[1] / 16;
			set => npc.localAI[1] = 16 * value;
		}
		private int TileY
		{
			get => (int)npc.localAI[2] / 16;
			set => npc.localAI[2] = 16 * value;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Draugr");
			Main.npcFrameCount[npc.type] = 12;
			NPCID.Sets.TrailCacheLength[npc.type] = 10;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 85;
			npc.defense = 15;
			npc.value = 300f;
			npc.knockBackResist = 0.3f;
			npc.width = 30;
			npc.height = 56;
			npc.damage = 27;

			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;

			npc.lavaImmune = true;
			npc.noTileCollide = false;
			npc.noGravity = false;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath6;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.DraugrBanner>();
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			for (int i = 0; i < npc.localAI.Length; i++)
				writer.Write(npc.localAI[i]);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			for (int i = 0; i < npc.localAI.Length; i++)
				npc.localAI[i] = reader.ReadSingle();
		}

		public override void AI()
		{
			Player player = Main.player[npc.target];
			npc.TargetClosest(true);
			npc.spriteDirection = npc.direction;
			if ((npc.localAI[3] += 2) == 2)
			{ //localai3 was decreasing every tick and i couldnt pinpoint why so i just took the lazy route out and made it increase more to counter it out
				DespawnTimer = 10;
				npc.localAI[1] = npc.Left.X;
				npc.localAI[2] = npc.Left.Y + 16;
				npc.netUpdate = true;
			}
			if (!player.active || player.dead || npc.Distance(player.Center) > 2000)
			{
				Movement();
				DespawnTimer--;
				if (DespawnTimer <= 0)
				{
					for (int i = 0; i < 6; i++)
						Gore.NewGore(npc.Center, Main.rand.NextVector2Circular(2, 2), 99);

					bool placed = false;
					Point CheckTile = new Point((int)npc.Left.X / 16, (int)(npc.Left.Y + 16) / 16);
					bool CanPlaceStatue(Point CheckFrom)
					{
						return (Collision.SolidTiles(CheckFrom.X, CheckFrom.X + 1, CheckFrom.Y + 1, CheckFrom.Y + 1) || //check if the bottom two tiles are solid
							(TileID.Sets.Platforms[Framing.GetTileSafely(CheckFrom.X, CheckFrom.Y + 1).type] && TileID.Sets.Platforms[Framing.GetTileSafely(CheckFrom.X + 1, CheckFrom.Y + 1).type])) //or if they're platforms
							&& !Framing.GetTileSafely(CheckFrom.X, CheckFrom.Y).active() && !Framing.GetTileSafely(CheckFrom.X + 1, CheckFrom.Y).active() //then check if the space to put a statue in is empty, probably a better way to do this??
							&& !Framing.GetTileSafely(CheckFrom.X, CheckFrom.Y - 1).active() && !Framing.GetTileSafely(CheckFrom.X + 1, CheckFrom.Y - 1).active()
							&& !Framing.GetTileSafely(CheckFrom.X, CheckFrom.Y - 2).active() && !Framing.GetTileSafely(CheckFrom.X + 1, CheckFrom.Y - 2).active()
							&& !Framing.GetTileSafely(CheckFrom.X, CheckFrom.Y - 3).active() && !Framing.GetTileSafely(CheckFrom.X + 1, CheckFrom.Y - 3).active();
					}

					if (CanPlaceStatue(CheckTile))
					{
						WorldGen.PlaceObject(CheckTile.X, CheckTile.Y, ModContent.TileType<CursedArmor>(), direction: npc.spriteDirection);
						if (Main.netMode != NetmodeID.SinglePlayer)
							NetMessage.SendData(MessageID.TileChange, -1, -1, null, ModContent.TileType<CursedArmor>(), CheckTile.X, CheckTile.Y);

						for (int i = 0; i < 6; i++)
							Gore.NewGore(CheckTile.ToWorldCoordinates(), Main.rand.NextVector2Circular(2, 2), 99);
						placed = true;
					}
					else if (CanPlaceStatue(new Point(TileX, TileY)))
					{
						WorldGen.PlaceObject(TileX, TileY, ModContent.TileType<CursedArmor>(), direction: npc.spriteDirection);
						if (Main.netMode != NetmodeID.SinglePlayer)
							NetMessage.SendData(MessageID.TileChange, -1, -1, null, ModContent.TileType<CursedArmor>(), TileX, TileY);

						for (int i = 0; i < 6; i++)
							Gore.NewGore(new Point(TileX, TileY).ToWorldCoordinates(), Main.rand.NextVector2Circular(2, 2), 99);
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
									WorldGen.PlaceObject(checkFrom.X, checkFrom.Y, ModContent.TileType<CursedArmor>(), direction: npc.spriteDirection);
									if (Main.netMode != NetmodeID.SinglePlayer)
										NetMessage.SendData(MessageID.TileChange, -1, -1, null, ModContent.TileType<CursedArmor>(), checkFrom.X, checkFrom.Y);

									for (int i = 0; i < 6; i++)
										Gore.NewGore(checkFrom.ToWorldCoordinates(), Main.rand.NextVector2Circular(2, 2), 99);
									placed = true;
								}
							}
						}
						tries++;
						if (tries >= 8)
							break;
					}

					npc.active = false;
				}
				return;
			}

			DespawnTimer = 10;

			if (Vector2.Distance(player.Center, npc.Center) > 60f)
				Movement();
			else
				npc.velocity.X = 0f;
			FlashTimer = Math.Max(FlashTimer - 1, 0);

			Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), 72 * 0.002f, 175 * 0.002f, 206 * 0.002f);
			CheckPlatform(player);
		}

		private void CheckPlatform(Player player)
		{
			bool onPlatform = true;
			for (int i = (int)npc.position.X; i < npc.position.X + npc.width; i += npc.width / 4)
			{
				Tile tile = Framing.GetTileSafely(new Point((int)npc.position.X / 16, (int)(npc.position.Y + npc.height + 8) / 16));
				if (!TileID.Sets.Platforms[tile.type])
					onPlatform = false;
			}
			if (onPlatform && npc.Bottom.Y < player.Top.Y)
				npc.noTileCollide = true;
			else
				npc.noTileCollide = false;
		}

		public void Movement()
		{
			int num1 = 30;
			int num2 = 10;
			bool flag1 = false;
			bool flag2 = false;
			bool flag3 = false;
			if (npc.velocity.Y == 0.0 && (npc.velocity.X > 0.0 && npc.direction < 0 || npc.velocity.X < 0.0 && npc.direction > 0))
			{
				flag2 = true;
				++npc.ai[3];
			}
			if (npc.position.X == npc.oldPosition.X || npc.ai[3] >= num1 || flag2)
			{
				++npc.ai[3];
				flag3 = true;
			}
			else if (npc.ai[3] > 0.0)
			{
				--npc.ai[3];
			}

			if (npc.ai[3] > (num1 * num2))
			{
				npc.ai[3] = 0.0f;
				npc.netUpdate = true;
			}

			if (npc.justHit)
			{
				npc.ai[3] = 0.0f;
				npc.netUpdate = true;
			}

			if (npc.ai[3] == num1)
				npc.netUpdate = true;

			Vector2 vector2_1 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
			float num3 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector2_1.X;
			float num4 = Main.player[npc.target].position.Y - vector2_1.Y;
			float num5 = (float)Math.Sqrt(num3 * num3 + num4 * num4);
			if (num5 < 200.0 && !flag3)
			{
				npc.ai[3] = 0.0f;
			}

			if (npc.ai[3] < num1)
				npc.TargetClosest(true);
			else
			{
				if (npc.velocity.X == 0.0)
				{
					if (npc.velocity.Y == 0.0)
					{
						++npc.ai[0];
						if (npc.ai[0] >= 2.0)
						{
							npc.direction *= -1;
							if (npc.velocity.X < 0f)
								npc.spriteDirection = -1;
							else if (npc.velocity.X > 0f)
								npc.spriteDirection = 1;

							npc.ai[0] = 0.0f;
						}
					}
				}
				else
					npc.ai[0] = 0.0f;

				npc.directionY = -1;
				if (npc.direction == 0)
					npc.direction = 1;
			}
			float num6 = 2f; //walking speed
			float num7 = 0.5f; //regular speed (x)
			if (!flag1 && (npc.velocity.Y == 0.0 || npc.wet || npc.velocity.X <= 0.0 && npc.direction < 0 || npc.velocity.X >= 0.0 && npc.direction > 0))
			{
				if (npc.velocity.X < -num6 || npc.velocity.X > num6)
				{
					if (npc.velocity.Y == 0.0)
					{
						Vector2 vector2_2 = npc.velocity * 0.5f; // Slide speed
						npc.velocity = vector2_2;
					}
				}
				else if (npc.velocity.X < num6 && npc.direction == 1)
				{
					npc.velocity.X += num7;
					if (npc.velocity.X > num6)
					{
						npc.velocity.X = num6;
					}
				}
				else if (npc.velocity.X > -num6 && npc.direction == -1)
				{
					npc.velocity.X -= num7;
					if (npc.velocity.X < -num6)
					{
						npc.velocity.X = -num6;
					}
				}
			}
			if (npc.velocity.Y >= 0)
			{
				int num8 = 0;
				if (npc.velocity.X < 0.0)
					num8 = -1;

				if (npc.velocity.X > 0.0)
					num8 = 1;

				Vector2 position = npc.position;
				position.X += npc.velocity.X;
				int index1 = (int)((position.X + (npc.width / 2) + ((npc.width / 2 + 1) * num8)) / 16.0);
				int index2 = (int)((position.Y + npc.height - 1.0) / 16.0);

				if (Main.tile[index1, index2] == null)
					Main.tile[index1, index2] = new Tile();

				if (Main.tile[index1, index2 - 1] == null)
					Main.tile[index1, index2 - 1] = new Tile();

				if (Main.tile[index1, index2 - 2] == null)
					Main.tile[index1, index2 - 2] = new Tile();

				if (Main.tile[index1, index2 - 3] == null)
					Main.tile[index1, index2 - 3] = new Tile();

				if (Main.tile[index1, index2 + 1] == null)
					Main.tile[index1, index2 + 1] = new Tile();

				if ((index1 * 16) < position.X + npc.width && (index1 * 16 + 16) > position.X && (Main.tile[index1, index2].nactive() && !Main.tile[index1, index2].topSlope() && (!Main.tile[index1, index2 - 1].topSlope() && Main.tileSolid[(int)Main.tile[index1, index2].type]) && !Main.tileSolidTop[(int)Main.tile[index1, index2].type] || Main.tile[index1, index2 - 1].halfBrick() && Main.tile[index1, index2 - 1].nactive()) && ((!Main.tile[index1, index2 - 1].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 - 1].type] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 1].type] || Main.tile[index1, index2 - 1].halfBrick() && (!Main.tile[index1, index2 - 4].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 - 4].type] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 4].type])) && ((!Main.tile[index1, index2 - 2].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 - 2].type] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 2].type]) && (!Main.tile[index1, index2 - 3].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 - 3].type] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 3].type]) && (!Main.tile[index1 - num8, index2 - 3].nactive() || !Main.tileSolid[(int)Main.tile[index1 - num8, index2 - 3].type]))))
				{
					float num9 = (float)(index2 * 16);
					if (Main.tile[index1, index2].halfBrick())
						num9 += 8f;

					if (Main.tile[index1, index2 - 1].halfBrick())
						num9 -= 8f;

					if (num9 < position.Y + npc.height)
					{
						float num10 = position.Y + npc.height - num9;
						if (num10 <= 16.1)
						{
							npc.gfxOffY += npc.position.Y + npc.height - num9;
							npc.position.Y = num9 - npc.height;
							npc.stepSpeed = num10 >= 9.0 ? 2f : 1f;
						}
					}
				}
			}
			if (npc.velocity.Y == 0.0)
			{
				int index1 = (int)((npc.position.X + (npc.width / 2) + ((npc.width / 2 + 2) * npc.direction) + npc.velocity.X * 5.0) / 16.0);
				int index2 = (int)((npc.position.Y + npc.height - 15.0) / 16.0);
				if (Main.tile[index1, index2] == null)
					Main.tile[index1, index2] = new Tile();

				if (Main.tile[index1, index2 - 1] == null)
					Main.tile[index1, index2 - 1] = new Tile();

				if (Main.tile[index1, index2 - 2] == null)
					Main.tile[index1, index2 - 2] = new Tile();

				if (Main.tile[index1, index2 - 3] == null)
					Main.tile[index1, index2 - 3] = new Tile();

				if (Main.tile[index1, index2 + 1] == null)
					Main.tile[index1, index2 + 1] = new Tile();

				if (Main.tile[index1 + npc.direction, index2 - 1] == null)
					Main.tile[index1 + npc.direction, index2 - 1] = new Tile();

				if (Main.tile[index1 + npc.direction, index2 + 1] == null)
					Main.tile[index1 + npc.direction, index2 + 1] = new Tile();

				if (Main.tile[index1 - npc.direction, index2 + 1] == null)
					Main.tile[index1 - npc.direction, index2 + 1] = new Tile();

				int spriteDirection = npc.spriteDirection;
				if (npc.velocity.X < 0.0 && spriteDirection == -1 || npc.velocity.X > 0.0 && spriteDirection == 1)
				{
					float num8 = 3f;
					if (Main.tile[index1, index2 - 2].nactive() && Main.tileSolid[Main.tile[index1, index2 - 2].type])
					{
						if (Main.tile[index1, index2 - 3].nactive() && Main.tileSolid[Main.tile[index1, index2 - 3].type])
						{
							npc.velocity.Y = -8.5f;
							npc.netUpdate = true;
						}
						else
						{
							npc.velocity.Y = -8.5f;
							npc.netUpdate = true;
						}
					}
					else if (Main.tile[index1, index2 - 1].nactive() && !Main.tile[index1, index2 - 1].topSlope() && Main.tileSolid[(int)Main.tile[index1, index2 - 1].type])
					{
						npc.velocity.Y = -8.5f;
						npc.netUpdate = true;
					}
					else if (npc.position.Y + npc.height - (index2 * 16) > 20.0 && Main.tile[index1, index2].nactive() && (!Main.tile[index1, index2].topSlope() && Main.tileSolid[Main.tile[index1, index2].type]))
					{
						npc.velocity.Y = -8.5f;
						npc.netUpdate = true;
					}
					else if ((npc.directionY < 0 || Math.Abs(npc.velocity.X) > num8) && ((!Main.tile[index1, index2 + 2].nactive() || !Main.tileSolid[Main.tile[index1, index2 + 2].type]) && (!Main.tile[index1 + npc.direction, index2 + 3].nactive() || !Main.tileSolid[Main.tile[index1 + npc.direction, index2 + 3].type])))
					{
						npc.velocity.Y = -8.5f;
						npc.netUpdate = true;
					}
				}
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/EnchantedArmorGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/EnchantedArmorGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/EnchantedArmorGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/EnchantedArmorGore4"), 1f);
			}
			for (int k = 0; k < 10; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Iron, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Iron, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Clentaminator_Green, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}
		}

		delegate void OnTileCheck(int x, int y);

		public override void NPCLoot()
		{
			void tilecheck(int i, int j, int maxX, int maxY, int type, OnTileCheck onTileCheck)
			{
				for (int indexX = -maxX; indexX <= maxX; indexX++)
					for (int indexY = -maxY; indexY <= maxY; indexY++)
						if (Framing.GetTileSafely(indexX + i, indexY + j).type == type && Framing.GetTileSafely(indexX + i, indexY + j).frameX == 0 && Framing.GetTileSafely(indexX + i, indexY + j).frameY == 0)
							onTileCheck(indexX + i, indexY + j);
			}

			if (Main.npc.Where(x => x.active && x.type == npc.type).Count() <= 1)
			{ //only run if no cursed armors are left after this dies
				tilecheck(npc.position.ToTileCoordinates().X, npc.position.ToTileCoordinates().Y, 90, 90, ModContent.TileType<SepulchreChestTile>(), delegate (int x, int y) //first, check if any chests are nearby
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
							Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/DownedMiniboss"));
					}
				});
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) => false;

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], new Vector2(npc.Center.X, npc.Center.Y - 8) - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);

			spriteBatch.Draw(mod.GetTexture("NPCs/Enchanted_Armor/Enchanted_Armor_Glow"), new Vector2(npc.Center.X, npc.Center.Y - 8) - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);

			float maskopacity = (FlashTimer / 30) * 0.5f;
			spriteBatch.Draw(mod.GetTexture("NPCs/Enchanted_Armor/Enchanted_Armor_Mask"), new Vector2(npc.Center.X, npc.Center.Y - 8) - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 Color.White * maskopacity, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
		}

		public override void FindFrame(int frameHeight)
		{
			Player player = Main.player[npc.target];
			npc.frameCounter++;
			npc.frame.Width = 90;
			if ((double)Vector2.Distance(player.Center, npc.Center) > 60f || !player.active || player.dead)
			{
				{
					if (npc.frameCounter < 6)
					{
						npc.frame.Y = 0 * frameHeight;
						npc.frame.X = 0;
					}
					else if (npc.frameCounter < 12)
					{
						npc.frame.Y = 1 * frameHeight;
						npc.frame.X = 0;
					}
					else if (npc.frameCounter < 18)
					{
						npc.frame.Y = 2 * frameHeight;
						npc.frame.X = 0;
					}
					else if (npc.frameCounter < 24)
					{
						npc.frame.Y = 3 * frameHeight;
						npc.frame.X = 0;
					}
					else if (npc.frameCounter < 30)
					{
						npc.frame.Y = 4 * frameHeight;
						npc.frame.X = 0;
					}
					else
						npc.frameCounter = 0;
				}
			}
			else
			{
				if (npc.frameCounter < 5)
				{
					npc.frame.Y = 5 * frameHeight;
					npc.frame.X = 0;
				}
				else if (npc.frameCounter < 10)
				{
					npc.frame.Y = 6 * frameHeight;
					npc.frame.X = 0;
				}
				else if (npc.frameCounter < 15)
				{
					npc.frame.Y = 7 * frameHeight;
					npc.frame.X = 0;
				}
				else if (npc.frameCounter < 20)
				{
					npc.frame.Y = 8 * frameHeight;
					npc.frame.X = 0;
				}
				else if (npc.frameCounter < 25)
				{
					npc.frame.Y = 9 * frameHeight;
					npc.frame.X = 0;
				}
				else if (npc.frameCounter < 30)
				{
					npc.frame.Y = 10 * frameHeight;
					npc.frame.X = 0;
				}
				else if (npc.frameCounter < 35)
				{
					if (npc.frameCounter == 30 && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
					{
						player.Hurt(PlayerDeathReason.LegacyDefault(), (int)(npc.damage * 1.5f), npc.direction, false, false, false, -1);
						npc.frame.Y = 9 * frameHeight;
					}
					npc.frame.Y = 11 * frameHeight;
					npc.frame.X = 0;
				}
				else
					npc.frameCounter = 0;
			}
		}
	}
}