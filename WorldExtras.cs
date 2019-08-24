using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Achievements;

namespace SpiritMod
{
	public static class WorldExtras
	{
		public static void Explode(float posX, float posY, float radius = 3, bool wallDamage = true)
		{
			int xBegin = (int)(posX / 16f - radius);
			int xEnd = (int)(posX / 16f + radius);
			int yBegin = (int)(posY / 16f - radius);
			int yEnd = (int)(posY / 16f + radius);
			if (xBegin < 0)
			{
				xBegin = 0;
			}
			if (xEnd > Main.maxTilesX)
			{
				xEnd = Main.maxTilesX;
			}
			if (yBegin < 0)
			{
				yBegin = 0;
			}
			if (yEnd > Main.maxTilesY)
			{
				yEnd = Main.maxTilesY;
			}
			bool breakWall = false;
			if (wallDamage)
			{
				for (int x = xBegin; x <= xEnd; x++)
				{
					for (int y = yBegin; y <= yEnd; y++)
					{
						float deltaX = Math.Abs((float)x - posX / 16f);
						float deltaY = Math.Abs((float)y - posY / 16f);
						double dist = Math.Sqrt((double)(deltaX * deltaX + deltaY * deltaY));
						if (dist < (double)radius && Main.tile[x, y] != null && Main.tile[x, y].wall == 0)
						{
							breakWall = true;
							break;
						}
					}
				}
			}
			AchievementsHelper.CurrentlyMining = true;
			for (int x = xBegin; x <= xEnd; x++)
			{
				for (int y = yBegin; y <= yEnd; y++)
				{
					float deltaX = Math.Abs((float)x - posX / 16f);
					float deltaY = Math.Abs((float)y - posY / 16f);
					double dist = Math.Sqrt((double)(deltaX * deltaX + deltaY * deltaY));
					if (dist < (double)radius)
					{
						bool destroyTile = true;
						if (Main.tile[x, y] != null && Main.tile[x, y].active())
						{
							destroyTile = true;
							ushort tile = Main.tile[x, y].type;
							if (Main.tileDungeon[(int)tile] || tile == 88 || tile == 21 || tile == 26 || tile == 107 || tile == 108 || tile == 111 || tile == 226 || tile == 237 || tile == 221 || tile == 222 || tile == 223 || tile == 211 || tile == 404)
							{
								destroyTile = false;
							}
							//patch file: x, y
							if (!Main.hardMode && tile == 58)
							{
								destroyTile = false;
								//patch file: x, y
							}
							if (!TileLoader.CanExplode(x, y))
							{
								destroyTile = false;
							}
							if (destroyTile)
							{
								WorldGen.KillTile(x, y, false, false, false);
								if (!Main.tile[x, y].active() && Main.netMode != 0)
								{
									NetMessage.SendData(17, -1, -1, null, 0, (float)x, (float)y, 0f, 0, 0, 0);
								}
							}
						}
						if (destroyTile && breakWall)
						{
							for (int wallX = x - 1; wallX <= x + 1; wallX++)
							{
								for (int wallY = y - 1; wallY <= y + 1; wallY++)
								{
									if (Main.tile[wallX, wallY] != null && Main.tile[wallX, wallY].wall > 0)
									{
										WorldGen.KillWall(wallX, wallY, false);
										if (Main.tile[wallX, wallY].wall == 0 && Main.netMode != 0)
										{
											NetMessage.SendData(17, -1, -1, null, 2, (float)wallX, (float)wallY, 0f, 0, 0, 0);
										}
									}
								}
							}
						}
					}
				}
			}
			AchievementsHelper.CurrentlyMining = false;
		}

		public static float? CollisionDistance(Vector2 start, float width, float height, Vector2 vel, bool ignoreTopCollision = true, bool evenActuated = false)
		{
			float xDiv = 16f / vel.X;
			float yDiv = 16f / vel.Y;
			if (float.IsInfinity(xDiv) && float.IsInfinity(yDiv))
			{
				return null;
			}
			bool movX = vel.X >= 0f;
			bool movY = vel.Y >= 0f;

			Vector2 pos = new Vector2(start.X * (1f/16f), start.Y * (1f/16f));
			Main.NewText("pos:  ( " + (int)pos.X + " | " + (int)pos.Y + ")");
			width *= 1f / 16f;
			if (movX)
			{
				pos.X += width;
				width *= -1f;
			}
			height = 1f / 16f;
			if (movY)
			{
				pos.Y += height;
				height *= -1f;
			}
			Vector2 end = pos + vel * (1f/16f);
			Main.NewText("end:  ( " + (int)end.X + " | " + (int)end.Y + ")");
			int xStart = Math.Max((int)pos.X, 0);
			int yStart = Math.Max((int)pos.Y, 0);
			int xEnd = Math.Min((int)end.X, Main.maxTilesX);
			int yEnd = Math.Min((int)end.Y, Main.maxTilesY);
			int cascades = Math.Abs(xEnd - xStart) + Math.Abs(yEnd - yStart);

			float xVel = vel.X * (1f / 16f);
			float yVel = vel.Y * (1f / 16f);
			float xNext = movX ? (float)Math.Ceiling(pos.X) : (float)Math.Floor(pos.X);
			float yNext = movY ? (float)Math.Ceiling(pos.Y) : (float)Math.Floor(pos.Y);
			float xRate = (xNext - pos.X) * xDiv;
			float yRate = (yNext - pos.Y) * yDiv;
			for (int i = cascades; i > 0; i--)
			{
				if (yRate > xRate)
				{ //X is next
					int x = (int)(movX ? xNext : xNext -1);
					//Skip, if x is out of bounds.
					if (InvalidTileX(x))
					{
						xNext += movX ? 1f : -1f;
						xRate = (xNext - pos.X) * xDiv;
						continue;
					}
					float scanStart = (xNext - start.X) * xDiv * yVel + pos.Y;
					int y = (int)scanStart;
					int target = (int)(scanStart + height);

					if (movY)
					{
						for (; y >= target; y--)
						{
							if (InvalidTileY(y))
							{
								continue;
							}
							Tile tile = Main.tile[x, y];
							if (tile != null && tile.active() && (evenActuated || !tile.inActive()) && Main.tileSolid[tile.type])
							{
								return (xNext - start.X) * xDiv;
							}
						}
					}
					else
					{
						for (; y <= target; y++)
						{
							if (InvalidTileY(y))
							{
								continue;
							}
							Tile tile = Main.tile[x, y];
							if (tile != null && tile.active() && (evenActuated || !tile.inActive()) && Main.tileSolid[tile.type])
							{
								return (xNext - start.X) * xDiv;
							}
						}
					}

					xNext += movX ? 1f : -1f;
					xRate = (xNext - pos.X) * xDiv;
				}
				else
				{ //Y is next
					int y = (int)(movY ? yNext : yNext - 1);
					//Skip, if y is out of bounds.
					if (InvalidTileY(y))
					{
						yNext += movY ? 1f : -1f;
						yRate = (yNext - pos.Y) * yDiv;
						continue;
					}
					float scanStart = (yNext - start.Y) * yDiv * xVel + pos.X;
					int x = (int)scanStart;
					int target = (int)(scanStart + width);

					if (movX)
					{
						for (; x >= target; x--)
						{
							if (InvalidTileX(x))
							{
								continue;
							}
							Tile tile = Main.tile[x, y];
							if (tile != null && tile.active() && (evenActuated || !tile.inActive()) && (Main.tileSolid[tile.type] || movY && Main.tileSolidTop[tile.type]))
							{
								return (yNext - start.Y) * yDiv;
							}
						}
					}
					else
					{
						for (; x <= target; x++)
						{
							if (InvalidTileX(x))
							{
								continue;
							}
							Tile tile = Main.tile[x, y];
							if (tile != null && tile.active() && (evenActuated || !tile.inActive()) && (Main.tileSolid[tile.type] || movY && Main.tileSolidTop[tile.type]))
							{
								return (yNext - start.Y) * yDiv;
							}
						}
					}

					yNext += movY ? 1f : -1f;
					yRate = (yNext - pos.Y) * yDiv;
				}
			}

			return null;
		}

		public static bool ValidTile(float x, float y)
		{
			return x >= 0f && x < Main.maxTilesX * 16f + 16f && y >= 0f && y < Main.maxTilesY * 16f + 16f;
		}

		public static bool ValidTile(int x, int y)
		{
			return x >= 0 && x <= Main.maxTilesX && y >= 0 && y <= Main.maxTilesY;
		}

		public static bool ValidTileX(int x)
		{
			return x >= 0 && x <= Main.maxTilesX;
		}

		public static bool ValidTileY(int y)
		{
			return y >= 0 && y <= Main.maxTilesY;
		}

		public static bool InvalidTile(int x, int y)
		{
			return x < 0 || x > Main.maxTilesX || y < 0 || y > Main.maxTilesY;
		}

		public static bool InvalidTileX(int x)
		{
			return x < 0 || x > Main.maxTilesX;
		}

		public static bool InvalidTileY(int y)
		{
			return y < 0 || y > Main.maxTilesY;
		}

	}
}
