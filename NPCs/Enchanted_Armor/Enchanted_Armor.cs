using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using System.Security.Cryptography;
using System.IO;
using MonoMod.Utils;
using SpiritMod.Projectiles.Clubs;
using SpiritMod.Sepulchre;
using System.Linq;

namespace SpiritMod.NPCs.Enchanted_Armor
{
	public class Enchanted_Armor : ModNPC
	{
		ref float FlashTimer => ref npc.ai[1];

		ref float DespawnTimer => ref npc.localAI[0];

		private int TileX {
			get => (int)npc.localAI[1] / 16;
			set => npc.localAI[1] = 16 * value;
		}
		private int TileY {
			get => (int)npc.localAI[2] / 16;
			set => npc.localAI[2] = 16 * value;
		}

		private bool SetTiles = false;

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
			npc.lavaImmune = false;
			npc.noTileCollide = false;
			npc.noGravity = false;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath6;
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
			if((npc.localAI[3] += 2) == 2) { //localai3 was decreasing every tick and i couldnt pinpoint why so i just took the lazy route out and made it increase more to counter it out
				DespawnTimer = 10;
				npc.localAI[1] = npc.Left.X;
				npc.localAI[2] = npc.Left.Y + 16;
				npc.netUpdate = true;
			}

			if (!player.active || player.dead || npc.Distance(player.Center) > 1200) {
				movement();
				DespawnTimer--;
				if(DespawnTimer <= 0) {
					for(int i = 0; i < 7; i++) 
						Gore.NewGore(npc.Center, Main.rand.NextVector2Circular(5, 5), 11 + Main.rand.Next(2));

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

					if (CanPlaceStatue(CheckTile)) {
						WorldGen.PlaceObject(CheckTile.X, CheckTile.Y, ModContent.TileType<CursedArmor>(), direction: npc.spriteDirection);
						placed = true;
					}
					else if (CanPlaceStatue(new Point(TileX, TileY))) {
						WorldGen.PlaceObject(TileX, TileY, ModContent.TileType<CursedArmor>(), direction: npc.spriteDirection);
						placed = true;
					}
					int tries = 0;
					while(!placed) {
						for(int indexX = -20; indexX <= 20; indexX++) {
							for (int indexY = -20; indexY <= 20; indexY++) {
								var checkFrom = new Point(indexX + CheckTile.X, indexY + CheckTile.Y);
								if (CanPlaceStatue(checkFrom) && !placed && Main.rand.NextBool(7)) {
									WorldGen.PlaceObject(checkFrom.X, checkFrom.Y, ModContent.TileType<CursedArmor>(), direction: npc.spriteDirection);
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

			if ((double)Vector2.Distance(player.Center, npc.Center) > (double)60f)
			{
				movement();
			}
			else
			{
				npc.velocity.X = 0f;
			}
			FlashTimer = Math.Max(FlashTimer - 1, 0);

			Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), 72*0.002f, 175*0.002f, 206*0.002f);
			CheckPlatform(player);
		}

		private void CheckPlatform(Player player)
		{
			bool onplatform = true;
			for (int i = (int)npc.position.X; i < npc.position.X + npc.width; i += npc.width / 4) {
				Tile tile = Framing.GetTileSafely(new Point((int)npc.position.X / 16, (int)(npc.position.Y + npc.height + 8) / 16));
				if (!TileID.Sets.Platforms[tile.type])
					onplatform = false;
			}
			if (onplatform && npc.Bottom.Y < player.Top.Y)
				npc.noTileCollide = true;
			else
				npc.noTileCollide = false;
		}

		public void movement()
		{
			int num1 = 30;
			int num2 = 10;
			bool flag1 = false;
			bool flag2 = false;
			bool flag3 = false;
			if ((double)npc.velocity.Y == 0.0 && ((double)npc.velocity.X > 0.0 && npc.direction < 0 || (double)npc.velocity.X < 0.0 && npc.direction > 0))
			{
				flag2 = true;
				++npc.ai[3];
			}
			if ((double)npc.position.X == (double)npc.oldPosition.X || (double)npc.ai[3] >= (double)num1 || flag2)
			{
				++npc.ai[3];
				flag3 = true;
			}
			else if ((double)npc.ai[3] > 0.0)
			{
				--npc.ai[3];
			}

			if ((double)npc.ai[3] > (double)(num1 * num2))
			{
				npc.ai[3] = 0.0f;
				npc.netUpdate = true;
			}

			if (npc.justHit)
			{
				npc.ai[3] = 0.0f;
				npc.netUpdate = true;
			}

			if ((double)npc.ai[3] == (double)num1)
			{
				npc.netUpdate = true;
			}

			Vector2 vector2_1 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
			float num3 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector2_1.X;
			float num4 = Main.player[npc.target].position.Y - vector2_1.Y;
			float num5 = (float)Math.Sqrt((double)num3 * (double)num3 + (double)num4 * (double)num4);
			if ((double)num5 < 200.0 && !flag3)
			{
				npc.ai[3] = 0.0f;	
			}

			if ((double)npc.ai[3] < (double)num1)
			{
				npc.TargetClosest(true);
			}
			else
			{
				if ((double)npc.velocity.X == 0.0)
				{
					if ((double)npc.velocity.Y == 0.0)
					{
						++npc.ai[0];
						if ((double)npc.ai[0] >= 2.0)
						{
							npc.direction *= -1;
							if (npc.velocity.X < 0f)
							{
								npc.spriteDirection = -1;
							}
							else if (npc.velocity.X > 0f)
							{
								npc.spriteDirection = 1;
							}

							npc.ai[0] = 0.0f;
						}
					}
				}
				else
				{
					npc.ai[0] = 0.0f;
				}

				npc.directionY = -1;
				if (npc.direction == 0)
				{
					npc.direction = 1;
				}
			}
			float num6 = 2f; //walking speed
			float num7 = 0.5f; //regular speed (x)
			if (!flag1 && ((double)npc.velocity.Y == 0.0 || npc.wet || (double)npc.velocity.X <= 0.0 && npc.direction < 0 || (double)npc.velocity.X >= 0.0 && npc.direction > 0))
			{
				if ((double)npc.velocity.X < -(double)num6 || (double)npc.velocity.X > (double)num6)
				{
					if ((double)npc.velocity.Y == 0.0)
					{
						Vector2 vector2_2 = npc.velocity * 0.5f; ///////SLIDE SPEED
						npc.velocity = vector2_2;
					}
				}
				else if ((double)npc.velocity.X < (double)num6 && npc.direction == 1)
				{
					npc.velocity.X += num7;
					if ((double)npc.velocity.X > (double)num6)
					{
						npc.velocity.X = num6;
					}
				}
				else if ((double)npc.velocity.X > -(double)num6 && npc.direction == -1)
				{
					npc.velocity.X -= num7;
					if ((double)npc.velocity.X < -(double)num6)
					{
						npc.velocity.X = -num6;
					}
				}
			}
			if ((double)npc.velocity.Y >= 0.0)
			{
				int num8 = 0;
				if ((double)npc.velocity.X < 0.0)
				{
					num8 = -1;
				}

				if ((double)npc.velocity.X > 0.0)
				{
					num8 = 1;
				}

				Vector2 position = npc.position;
				position.X += npc.velocity.X;
				int index1 = (int)(((double)position.X + (double)(npc.width / 2) + (double)((npc.width / 2 + 1) * num8)) / 16.0);
				int index2 = (int)(((double)position.Y + (double)npc.height - 1.0) / 16.0);
				if (Main.tile[index1, index2] == null)
				{
					Main.tile[index1, index2] = new Tile();
				}

				if (Main.tile[index1, index2 - 1] == null)
				{
					Main.tile[index1, index2 - 1] = new Tile();
				}

				if (Main.tile[index1, index2 - 2] == null)
				{
					Main.tile[index1, index2 - 2] = new Tile();
				}

				if (Main.tile[index1, index2 - 3] == null)
				{
					Main.tile[index1, index2 - 3] = new Tile();
				}

				if (Main.tile[index1, index2 + 1] == null)
				{
					Main.tile[index1, index2 + 1] = new Tile();
				}

				if ((double)(index1 * 16) < (double)position.X + (double)npc.width && (double)(index1 * 16 + 16) > (double)position.X && (Main.tile[index1, index2].nactive() && !Main.tile[index1, index2].topSlope() && (!Main.tile[index1, index2 - 1].topSlope() && Main.tileSolid[(int)Main.tile[index1, index2].type]) && !Main.tileSolidTop[(int)Main.tile[index1, index2].type] || Main.tile[index1, index2 - 1].halfBrick() && Main.tile[index1, index2 - 1].nactive()) && ((!Main.tile[index1, index2 - 1].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 - 1].type] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 1].type] || Main.tile[index1, index2 - 1].halfBrick() && (!Main.tile[index1, index2 - 4].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 - 4].type] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 4].type])) && ((!Main.tile[index1, index2 - 2].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 - 2].type] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 2].type]) && (!Main.tile[index1, index2 - 3].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 - 3].type] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 3].type]) && (!Main.tile[index1 - num8, index2 - 3].nactive() || !Main.tileSolid[(int)Main.tile[index1 - num8, index2 - 3].type]))))
				{
					float num9 = (float)(index2 * 16);
					if (Main.tile[index1, index2].halfBrick())
					{
						num9 += 8f;
					}

					if (Main.tile[index1, index2 - 1].halfBrick())
					{
						num9 -= 8f;
					}

					if ((double)num9 < (double)position.Y + (double)npc.height)
					{
						float num10 = position.Y + (float)npc.height - num9;
						if ((double)num10 <= 16.1)
						{
							npc.gfxOffY += npc.position.Y + (float)npc.height - num9;
							npc.position.Y = num9 - (float)npc.height;
							npc.stepSpeed = (double)num10 >= 9.0 ? 2f : 1f;
						}
					}
				}
			}
			if ((double)npc.velocity.Y == 0.0)
			{
				int index1 = (int)(((double)npc.position.X + (double)(npc.width / 2) + (double)((npc.width / 2 + 2) * npc.direction) + (double)npc.velocity.X * 5.0) / 16.0);/////////
				int index2 = (int)(((double)npc.position.Y + (double)npc.height - 15.0) / 16.0);
				if (Main.tile[index1, index2] == null)
				{
					Main.tile[index1, index2] = new Tile();
				}

				if (Main.tile[index1, index2 - 1] == null)
				{
					Main.tile[index1, index2 - 1] = new Tile();
				}

				if (Main.tile[index1, index2 - 2] == null)
				{
					Main.tile[index1, index2 - 2] = new Tile();
				}

				if (Main.tile[index1, index2 - 3] == null)
				{
					Main.tile[index1, index2 - 3] = new Tile();
				}

				if (Main.tile[index1, index2 + 1] == null)
				{
					Main.tile[index1, index2 + 1] = new Tile();
				}

				if (Main.tile[index1 + npc.direction, index2 - 1] == null)
				{
					Main.tile[index1 + npc.direction, index2 - 1] = new Tile();
				}

				if (Main.tile[index1 + npc.direction, index2 + 1] == null)
				{
					Main.tile[index1 + npc.direction, index2 + 1] = new Tile();
				}

				if (Main.tile[index1 - npc.direction, index2 + 1] == null)
				{
					Main.tile[index1 - npc.direction, index2 + 1] = new Tile();
				}

				int spriteDirection = npc.spriteDirection;
				if ((double)npc.velocity.X < 0.0 && spriteDirection == -1 || (double)npc.velocity.X > 0.0 && spriteDirection == 1)
				{
					bool flag4 = npc.type == 410 || npc.type == 423;
					float num8 = 3f;
					if (Main.tile[index1, index2 - 2].nactive() && Main.tileSolid[(int)Main.tile[index1, index2 - 2].type])
					{
						if (Main.tile[index1, index2 - 3].nactive() && Main.tileSolid[(int)Main.tile[index1, index2 - 3].type])
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
					else if ((double)npc.position.Y + (double)npc.height - (double)(index2 * 16) > 20.0 && Main.tile[index1, index2].nactive() && (!Main.tile[index1, index2].topSlope() && Main.tileSolid[(int)Main.tile[index1, index2].type]))
					{
						npc.velocity.Y = -8.5f;
						npc.netUpdate = true;
					}
					else if ((npc.directionY < 0 || (double)Math.Abs(npc.velocity.X) > (double)num8) && (!flag4 || !Main.tile[index1, index2 + 1].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 + 1].type]) && ((!Main.tile[index1, index2 + 2].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 + 2].type]) && (!Main.tile[index1 + npc.direction, index2 + 3].nactive() || !Main.tileSolid[(int)Main.tile[index1 + npc.direction, index2 + 3].type])))
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
				Dust.NewDust(npc.position, npc.width, npc.height, 8, 2.5f * hitDirection, -2.5f, 0, default(Color), 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, 8, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, 110, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.7f);
			}
		}

		public override void NPCLoot()
		{
			if(Main.npc.Where(x => x.active && x.type == npc.type).Count() <= 1) {
				int i = (int)npc.position.X / 16;
				int j = (int)npc.position.Y / 16;
				for (int indexX = -70; indexX <= 70; indexX++) {
					for (int indexY = -90; indexY <= 90; indexY++) {
						if (Framing.GetTileSafely(indexX + i, indexY + j).type == mod.TileType("SepulchreChestTile") && Framing.GetTileSafely(indexX + i, indexY + j).frameX == 0 && Framing.GetTileSafely(indexX + i, indexY + j).frameY == 0) {
							CombatText.NewText(new Rectangle((indexX + i) * 16, (indexY + j) * 16, 20, 10), Color.GreenYellow, "Unlocked!");
						}
					}
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) => false;

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], new Vector2(npc.Center.X, npc.Center.Y-8) - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
							 
			spriteBatch.Draw(mod.GetTexture("NPCs/Enchanted_Armor/Enchanted_Armor_Glow"), new Vector2(npc.Center.X, npc.Center.Y-8) - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);

			float maskopacity = (FlashTimer / 30) * 0.5f;
			spriteBatch.Draw(mod.GetTexture("NPCs/Enchanted_Armor/Enchanted_Armor_Mask"), new Vector2(npc.Center.X, npc.Center.Y - 8) - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 Color.White * maskopacity, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
		}
		
		public override void FindFrame(int frameHeight)
		{
			const int Frame_1= 0;
			const int Frame_2= 1;
			const int Frame_3= 2;
			const int Frame_4= 3;
			const int Frame_5= 4;
			const int Frame_6= 5;
			const int Frame_7= 6;
			const int Frame_8= 7;
			const int Frame_9= 8;
			const int Frame_10= 9;
			const int Frame_11= 10;
			const int Frame_12= 11;

			Player player = Main.player[npc.target];
			npc.frameCounter++;
			npc.frame.Width = 90;
			if ((double)Vector2.Distance(player.Center, npc.Center) > (double)60f || !player.active || player.dead)
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
					{
						npc.frameCounter = 0;
					}
				}
			}
			else
			{
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
							npc.frame.Y = Frame_10 * frameHeight;
						}
						npc.frame.Y = 11 * frameHeight;
						npc.frame.X = 0;
					}
					else
					{
						npc.frameCounter = 0;
					}
				}
			}
		}
	}
}