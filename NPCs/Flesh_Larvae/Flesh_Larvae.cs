using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.NPCs.Flesh_Larvae
{
	public class Flesh_Larvae : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flesh Larvae");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 75;
			npc.defense = 20;
			npc.value = 500f;
			aiType = 0;
			npc.knockBackResist = 0.6f;
			npc.width = 40;
			npc.height = 24;
			npc.damage = 25;
			npc.lavaImmune = false;
			npc.noTileCollide = false;
			npc.alpha = 0;
			npc.HitSound = new Terraria.Audio.LegacySoundStyle(3, 1);
			npc.dontTakeDamage = false;
			npc.DeathSound = new Terraria.Audio.LegacySoundStyle(4, 11);
		}

		public override void AI()
		{
			Player player = Main.player[npc.target];

			npc.TargetClosest(true);

			if (Vector2.Distance(player.Center, npc.Center) < 500f)
			{
				if (npc.alpha <= 200 && npc.alpha >= 5)
					npc.alpha -= 5;

				Movement();
				if (npc.velocity.X < 0f)
					npc.spriteDirection = -1;
				else if (npc.velocity.X > 0f)
					npc.spriteDirection = 1;

				npc.dontTakeDamage = false;
				npc.damage = 25;
				int num = Main.rand.Next(1, 3);
				if (Vector2.Distance(player.Center, npc.Center) < 350f)
				{
					for (int index1 = 0; index1 < num; ++index1)
					{
						if (npc.velocity.X != 0f)
						{
							if (Vector2.Distance(player.Center, npc.Center) < 150f)
							{
								player.AddBuff(22, 2);
								player.AddBuff(32, 2);
								if (Main.rand.Next(300) == 0)
								{
									player.AddBuff(31, 2);
								}
							}
						}
						else if (npc.velocity.X == 0f)
						{
							if (Vector2.Distance(player.Center, npc.Center) < 50f)
							{
								player.AddBuff(22, 2);
								player.AddBuff(32, 2);
								if (Main.rand.Next(300) == 0)
								{
									player.AddBuff(31, 2);
								}
							}
						}
						int index2 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Smoke, 0.0f, 0.0f, 100, Color.Pink, 0.4f);
						Main.dust[index2].alpha += Main.rand.Next(100);
						Main.dust[index2].velocity *= 0.3f;
						Main.dust[index2].velocity.X += Main.rand.Next(-10, 11) * 0.025f * npc.velocity.X;
						Main.dust[index2].velocity.Y -= (float)(0.400000005960464 + Main.rand.Next(-3, 14) * 0.150000005960464);
						Main.dust[index2].fadeIn = (float)(0.25 + Main.rand.Next(10) * 0.150000005960464);
					}
				}
			}
			else
			{
				npc.velocity.X = 0f;
				if (npc.alpha < 200)
					npc.alpha += 5;

				npc.dontTakeDamage = true;
				npc.damage = 0;
			}
		}

		public void Movement()
		{
			Player player = Main.player[npc.target];

			bool flag2 = false;
			bool flag3 = false;

			if (npc.velocity.Y == 0.0 && (npc.velocity.X > 0.0 && npc.direction < 0 || npc.velocity.X < 0.0 && npc.direction > 0))
			{
				flag2 = true;
				++npc.ai[3];
			}

			if (npc.position.X == npc.oldPosition.X || npc.ai[3] >= 30 || flag2)
			{
				++npc.ai[3];
				flag3 = true;
			}
			else if (npc.ai[3] > 0.0)
				--npc.ai[3];

			if (npc.ai[3] > 300)
				npc.ai[3] = 0.0f;

			if (npc.justHit)
				npc.ai[3] = 0.0f;

			if (npc.ai[3] == 30)
				npc.netUpdate = true;

			if (npc.DistanceSQ(player.Center) < 200 * 200 && !flag3)
				npc.ai[3] = 0.0f;

			if (npc.ai[3] < 30)
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

			float walkSpeed = 3f; //walking speed

			if (npc.DistanceSQ(player.Center) < 50 * 50)
				walkSpeed = 5f;
			else if (npc.DistanceSQ(player.Center) >= 50 * 50 && npc.DistanceSQ(player.Center) < 200 * 200)
				walkSpeed = 2.5f;
			else if (npc.DistanceSQ(player.Center) >= 200 * 200 && npc.DistanceSQ(player.Center) < 300 * 300)
				walkSpeed = 2f;
			else if (npc.DistanceSQ(player.Center) >= 300 * 300 && npc.DistanceSQ(player.Center) < 400 * 400)
				walkSpeed = 1.5f;
			else if (npc.DistanceSQ(player.Center) >= 400 * 400)
				walkSpeed = 1f;

			const float SpeedDecrease = 0.08f;
			if (npc.velocity.Y == 0.0 || npc.wet || npc.velocity.X <= 0.0 && npc.direction < 0 || npc.velocity.X >= 0.0 && npc.direction > 0)
			{
				if (npc.velocity.X < -walkSpeed || npc.velocity.X > walkSpeed)
				{
					if (npc.velocity.Y == 0.0)
						npc.velocity *= 0.5f;
				}
				else if (npc.velocity.X < walkSpeed && npc.direction == 1)
				{
					npc.velocity.X -= SpeedDecrease;
					if (npc.velocity.X > walkSpeed)
						npc.velocity.X = -walkSpeed;
				}
				else if (npc.velocity.X > -walkSpeed && npc.direction == -1)
				{
					npc.velocity.X += SpeedDecrease;
					if (npc.velocity.X < -walkSpeed)
						npc.velocity.X = walkSpeed;
				}
			}

			if (npc.velocity.Y >= 0.0)
			{
				int num8 = 0;

				if (npc.velocity.X < 0.0)
					num8 = -1;
				else if (npc.velocity.X > 0.0)
					num8 = 1;

				Vector2 position = npc.position;
				position.X += npc.velocity.X;
				int index1 = (int)((position.X + (npc.width / 2f) + ((npc.width / 2f + 1) * num8)) / 16f);
				int index2 = (int)((position.Y + npc.height - 1) / 16f);

				if (Main.tile[index1, index2 + 1] == null)
					Main.tile[index1, index2 + 1] = new Tile();

				Tile currentTile = Framing.GetTileSafely(index1, index2);
				Tile tileAbove = Framing.GetTileSafely(index1, index2 - 1);
				Tile tile2Above = Framing.GetTileSafely(index1, index2 - 2);
				Tile tile3Above = Framing.GetTileSafely(index1, index2 - 3);
				Tile tile4Above = Framing.GetTileSafely(index1, index2 - 4);

				bool NActiveTopSlope(Tile t) => t.nactive() && !t.topSlope();
				bool NonSolidOrSolidTop(Tile t) => !Main.tileSolid[t.type] || Main.tileSolidTop[t.type];

				if ((index1 * 16) < position.X + npc.width && (index1 * 16 + 16) > position.X && (NActiveTopSlope(currentTile) && 
					(!tileAbove.topSlope() && Main.tileSolid[currentTile.type] && !Main.tileSolidTop[currentTile.type] || NActiveTopSlope(tileAbove)) && tileAbove.nactive() ||
					NonSolidOrSolidTop(tileAbove) || tileAbove.halfBrick() && (!tile4Above.nactive() ||
					NonSolidOrSolidTop(tile4Above)) && ((!tile2Above.nactive() ||
					NonSolidOrSolidTop(tile2Above)) && (!tile3Above.nactive() ||
					NonSolidOrSolidTop(tile3Above)) && (!Main.tile[index1 - num8, index2 - 3].nactive() || 
					!Main.tileSolid[Main.tile[index1 - num8, index2 - 3].type])))) //this is one of the worst if statements I've ever seen
				{
					float num9 = index2 * 16;
					if (currentTile.halfBrick())
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
							npc.stepSpeed = num10 >= 9.0 ? 4f : 2f;
						}
					}
				}
			}

			if (npc.velocity.Y == 0.0)
			{
				int index1 = (int)((npc.position.X + (npc.width / 2) + (15 * npc.direction)) / 16.0);
				int index2 = (int)((npc.position.Y + npc.height - 15.0) / 16.0);

				if (Main.tile[index1 + npc.direction, index2 - 1] == null)
					Main.tile[index1 + npc.direction, index2 - 1] = new Tile();

				if (Main.tile[index1 + npc.direction, index2 + 1] == null)
					Main.tile[index1 + npc.direction, index2 + 1] = new Tile();

				if (Main.tile[index1 - npc.direction, index2 + 1] == null)
					Main.tile[index1 - npc.direction, index2 + 1] = new Tile();

				Tile currentTile = Framing.GetTileSafely(index1, index2);
				Tile tileAbove = Framing.GetTileSafely(index1, index2 - 1);
				Tile tile2Above = Framing.GetTileSafely(index1, index2 - 2);
				Tile tile3Above = Framing.GetTileSafely(index1, index2 - 3);
				Tile tileBelow = Framing.GetTileSafely(index1, index2 + 1);

				tileBelow.halfBrick();

				if (npc.velocity.X < 0.0 || npc.velocity.X > 0.0)
				{
					if (npc.height >= 24 && tile2Above.nactive() && Main.tileSolid[tile2Above.type])
					{
						if (Main.tile[index1, index2 - 3].nactive() && Main.tileSolid[tile3Above.type])
						{
							npc.velocity.Y = -8f;
							npc.netUpdate = true;
						}
						else
						{
							npc.velocity.Y = -8f;
							npc.netUpdate = true;
						}
					}
					else if (tileAbove.nactive() && Main.tileSolid[tileAbove.type])
					{
						npc.velocity.Y = -8f;
						npc.netUpdate = true;
					}
					else if (npc.position.Y + npc.height - (index2 * 16) > 10.0 && tileAbove.nactive() && (!currentTile.topSlope() && Main.tileSolid[currentTile.type]))
					{
						npc.velocity.Y = -8f;
						npc.netUpdate = true;
					}
					else if (npc.directionY < 0 && (!tileBelow.nactive() || !Main.tileSolid[tileBelow.type]) && (!Main.tile[index1 + npc.direction, index2 + 1].nactive() || !Main.tileSolid[Main.tile[index1 + npc.direction, index2 + 1].type]))
					{
						npc.velocity.Y = -8f;
						npc.velocity.X *= 1.5f;
						npc.netUpdate = true;
					}
				}
			}

			if (npc.velocity.X == 0f)
				npc.velocity.Y = -8f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
				for (int i = 1; i < 5; ++i)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LarvaeGore" + i), 1f);

			for (int k = 0; k < 7; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => NPC.downedBoss2 ? SpawnCondition.Crimson.Chance * 0.15f : 0f;

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter++;

			if (npc.DistanceSQ(Main.player[npc.target].Center) < 500 * 500)
			{
				if (npc.frameCounter < 7)
					npc.frame.Y = 0 * frameHeight;
				else if (npc.frameCounter < 14)
					npc.frame.Y = 1 * frameHeight;
				else if (npc.frameCounter < 21)
					npc.frame.Y = 2 * frameHeight;
				else if (npc.frameCounter < 28)
					npc.frame.Y = 3 * frameHeight;
				else if (npc.frameCounter < 35)
					npc.frame.Y = 4 * frameHeight;
				else if (npc.frameCounter < 42)
					npc.frame.Y = 5 * frameHeight;
				else if (npc.frameCounter < 49)
					npc.frame.Y = 6 * frameHeight;
				else if (npc.frameCounter < 56)
					npc.frame.Y = 7 * frameHeight;
				else
					npc.frameCounter = 0;
			}
			else
				npc.frame.Y = 2 * frameHeight;
		}
	}
}