using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace SpiritMod.World
{
	public class WorldMethods
	{
		public static void RoundHill(int X, int Y, int Xmult, int Ymult, int strength, bool initialplace, ushort type)
		{
			if (initialplace)
				TileRunner(X, Y, (double)strength * 5, 1, type, true, 0f, 0f, true, true);
			for (int rotation2 = 0; rotation2 < 350; rotation2++)
			{
				int DistX = (int)(0 - (Math.Sin(rotation2) * Xmult));
				int DistY = (int)(0 - (Math.Cos(rotation2) * Ymult));
				TileRunner(X + DistX, Y + DistY, strength, 1, type, true, 0f, 0f, true, true);
			}
		}

		public static void RoundHill2(int X, int Y, int Xmult, int Ymult, int strength, bool initialplace, ushort type)
		{
			if (initialplace)
			{
				TileRunner(X, Y, (double)strength * 5, 1, type, false, 0f, 0f, true, true);
			}
			for (int rotation2 = 0; rotation2 < 350; rotation2++)
			{
				int DistX = (int)(0 - (Math.Sin(rotation2) * Xmult));
				int DistY = (int)(0 - (Math.Cos(rotation2) * Ymult));
			TileRunner(X + DistX, Y + DistY, strength, 1, type, false, 0f, 0f, true, true);
			}
		}

		public static void CragSpike(int X, int Y, int length, int height, ushort type2, float slope, float sloperight)
		{
			float trueslope = 1 / slope;
			float truesloperight = 1 / sloperight;
			int Xstray = length / 2;
			for (int level = 0; level <= height; level++)
			{
				Main.tile[X, (int)(Y + level - (slope / 2))].HasTile = true;
				Main.tile[X, (int)(Y + level - (slope / 2))].TileType = type2;
				for (int I = X - (int)(length + (level * trueslope)); I < X + (int)(length + (level * truesloperight)); I++)
				{
					Main.tile[I, (Y + level)].HasTile = true;
					Main.tile[I, (Y + level)].TileType = type2;
				}
			}
		}

		public static void Island(int X, int Y, int Xsize, float slope, ushort tile)
		{
			for (int xAxis = X; xAxis < X + Xsize; xAxis++)
			{
				int Slope2 = (int)(Math.Abs(Main.rand.Next((Xsize / 2) - (Xsize / 12), (Xsize / 2) + (Xsize / 12)) - Math.Abs((xAxis - X) - Main.rand.Next((Xsize / 2) - (Xsize / 12), (Xsize / 2) + (Xsize / 12)))) * slope);
				for (int I = 0; I < Slope2; I++)
					TileRunner(xAxis, Y + I, Xsize / 5, 1, tile, false, 0f, 0f, true, true);
				TileRunner(xAxis, Y, Xsize / 5, 1, tile, false, 0f, 0f, true, true);
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
								if (type == -2 && Main.tile[k, l].HasTile && (l < WorldGen.waterLine || l > WorldGen.lavaLine))
								{
									Main.tile[k, l].LiquidAmount = 255;
									if (l > WorldGen.lavaLine)
									{
										Main.tile[k, l].LiquidType = LiquidID.Lava;
									}
								}
								Main.tile[k, l].HasTile = false;
							}
							else
							{
								if (overRide || !Main.tile[k, l].HasTile)
								{
									Tile tile = Main.tile[k, l];
									bool flag3 = Main.tileStone[type] && tile.TileType != 1;
									if (!TileID.Sets.CanBeClearedDuringGeneration[(int)tile.TileType])
									{
										flag3 = true;
									}
									ushort type2 = tile.TileType;
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
										tile.TileType = (ushort)type;
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
									Main.tile[k, l].HasTile = true;
									Main.tile[k, l].LiquidAmount = 0;
									Main.tile[k, l].lava/* tModPorter Suggestion: LiquidType = ... */(false);
								}
								if (type == 59 && l > WorldGen.waterLine && Main.tile[k, l].LiquidAmount > 0)
								{
									Main.tile[k, l].lava/* tModPorter Suggestion: LiquidType = ... */(false);
									Main.tile[k, l].LiquidAmount = 0;
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

		public static int AreaCount(int i, int j, int width, int height)
		{
			int count = 0; 

			for (int x = i; x < i + width; ++x)
			{
				for (int y = j; y < j + height; ++y)
				{
					Tile tile = Framing.GetTileSafely(x, y);
					if (tile.HasTile && Main.tileSolid[tile.TileType])
						count++;
				}
			}

			return count;
		}

		public static bool AreaClear(int i, int j, int width, int height) => AreaCount(i, j, width, height) == 0;
	}
}