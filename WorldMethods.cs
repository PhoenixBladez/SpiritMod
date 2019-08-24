using System.IO;
using System.Collections.Generic;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;

namespace SpiritMod
{
	public class WorldMethods
	{
		public static void RoundHill(int X, int Y, int Xmult, int Ymult, int strength, bool initialplace, ushort type)
		{
			if (initialplace)
			{
				WorldMethods.TileRunner(X, Y, (double)strength * 5, 1, type, true, 0f, 0f, true, true);
			}
			for (int rotation2 = 0; rotation2 < 350; rotation2++)
			{
				int DistX = (int)(0 - (Math.Sin(rotation2) * Xmult));
				int DistY = (int)(0 - (Math.Cos(rotation2) * Ymult));
				WorldMethods.TileRunner(X + DistX, Y + DistY, (double)strength, 1, type, true, 0f, 0f, true, true);
			}
		}

		public static void RoundHill2(int X, int Y, int Xmult, int Ymult, int strength, bool initialplace, ushort type)
		{
			if (initialplace)
			{
				WorldMethods.TileRunner(X, Y, (double)strength * 5, 1, type, false, 0f, 0f, true, true);
			}
			for (int rotation2 = 0; rotation2 < 350; rotation2++)
			{
				int DistX = (int)(0 - (Math.Sin(rotation2) * Xmult));
				int DistY = (int)(0 - (Math.Cos(rotation2) * Ymult));
				WorldMethods.TileRunner(X + DistX, Y + DistY, (double)strength, 1, type, false, 0f, 0f, true, true);
			}
		}

		public static void CragSpike(int X, int Y, int length, int height, ushort type2, float slope, float sloperight)
		{
			float trueslope = 1 / slope;
			float truesloperight = 1 / sloperight;
			int Xstray = length / 2;
			for (int level = 0; level <= height; level++)
			{
				Main.tile[X, (int)(Y + level - (slope / 2))].active(true);
				Main.tile[X, (int)(Y + level - (slope / 2))].type = type2;
				for (int I = X - (int)(length + (level * trueslope)); I < X + (int)(length + (level * truesloperight)); I++)
				{
					//		if (Main.tile[(int)I, (int)(Y + level)].type != replacetile || replace)
					//	{
					Main.tile[(int)I, (int)(Y + level)].active(true);
					Main.tile[(int)I, (int)(Y + level)].type = type2;
					//}
				}
			}
		}

		public static void Island(int X, int Y, int Xsize, float slope, ushort tile)
		{
			for (int xAxis = X; xAxis < X + Xsize; xAxis++)
			{
				int Slope2 = (int)(Math.Abs(Main.rand.Next((int)(Xsize / 2) - (int)(Xsize / 12), (int)(Xsize / 2) + (int)(Xsize / 12)) - Math.Abs((xAxis - X) - Main.rand.Next((int)(Xsize / 2) - (int)(Xsize / 12), (int)(Xsize / 2) + (int)(Xsize / 12)))) * slope);
				string SlopeText = Slope2.ToString();
				//Main.NewText(SlopeText, Color.Orange.R, Color.Orange.G, Color.Orange.B);
				for (int I = 0; I < Slope2; I++)
				{
					WorldMethods.TileRunner(xAxis, Y + I, Xsize / 5, 1, tile, true, 0f, 0f, true, true);
				}
				WorldMethods.TileRunner(xAxis, Y, Xsize / 5, 1, tile, true, 0f, 0f, true, true);
				if (Main.rand.Next(5) == 0)
				{
					WorldMethods.RoundHill(xAxis, Y, Xsize / 8, Xsize / 12, Xsize / 5, true, tile);

				}
				//   if (Main.rand.Next(55) == 0)
				//   {
				//      WorldMethods.RoundHill(xAxis, Y - 7, 22, 14, 15, true, tile);

				//  }
			}
		}

		public static void RoundHole(int X, int Y, int Xmult, int Ymult, int strength, bool initialdig)
		{
			if (initialdig)
			{
				WorldGen.digTunnel(X, Y, 0, 0, strength, strength, false);
			}
			for (int rotation2 = 0; rotation2 < 350; rotation2++)
			{
				int DistX = (int)(0 - (Math.Sin(rotation2) * Xmult));
				int DistY = (int)(0 - (Math.Cos(rotation2) * Ymult));

				WorldGen.digTunnel(X + DistX, Y + DistY, 0, 0, strength, strength, false);
			}
		}

		public static void TileRunner(int i, int j, double strength, int steps, int type, bool addTile = false, float speedX = 0f, float speedY = 0f, bool noYChange = false, bool overRide = true)
		{
			double num = strength;
			float num2 = (float)steps;
			Vector2 pos;
			pos.X = (float)i;
			pos.Y = (float)j;
			Vector2 randVect;
			randVect.X = (float)WorldGen.genRand.Next(-10, 11) * 0.1f;
			randVect.Y = (float)WorldGen.genRand.Next(-10, 11) * 0.1f;
			if (speedX != 0f || speedY != 0f)
			{
				randVect.X = speedX;
				randVect.Y = speedY;
			}

			while (num > 0.0 && num2 > 0f)
			{
				if (pos.Y < 0f && num2 > 0f && type == 59)
				{
					num2 = 0f;
				}
				num = strength * (double)(num2 / (float)steps);
				num2 -= 1f;
				int num3 = (int)((double)pos.X - num * 0.5);
				int num4 = (int)((double)pos.X + num * 0.5);
				int num5 = (int)((double)pos.Y - num * 0.5);
				int num6 = (int)((double)pos.Y + num * 0.5);
				if (num3 < 1)
				{
					num3 = 1;
				}
				if (num5 < 1)
				{
					num5 = 1;
				}
				if (num6 > Main.maxTilesY - 1)
				{
					num6 = Main.maxTilesY - 1;
				}
				for (int k = num3; k < num4; k++)
				{
					for (int l = num5; l < num6; l++)
					{
						if ((double)(Math.Abs((float)k - pos.X) + Math.Abs((float)l - pos.Y)) < strength * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015))
						{

							if (type < 0)
							{
								if (type == -2 && Main.tile[k, l].active() && (l < WorldGen.waterLine || l > WorldGen.lavaLine))
								{
									Main.tile[k, l].liquid = 255;
									if (l > WorldGen.lavaLine)
									{
										Main.tile[k, l].lava(true);
									}
								}
								Main.tile[k, l].active(false);
							}
							else
							{
								if (overRide || !Main.tile[k, l].active())
								{
									Tile tile = Main.tile[k, l];
									bool flag3 = Main.tileStone[type] && tile.type != 1;
									if (!TileID.Sets.CanBeClearedDuringGeneration[(int)tile.type])
									{
										flag3 = true;
									}
									ushort type2 = tile.type;
									if (type2 <= 147)
									{
										if (type2 <= 45)
										{
											if (type2 != 1)
											{
												if (type2 == 45)
												{
													goto IL_575;
												}
											}
											else if (type == 59 && (double)l < Main.worldSurface + (double)WorldGen.genRand.Next(-50, 50))
											{
												flag3 = true;
											}
										}
										else if (type2 != 53)
										{
											if (type2 == 147)
											{
												goto IL_575;
											}
										}
										else
										{
											if (type == 40)
											{
												flag3 = true;
											}
											if ((double)l < Main.worldSurface && type != 59)
											{
												flag3 = true;
											}
										}
									}
									else if (type2 <= 196)
									{
										switch (type2)
										{
											case 189:
											case 190:
												goto IL_575;
											default:
												if (type2 == 196)
												{
													goto IL_575;
												}
												break;
										}
									}
									else
									{
										switch (type2)
										{
											case 367:
											case 368:
												if (type == 59)
												{
													flag3 = true;
												}
												break;
											default:
												switch (type2)
												{
													case 396:
													case 397:
														flag3 = !TileID.Sets.Ore[type];
														break;
												}
												break;
										}
									}
IL_5B7:
									if (!flag3)
									{
										tile.type = (ushort)type;
										goto IL_5C5;
									}
									goto IL_5C5;
IL_575:
									flag3 = true;
									goto IL_5B7;
								}
IL_5C5:
								if (addTile)
								{
									Main.tile[k, l].active(true);
									Main.tile[k, l].liquid = 0;
									Main.tile[k, l].lava(false);
								}
								if (type == 59 && l > WorldGen.waterLine && Main.tile[k, l].liquid > 0)
								{
									Main.tile[k, l].lava(false);
									Main.tile[k, l].liquid = 0;
								}
							}
						}
					}
				}
				pos += randVect;
				if (num > 50.0)
				{
					pos += randVect;
					num2 -= 1f;
					randVect.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
					randVect.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
					if (num > 100.0)
					{
						pos += randVect;
						num2 -= 1f;
						randVect.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
						randVect.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
						if (num > 150.0)
						{
							pos += randVect;
							num2 -= 1f;
							randVect.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
							randVect.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
							if (num > 200.0)
							{
								pos += randVect;
								num2 -= 1f;
								randVect.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
								randVect.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
								if (num > 250.0)
								{
									pos += randVect;
									num2 -= 1f;
									randVect.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
									randVect.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
									if (num > 300.0)
									{
										pos += randVect;
										num2 -= 1f;
										randVect.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
										randVect.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
										if (num > 400.0)
										{
											pos += randVect;
											num2 -= 1f;
											randVect.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
											randVect.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
											if (num > 500.0)
											{
												pos += randVect;
												num2 -= 1f;
												randVect.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
												randVect.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
												if (num > 600.0)
												{
													pos += randVect;
													num2 -= 1f;
													randVect.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
													randVect.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
													if (num > 700.0)
													{
														pos += randVect;
														num2 -= 1f;
														randVect.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
														randVect.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
														if (num > 800.0)
														{
															pos += randVect;
															num2 -= 1f;
															randVect.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
															randVect.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
															if (num > 900.0)
															{
																pos += randVect;
																num2 -= 1f;
																randVect.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
																randVect.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				randVect.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
				if (randVect.X > 1f)
				{
					randVect.X = 1f;
				}
				if (randVect.X < -1f)
				{
					randVect.X = -1f;
				}
				if (!noYChange)
				{
					randVect.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
					if (randVect.Y > 1f)
					{
						randVect.Y = 1f;
					}
					if (randVect.Y < -1f)
					{
						randVect.Y = -1f;
					}
				}
				else if (type != 59 && num < 3.0)
				{
					if (randVect.Y > 1f)
					{
						randVect.Y = 1f;
					}
					if (randVect.Y < -1f)
					{
						randVect.Y = -1f;
					}
				}
				if (type == 59 && !noYChange)
				{
					if ((double)randVect.Y > 0.5)
					{
						randVect.Y = 0.5f;
					}
					if ((double)randVect.Y < -0.5)
					{
						randVect.Y = -0.5f;
					}
					if ((double)pos.Y < Main.rockLayer + 100.0)
					{
						randVect.Y = 1f;
					}
					if (pos.Y > (float)(Main.maxTilesY - 300))
					{
						randVect.Y = -1f;
					}
				}
			}
		}
		public static void templeCleaner(int x, int y)
		{
			int num = 0;
			if (Main.tile[x + 1, y].active() && Main.tile[x + 1, y].type == 206)
			{
				num++;
			}
			if (Main.tile[x - 1, y].active() && Main.tile[x - 1, y].type == 206)
			{
				num++;
			}
			if (Main.tile[x, y + 1].active() && Main.tile[x, y + 1].type == 206)
			{
				num++;
			}
			if (Main.tile[x, y - 1].active() && Main.tile[x, y - 1].type == 206)
			{
				num++;
			}
			if (Main.tile[x, y].active() && Main.tile[x, y].type == 206)
			{
				if (num <= 1)
				{
					Main.tile[x, y].active(false);
					Main.tile[x, y].wall = 84;
					return;
				}
			}
			else if (!Main.tile[x, y].active() && num == 3)
			{
				Main.tile[x, y].active(true);
				Main.tile[x, y].type = 206;
				Main.tile[x, y].liquid = 0;
				Main.tile[x, y].slope(0);
				Main.tile[x, y].halfBrick(false);
			}
		}
		public static Vector2 templePather(Vector2 templePath, int destX, int destY)
		{
			int num = (int)templePath.X;
			int num2 = (int)templePath.Y;
			int num3 = Main.rand.Next(5, 20);
			int num4 = Main.rand.Next(2, 5);
			while (num3 > 0 && (num != destX || num2 != destY))
			{
				num3--;
				if (num > destX)
				{
					num--;
				}
				if (num < destX)
				{
					num++;
				}
				if (num2 > destY)
				{
					num2--;
				}
				if (num2 < destY)
				{
					num2++;
				}
				for (int i = num - num4; i < num + num4; i++)
				{
					for (int j = num2 - num4; j < num2 + num4; j++)
					{
						Main.tile[i, j].active(false);
						Main.tile[i, j].wall = 84;
					}
				}
			}
			return new Vector2((float)num, (float)num2);
		}


		public static void outerTempled(int x, int y)
		{
			if (Main.tile[x, y].active() & Main.tile[x, y].type == 206)
			{
				return;
			}
			if (Main.tile[x, y].wall == 84)
			{
				return;
			}
			int num = 6;
			for (int i = x - num; i <= x + num; i++)
			{
				for (int j = y - num; j <= y + num; j++)
				{
					if (!Main.tile[i, j].active() && Main.tile[i, j].wall == 84)
					{
						Main.tile[x, y].active(true);
						Main.tile[x, y].type = 206;
						Main.tile[x, y].liquid = 0;
						Main.tile[x, y].slope(0);
						Main.tile[x, y].halfBrick(false);
						return;
					}
				}
			}
		}

		public static void makeTemple(int x, int y)
		{
			Rectangle[] array = new Rectangle[40];
			float num = (float)(Main.maxTilesX / 4200);
			int num2 = Main.rand.Next((int)(num * 10f), (int)(num * 16f));
			int num3 = 1;
			if (Main.rand.Next(2) == 0)
			{
				num3 = -1;
			}
			int num4 = num3;
			int num5 = x;
			int num6 = y;
			int num7 = x;
			int num8 = y;
			int num9 = Main.rand.Next(1, 3);
			int num10 = 0;
			for (int i = 0; i < num2; i++)
			{
				num10++;
				int num11 = num3;
				int num12 = num7;
				int num13 = num8;
				bool flag = true;
				int num14 = 0;
				int num15 = 0;
				int num16 = -10;
				Rectangle rectangle = new Rectangle(num12 - num14 / 2, num13 - num15 / 2, num14, num15);
				while (flag)
				{
					num12 = num7;
					num13 = num8;
					num14 = Main.rand.Next(25, 50);
					num15 = Main.rand.Next(20, 35);
					if (num15 > num14)
					{
						num15 = num14;
					}
					if (i == num2 - 1)
					{
						num14 = Main.rand.Next(55, 65);
						num15 = Main.rand.Next(45, 50);
						if (num15 > num14)
						{
							num15 = num14;
						}
						num14 = (int)((double)((float)num14) * 1.6);
						num15 = (int)((double)((float)num15) * 1.35);
						num13 += Main.rand.Next(5, 10);
					}
					if (num10 > num9)
					{
						num13 += Main.rand.Next(num15 + 1, num15 + 3) + num16;
						num12 += Main.rand.Next(-5, 6);
						num11 = num3 * -1;
					}
					else
					{
						num12 += (Main.rand.Next(num14 + 1, num14 + 3) + num16) * num11;
						num13 += Main.rand.Next(-5, 6);
					}
					flag = false;
					rectangle = new Rectangle(num12 - num14 / 2, num13 - num15 / 2, num14, num15);
					for (int j = 0; j < i; j++)
					{
						if (rectangle.Intersects(array[j]))
						{
							flag = true;
						}
						if (Main.rand.Next(100) == 0)
						{
							num16++;
						}
					}
				}
				if (num10 > num9)
				{
					num9++;
					num10 = 1;
				}
				array[i] = rectangle;
				num3 = num11;
				num7 = num12;
				num8 = num13;
			}
			for (int k = 0; k < num2; k++)
			{
				for (int l = 0; l < 2; l++)
				{
					for (int m = 0; m < num2; m++)
					{
						for (int n = 0; n < 2; n++)
						{
							int num17 = array[k].X;
							if (l == 1)
							{
								num17 += array[k].Width - 1;
							}
							int num18 = array[k].Y;
							int num19 = num18 + array[k].Height;
							int num20 = array[m].X;
							if (n == 1)
							{
								num20 += array[m].Width - 1;
							}
							int y2 = array[m].Y;
							int num21 = y2 + array[m].Height;
							while (num17 != num20 || num18 != y2 || num19 != num21)
							{
								if (num17 < num20)
								{
									num17++;
								}
								if (num17 > num20)
								{
									num17--;
								}
								if (num18 < y2)
								{
									num18++;
								}
								if (num18 > y2)
								{
									num18--;
								}
								if (num19 < num21)
								{
									num19++;
								}
								if (num19 > num21)
								{
									num19--;
								}
								int num22 = num17;
								for (int num23 = num18; num23 < num19; num23++)
								{
									Main.tile[num22, num23].active(true);
									Main.tile[num22, num23].type = 206;
									Main.tile[num22, num23].liquid = 0;
									Main.tile[num22, num23].slope(0);
									Main.tile[num22, num23].halfBrick(false);
								}
							}
						}
					}
				}
			}
			for (int num24 = 0; num24 < num2; num24++)
			{
				if (Main.rand.Next(1) == 0)
				{
					for (int num25 = array[num24].X; num25 < array[num24].X + array[num24].Width; num25++)
					{
						for (int num26 = array[num24].Y; num26 < array[num24].Y + array[num24].Height; num26++)
						{
							Main.tile[num25, num26].active(true);
							Main.tile[num25, num26].type = 206;
							Main.tile[num25, num26].liquid = 0;
							Main.tile[num25, num26].slope(0);
							Main.tile[num25, num26].halfBrick(false);
						}
					}
					int num27 = array[num24].X;
					int num28 = num27 + array[num24].Width;
					int num29 = array[num24].Y;
					int num30 = num29 + array[num24].Height;
					num27 += Main.rand.Next(3, 8);
					num28 -= Main.rand.Next(3, 8);
					num29 += Main.rand.Next(3, 8);
					num30 -= Main.rand.Next(3, 8);
					int num31 = num27;
					int num32 = num28;
					int num33 = num29;
					int num34 = num30;
					int num35 = (num27 + num28) / 2;
					int num36 = (num29 + num30) / 2;
					for (int num37 = num27; num37 < num28; num37++)
					{
						for (int num38 = num29; num38 < num30; num38++)
						{
							if (Main.rand.Next(20) == 0)
							{
								num33 += Main.rand.Next(-1, 2);
							}
							if (Main.rand.Next(20) == 0)
							{
								num34 += Main.rand.Next(-1, 2);
							}
							if (Main.rand.Next(20) == 0)
							{
								num31 += Main.rand.Next(-1, 2);
							}
							if (Main.rand.Next(20) == 0)
							{
								num32 += Main.rand.Next(-1, 2);
							}
							if (num31 < num27)
							{
								num31 = num27;
							}
							if (num32 > num28)
							{
								num32 = num28;
							}
							if (num33 < num29)
							{
								num33 = num29;
							}
							if (num34 > num30)
							{
								num34 = num30;
							}
							if (num31 > num35)
							{
								num31 = num35;
							}
							if (num32 < num35)
							{
								num32 = num35;
							}
							if (num33 > num36)
							{
								num33 = num36;
							}
							if (num34 < num36)
							{
								num34 = num36;
							}
							if (num37 >= num31 && (num37 < num32 & num38 >= num33) && num38 <= num34)
							{
								Main.tile[num37, num38].active(false);
								Main.tile[num37, num38].wall = 84;
							}
						}
					}
					for (int num39 = num30; num39 > num29; num39--)
					{
						for (int num40 = num28; num40 > num27; num40--)
						{
							if (Main.rand.Next(20) == 0)
							{
								num33 += Main.rand.Next(-1, 2);
							}
							if (Main.rand.Next(20) == 0)
							{
								num34 += Main.rand.Next(-1, 2);
							}
							if (Main.rand.Next(20) == 0)
							{
								num31 += Main.rand.Next(-1, 2);
							}
							if (Main.rand.Next(20) == 0)
							{
								num32 += Main.rand.Next(-1, 2);
							}
							if (num31 < num27)
							{
								num31 = num27;
							}
							if (num32 > num28)
							{
								num32 = num28;
							}
							if (num33 < num29)
							{
								num33 = num29;
							}
							if (num34 > num30)
							{
								num34 = num30;
							}
							if (num31 > num35)
							{
								num31 = num35;
							}
							if (num32 < num35)
							{
								num32 = num35;
							}
							if (num33 > num36)
							{
								num33 = num36;
							}
							if (num34 < num36)
							{
								num34 = num36;
							}
							if (num40 >= num31 && (num40 < num32 & num39 >= num33) && num39 <= num34)
							{
								Main.tile[num40, num39].active(false);
								Main.tile[num40, num39].wall = 84;
							}
						}
					}
				}
			}
			Vector2 vector = new Vector2((float)num5, (float)num6);
			for (int num41 = 0; num41 < num2; num41++)
			{
				Rectangle rectangle2 = array[num41];
				rectangle2.X += 8;
				rectangle2.Y += 8;
				rectangle2.Width -= 16;
				rectangle2.Height -= 16;
				bool flag2 = true;
				while (flag2)
				{
					int num42 = Main.rand.Next(rectangle2.X, rectangle2.X + rectangle2.Width);
					int num43 = Main.rand.Next(rectangle2.Y, rectangle2.Y + rectangle2.Height);
					vector = WorldMethods.templePather(vector, num42, num43);
					if (vector.X == (float)num42 && vector.Y == (float)num43)
					{
						flag2 = false;
					}
				}
				if (num41 < num2 - 1)
				{
					if (Main.rand.Next(3) != 0)
					{
						int num44 = num41 + 1;
						if (array[num44].Y >= array[num41].Y + array[num41].Height)
						{
							rectangle2.X = array[num44].X;
							if (array[num44].X < array[num41].X)
							{
								rectangle2.X += (int)((double)((float)array[num44].Width) * 0.2);
							}
							else
							{
								rectangle2.X += (int)((double)((float)array[num44].Width) * 0.8);
							}
							rectangle2.Y = array[num44].Y;
						}
						else
						{
							rectangle2.X = (array[num41].X + array[num41].Width / 2 + (array[num44].X + array[num44].Width / 2)) / 2;
							rectangle2.Y = (int)((double)array[num44].Y + (double)array[num44].Height * 0.8);
						}
						int x2 = rectangle2.X;
						int y3 = rectangle2.Y;
						flag2 = true;
						while (flag2)
						{
							int num45 = Main.rand.Next(x2 - 6, x2 + 7);
							int num46 = Main.rand.Next(y3 - 6, y3 + 7);
							vector = WorldMethods.templePather(vector, num45, num46);
							if (vector.X == (float)num45 && vector.Y == (float)num46)
							{
								flag2 = false;
							}
						}
					}
					else
					{
						int num47 = num41 + 1;
						int num48 = (array[num41].X + array[num41].Width / 2 + (array[num47].X + array[num47].Width / 2)) / 2;
						int num49 = (array[num41].Y + array[num41].Height / 2 + (array[num47].Y + array[num47].Height / 2)) / 2;
						flag2 = true;
						while (flag2)
						{
							int num50 = Main.rand.Next(num48 - 6, num48 + 7);
							int num51 = Main.rand.Next(num49 - 6, num49 + 7);
							vector = WorldMethods.templePather(vector, num50, num51);
							if (vector.X == (float)num50 && vector.Y == (float)num51)
							{
								flag2 = false;
							}
						}
					}
				}
			}
			int num52 = Main.maxTilesX - 20;
			int num53 = 20;
			int num54 = Main.maxTilesY - 20;
			int num55 = 20;
			for (int num56 = 0; num56 < num2; num56++)
			{
				if (array[num56].X < num52)
				{
					num52 = array[num56].X;
				}
				if (array[num56].X + array[num56].Width > num53)
				{
					num53 = array[num56].X + array[num56].Width;
				}
				if (array[num56].Y < num54)
				{
					num54 = array[num56].Y;
				}
				if (array[num56].Y + array[num56].Height > num55)
				{
					num55 = array[num56].Y + array[num56].Height;
				}
			}
			num52 -= 10;
			num53 += 10;
			num54 -= 10;
			num55 += 10;
			for (int num57 = num52; num57 < num53; num57++)
			{
				for (int num58 = num54; num58 < num55; num58++)
				{
					WorldMethods.outerTempled(num57, num58);
				}
			}
			for (int num59 = num53; num59 >= num52; num59--)
			{
				for (int num60 = num54; num60 < num55 / 2; num60++)
				{
					WorldMethods.outerTempled(num59, num60);
				}
			}
			for (int num61 = num54; num61 < num55; num61++)
			{
				for (int num62 = num52; num62 < num53; num62++)
				{
					WorldMethods.outerTempled(num62, num61);
				}
			}
			for (int num63 = num55; num63 >= num54; num63--)
			{
				for (int num64 = num52; num64 < num53; num64++)
				{
					WorldMethods.outerTempled(num64, num63);
				}
			}
			num3 = -num4;
			Vector2 vector2 = new Vector2((float)num5, (float)num6);
			int num65 = Main.rand.Next(2, 5);
			bool flag3 = true;
			int num66 = 0;
			int num67 = Main.rand.Next(9, 14);
			while (flag3)
			{
				num66++;
				if (num66 >= num67)
				{
					num66 = 0;
					vector2.Y -= 1f;
				}
				vector2.X += (float)num3;
				int num68 = (int)vector2.X;
				flag3 = false;
				int num69 = (int)vector2.Y - num65;
				while ((float)num69 < vector2.Y + (float)num65)
				{
					if (Main.tile[num68, num69].wall == 84 || (Main.tile[num68, num69].active() && Main.tile[num68, num69].type == 206))
					{
						flag3 = true;
					}
					if (Main.tile[num68, num69].active() && Main.tile[num68, num69].type == 206)
					{
						Main.tile[num68, num69].active(false);
						Main.tile[num68, num69].wall = 84;
					}
					num69++;
				}
			}
			int num70 = num5;
			int num71 = num6;
			while (!Main.tile[num70, num71].active())
			{
				num71++;
			}
			num71 -= 4;
			int num72 = num71;
			while ((Main.tile[num70, num72].active() && Main.tile[num70, num72].type == 206) || Main.tile[num70, num72].wall == 84)
			{
				num72--;
			}
			num72 += 2;
			for (int num73 = num70 - 1; num73 <= num70 + 1; num73++)
			{
				for (int num74 = num72; num74 <= num71; num74++)
				{
					Main.tile[num73, num74].active(true);
					Main.tile[num73, num74].type = 206;
					Main.tile[num73, num74].liquid = 0;
					Main.tile[num73, num74].slope(0);
					Main.tile[num73, num74].halfBrick(false);
				}
			}
			for (int num75 = num70 - 4; num75 <= num70 + 4; num75++)
			{
				for (int num76 = num71 - 1; num76 < num71 + 3; num76++)
				{
					Main.tile[num75, num76].active(false);
					Main.tile[num75, num76].wall = 84;
				}
			}
			for (int num77 = num70 - 1; num77 <= num70 + 1; num77++)
			{
				for (int num78 = num71 - 5; num78 <= num71 + 8; num78++)
				{
					Main.tile[num77, num78].active(true);
					Main.tile[num77, num78].type = 206;
					Main.tile[num77, num78].liquid = 0;
					Main.tile[num77, num78].slope(0);
					Main.tile[num77, num78].halfBrick(false);
				}
			}
			for (int num79 = num70 - 1; num79 <= num70 + 1; num79++)
			{
				for (int num80 = num71; num80 < num71 + 3; num80++)
				{
					Main.tile[num79, num80].active(false);
					Main.tile[num79, num80].wall = 84;
				}
			}
			WorldGen.PlaceTile(num70, num71, 10, true, false, -1, 11);
			for (int num81 = num52; num81 < num53; num81++)
			{
				for (int num82 = num54; num82 < num55; num82++)
				{
					WorldMethods.templeCleaner(num81, num82);
				}
			}
			for (int num83 = num55; num83 >= num54; num83--)
			{
				for (int num84 = num53; num84 >= num52; num84--)
				{
					WorldMethods.templeCleaner(num84, num83);
				}
			}
			for (int num85 = num52; num85 < num53; num85++)
			{
				for (int num86 = num54; num86 < num55; num86++)
				{
					bool flag4 = true;
					for (int num84 = num85 - 1; num84 <= num85 + 1; num84++)
					{
						for (int num88 = num86 - 1; num88 <= num86 + 1; num88++)
						{
							if ((!Main.tile[num84, num88].active() || Main.tile[num84, num88].type != 206) && Main.tile[num84, num88].wall != 84)
							{
								flag4 = false;
								break;
							}
						}
					}
					if (flag4)
					{
						Main.tile[num85, num86].wall = 84;
					}
				}
			}
			int num89 = 0;
			Rectangle rectangle3;
			int num90;
			int num91;
			do
			{
				num89++;
				rectangle3 = array[num2 - 1];
				num90 = rectangle3.X + Main.rand.Next(rectangle3.Width);
				num91 = rectangle3.Y + Main.rand.Next(rectangle3.Height);
				WorldGen.PlaceTile(num90, num91, 237, false, false, -1, 0);
				if (Main.tile[num90, num91].type == 237)
				{
					goto IL_12B3;
				}
			}
			while (num89 < 1000);
			num90 = rectangle3.X + rectangle3.Width / 2;
			num91 = rectangle3.Y + rectangle3.Height / 2;
			num90 += Main.rand.Next(-10, 11);
			num91 += Main.rand.Next(-10, 11);
			while (!Main.tile[num90, num91].active())
			{
				num91++;
			}
			Main.tile[num90 - 1, num91].active(true);
			Main.tile[num90 - 1, num91].slope(0);
			Main.tile[num90 - 1, num91].halfBrick(false);
			Main.tile[num90 - 1, num91].type = 206;
			Main.tile[num90, num91].active(true);
			Main.tile[num90, num91].slope(0);
			Main.tile[num90, num91].halfBrick(false);
			Main.tile[num90, num91].type = 206;
			Main.tile[num90 + 1, num91].active(true);
			Main.tile[num90 + 1, num91].slope(0);
			Main.tile[num90 + 1, num91].halfBrick(false);
			Main.tile[num90 + 1, num91].type = 206;
			num91 -= 2;
			num90--;
			for (int num92 = -1; num92 <= 3; num92++)
			{
				for (int num93 = -1; num93 <= 1; num93++)
				{
					x = num90 + num92;
					y = num91 + num93;
					Main.tile[x, y].active(false);
				}
			}
			for (int num94 = 0; num94 <= 2; num94++)
			{
				for (int num95 = 0; num95 <= 1; num95++)
				{
					x = num90 + num94;
					y = num91 + num95;
					Main.tile[x, y].active(true);
					Main.tile[x, y].type = 237;
					Main.tile[x, y].frameX = (short)(num94 * 18);
					Main.tile[x, y].frameY = (short)(num95 * 18);
				}
			}
			goto IL_1547;
IL_12B3:
			int lAltarX = num90 - (int)(Main.tile[num90, num91].frameX / 18);
			int lAltarY = num91 - (int)(Main.tile[num90, num91].frameY / 18);
IL_1547:
			float num96 = (float)num2 * 1.1f;
			num96 *= 1f + (float)Main.rand.Next(-25, 26) * 0.01f;
			int num97 = 0;
			while (num96 > 0f)
			{
				num97++;
				int num98 = Main.rand.Next(num2);
				int num99 = Main.rand.Next(array[num98].X, array[num98].X + array[num98].Width);
				int num100 = Main.rand.Next(array[num98].Y, array[num98].Y + array[num98].Height);
				if (Main.tile[num99, num100].wall == 84 && !Main.tile[num99, num100].active())
				{
					bool flag5 = false;
					if (Main.rand.Next(2) == 0)
					{
						int num101 = 1;
						if (Main.rand.Next(2) == 0)
						{
							num101 = -1;
						}
						while (!Main.tile[num99, num100].active())
						{
							num100 += num101;
						}
						num100 -= num101;
						int num102 = Main.rand.Next(2);
						int num103 = Main.rand.Next(3, 10);
						bool flag6 = true;
						for (int num104 = num99 - num103; num104 < num99 + num103; num104++)
						{
							for (int num105 = num100 - num103; num105 < num100 + num103; num105++)
							{
								if (Main.tile[num104, num105].active() && Main.tile[num104, num105].type == 10)
								{
									flag6 = false;
									break;
								}
							}
						}
						if (flag6)
						{
							for (int num106 = num99 - num103; num106 < num99 + num103; num106++)
							{
								for (int num107 = num100 - num103; num107 < num100 + num103; num107++)
								{
									if (WorldGen.SolidTile(num106, num107) && Main.tile[num106, num107].type != 232 && !WorldGen.SolidTile(num106, num107 - num101))
									{
										Main.tile[num106, num107].type = 232;
										flag5 = true;
										if (num102 == 0)
										{
											Main.tile[num106, num107 - 1].type = 232;
											Main.tile[num106, num107 - 1].active(true);
										}
										else
										{
											Main.tile[num106, num107 + 1].type = 232;
											Main.tile[num106, num107 + 1].active(true);
										}
										num102++;
										if (num102 > 1)
										{
											num102 = 0;
										}
									}
								}
							}
						}
						if (flag5)
						{
							num97 = 0;
							num96 -= 1f;
						}
					}
					else
					{
						int num108 = 1;
						if (Main.rand.Next(2) == 0)
						{
							num108 = -1;
						}
						while (!Main.tile[num99, num100].active())
						{
							num99 += num108;
						}
						num99 -= num108;
						int num109 = Main.rand.Next(2);
						int num110 = Main.rand.Next(3, 10);
						bool flag7 = true;
						for (int num111 = num99 - num110; num111 < num99 + num110; num111++)
						{
							for (int num112 = num100 - num110; num112 < num100 + num110; num112++)
							{
								if (Main.tile[num111, num112].active() && Main.tile[num111, num112].type == 10)
								{
									flag7 = false;
									break;
								}
							}
						}
						if (flag7)
						{
							for (int num113 = num99 - num110; num113 < num99 + num110; num113++)
							{
								for (int num114 = num100 - num110; num114 < num100 + num110; num114++)
								{
									if (WorldGen.SolidTile(num113, num114) && Main.tile[num113, num114].type != 232 && !WorldGen.SolidTile(num113 - num108, num114))
									{
										Main.tile[num113, num114].type = 232;
										flag5 = true;
										if (num109 == 0)
										{
											Main.tile[num113 - 1, num114].type = 232;
											Main.tile[num113 - 1, num114].active(true);
										}
										else
										{
											Main.tile[num113 + 1, num114].type = 232;
											Main.tile[num113 + 1, num114].active(true);
										}
										num109++;
										if (num109 > 1)
										{
											num109 = 0;
										}
									}
								}
							}
						}
						if (flag5)
						{
							num97 = 0;
							num96 -= 1f;
						}
					}
				}
				if (num97 > 1000)
				{
					num97 = 0;
					num96 -= 1f;
				}
			}
			WorldGen.tLeft = num52;
			WorldGen.tRight = num53;
			WorldGen.tTop = num54;
			WorldGen.tBottom = num55;
			WorldGen.tRooms = num2;
		}

		public static void templePart2()
		{
			int minValue = WorldGen.tLeft;
			int maxValue = WorldGen.tRight;
			int minValue2 = WorldGen.tTop;
			int num = WorldGen.tBottom;
			int num2 = WorldGen.tRooms;
			float num3 = (float)num2 * 1.9f;
			num3 *= 1f + (float)Main.rand.Next(-15, 16) * 0.01f;
			int num4 = 0;
			while (num3 > 0f)
			{
				int num5 = Main.rand.Next(minValue, maxValue);
				int num6 = Main.rand.Next(minValue2, num);
				if (Main.tile[num5, num6].wall == 84 && !Main.tile[num5, num6].active())
				{
					if (WorldGen.mayanTrap(num5, num6))
					{
						num3 -= 1f;
						num4 = 0;
					}
					else
					{
						num4++;
					}
				}
				else
				{
					num4++;
				}
				if (num4 > 100)
				{
					num4 = 0;
					num3 -= 1f;
				}
			}
			Main.tileSolid[232] = false;
			float num7 = (float)num2 * 0.35f;
			num7 *= 1f + (float)Main.rand.Next(-15, 16) * 0.01f;
			int contain = 1293;
			num4 = 0;
			while (num7 > 0f)
			{
				int num8 = Main.rand.Next(minValue, maxValue);
				int num9 = Main.rand.Next(minValue2, num);
				if (Main.tile[num8, num9].wall == 84 && !Main.tile[num8, num9].active() && WorldGen.AddBuriedChest(num8, num9, contain, true, 16))
				{
					num7 -= 1f;
					num4 = 0;
				}
				num4++;
				if (num4 > 10000)
				{
					break;
				}
			}
			float num10 = (float)num2 * 1.25f;
			num10 *= 1f + (float)Main.rand.Next(-25, 36) * 0.01f;
			num4 = 0;
			while (num10 > 0f)
			{
				num4++;
				int num11 = Main.rand.Next(minValue, maxValue);
				int num12 = Main.rand.Next(minValue2, num);
				if (Main.tile[num11, num12].wall == 84 && !Main.tile[num11, num12].active())
				{
					int num13 = num11;
					int num14 = num12;
					while (!Main.tile[num13, num14].active())
					{
						num14++;
						if (num14 > num)
						{
							break;
						}
					}
					num14--;
					if (num14 <= num)
					{
						WorldGen.PlaceTile(num13, num14, 93, true, false, -1, 5);
						if (Main.tile[num13, num14].type == 93)
						{
							num10 -= 1f;
						}
					}
				}
			}
			float num15 = (float)num2 * 1.35f;
			num15 *= 1f + (float)Main.rand.Next(-15, 26) * 0.01f;
			num4 = 0;
			while (num15 > 0f)
			{
				num4++;
				int num16 = Main.rand.Next(minValue, maxValue);
				int num17 = Main.rand.Next(minValue2, num);
				if (Main.tile[num16, num17].wall == 84 && !Main.tile[num16, num17].active())
				{
					int num18 = num16;
					int num19 = num17;
					while (!Main.tile[num18, num19].active())
					{
						num19++;
						if (num19 > num)
						{
							break;
						}
					}
					num19--;
					if (num19 <= num)
					{
						int num20 = Main.rand.Next(3);
						if (num20 == 0)
						{
							WorldGen.PlaceTile(num18, num19, 18, true, false, -1, 10);
							if (Main.tile[num18, num19].type == 18)
							{
								num15 -= 1f;
							}
						}
						else if (num20 == 1)
						{
							WorldGen.PlaceTile(num18, num19, 14, true, false, -1, 9);
							if (Main.tile[num18, num19].type == 14)
							{
								num15 -= 1f;
							}
						}
						else if (num20 == 2)
						{
							WorldGen.PlaceTile(num18, num19, 15, true, false, -1, 12);
							if (Main.tile[num18, num19].type == 15)
							{
								num15 -= 1f;
							}
						}
					}
				}
				if (num4 > 10000)
				{
					break;
				}
			}
			Main.tileSolid[232] = true;
		}
	}
}