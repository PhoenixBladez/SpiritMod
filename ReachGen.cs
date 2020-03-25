using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using SpiritMod;
using System.Reflection;
using Terraria.Utilities;
using System.Runtime.Serialization.Formatters.Binary;

namespace SpiritMod
{
	public class ReachGen : ModWorld
	{
        public static int ReachTiles = 0;

		private void PlaceShrine(int i, int j, int[,] ShrineArray) {
			
			for (int y = 0; y < ShrineArray.GetLength(0); y++) { // This loops clears the area (makes the proper hemicircle) and placed dirt in the bottom if there are no blocks (so that the chest and fireplace can be placed).
				for (int x = 0; x < ShrineArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (ShrineArray[y, x]) {
							case 0:
								break; // no changes
							case 1:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 2:
								WorldGen.PlaceTile(k, l, mod.TileType("ReachGrassTile")); // Dirt
								tile.active(true);
								break;
							case 3:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 4:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
						}
					}
				}
			}
			
			for (int y = 0; y < ShrineArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for (int x = 0; x < ShrineArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (ShrineArray[y, x]) {
							case 2:
								WorldGen.PlaceTile(k, l, mod.TileType("ReachGrassTile")); // Dirt
								tile.active(true);
								break;
							case 3:
								WorldGen.PlaceObject(k, l, 215); // Campfire
								break;
							case 4:
								WorldGen.PlaceChest(k, l, (ushort)mod.TileType("ReachChest"), false, 0); // Gold Chest
								break;
						}
					}
				}
			}
		}
		
		public void PlaceReachs(int x, int y)
		{
			Tile tile = Main.tile[1, 1];
			int[,] ShrineShape = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // 0 : no changes ; 1 : air ; 2 : dirt ; 3 : fireplace ; 4 : chest
				{0,0,0,0,1,1,1,1,1,1,0,0,0,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,0,0},
				{0,1,1,1,1,1,1,1,1,1,1,1,1,0},
				{0,1,1,1,1,1,1,1,1,1,1,1,1,0},
				{0,1,1,1,1,4,1,1,1,1,3,1,1,0},
				{0,2,2,2,2,2,2,2,2,2,2,2,2,0},
			};
			int startx = x; //keep track of starting position
			int starty = y;
			for (int z = 0; z < 147; z++)
			{
				if (Main.rand.Next(2) == 1)
				{
				WorldMethods.TileRunner(x, y + z, (double)Main.rand.Next(300), 1, mod.TileType("ReachGrassTile"), false, 0f, 0f, true, true); //Basic grass shape. Will be improved later. Specifically, make it only override certain tiles, and make it fill in random holes in the ground.
				}
			}
			
			
			
			//temporary wall placement until i get a better method
			for (int A = x - 150; A < x + 150; A++)
					{
						for (int B = y + 50; B < y + 1000; B++)
						{
							if (Main.tile[A,B] != null)
							{
                                int Wal = (int)Main.tile[A, B].wall;
                                if (Main.tile[A, B].type == mod.TileType("ReachGrassTile") && ((Wal == 2 || Wal == 54 || Wal == 55 || Wal == 56 || Wal == 57 || Wal == 58 || Wal == 59) || B > WorldGen.rockLayer - 100)) // A = x, B = y.
                                {
                                    if (WorldGen.genRand.Next(3) == 0)
                                    {
                                        WorldGen.KillWall(A, B);
                                        WorldGen.PlaceWall(A, B, mod.WallType("ReachWallNatural"));
                                    }
								}
							}
						}
					}
            int smoothness = 3; //how smooth the tunnels are
            int chestrarity = 7; //how rare chests are
			int depth = 9; //how many vertical tunnels deep the initial goes.
			int tunnelheight = Main.rand.Next(65, 80); //how high the first tunnel is 
			int tunnelthickness = 5;
			for (int q = 0; q < 6; q++) //make 2 tunnels to overlap
			{
				if (startx == x && starty == y)
				{
					tunnelheight = Main.rand.Next(45, 80);
					tunnelthickness = 4;
				}
				int jt = 0; //j placeholder to use out of loop
				int newx = startx;
				int newy = starty - 40;
				int tunnelX = Main.rand.Next(-55, 55); //how far away on the x axis each tunnel starts
				for (int p = 0; p < depth; p++)
				{
					int k = 0; //placeholder
					if (tunnelX > 0) //if tunnel leads right
					{
						for (k = newx; k < newx + tunnelX; k++)
						{
                            for (int e = 0; e < smoothness; e++)
                            {
                                WorldGen.digTunnel(k, newy, 0, 0, 1, tunnelthickness, false); //horizontal tunneling
                                if (Main.rand.Next(5) == 0)
                                {
                                    WorldGen.TileRunner(k, newy, 9, 1, 48, false, 0f, 0f, false, true);
                                }                                
                            }
                       
                            if (Main.rand.Next(75) == 1) //make a full branch
							{
								startx = k;
								starty = newy;
								depth = (depth - p) + 1;
							}
						}
                    }
					else if (tunnelX < 0) //if tunnel should lead left
					{
						for (k = newx; k > newx + tunnelX; k--)
						{
                            for (int e = 0; e < smoothness; e++)
                            {
                                WorldGen.digTunnel(k, newy, 0, 0, 1, tunnelthickness, false); //horizontal tunneling
                            }
                            if (Main.rand.Next(50) == 1) //make a full branch
							{
								startx = k;
								starty = newy;
								depth = (depth - p) + 1;
							}
						}
                    }
					 tunnelthickness = Main.rand.Next(3, 7);
					for (int j = 0; j < tunnelheight; j++) //go down the tunnel
					{
                        for (int e = 0; e < smoothness; e++)
                        {
                            WorldGen.digTunnel(newx + tunnelX, newy + j, 0, 0, 1, tunnelthickness, false); //vertical tunneling
                            if (Main.rand.Next(2) == 0)
                            {
                                WorldGen.TileRunner(newx + tunnelX, newy + j, 9, 1, 48, false, 0f, 0f, false, true);
                            }
                        }
						jt = j;
					}
					tunnelheight = Main.rand.Next(20,40); //how high the first tunnel is 
					tunnelthickness = Main.rand.Next(3, 7);
					newx = newx + tunnelX; //setting values to new places
					newy = newy + jt;
					if (Main.rand.Next(2) == 1)
					{
					tunnelX = Main.rand.Next(15, 50); //how far the horizontal tunnel goes
					}
					else
					{
					tunnelX = Main.rand.Next(-50, -15); //how far the horizontal tunnel goes
					}
					
					if (newx > x + 70) //if it strays too far on the x axis
					{
						tunnelX = Main.rand.Next(-75, -55);
					}
					if (newx < x - 70) 
					{
						tunnelX = Main.rand.Next(55, 75);
					}
				}
			}
		}	
		static bool CanPlaceReachs(int x, int y)
		{
			x = WorldGen.genRand.Next(50, Main.maxTilesX / 6);
			for (int i = x - 50; i < x + 50; i++)
			{
				for (int j = y - 64; j < y + 64; j++)
				{
					int[] TileArray = {TileID.Cloud, TileID.RainCloud,
						TileID.SnowBlock, 23 };
					for (int block = 0; block < TileArray.Length; block++)
					{
						if (Main.tile[i, j].type == (ushort)TileArray[block])
						{
							return false;
						}
					}
				}
			}
			return true;
		}
		

		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			
			int GuideIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Marble"));
			if (GuideIndex == -1)
			{
				// Guide pass removed by some other mod.
				return;
			}
			tasks.Insert(GuideIndex + 1, new PassLegacy("Reach", 
				delegate (GenerationProgress progress)
			{
				progress.Message = "Creating Hostile Settlements";
				bool success = false;
				int inset = 200;
				int spawn = WorldGen.genRand.Next(50, Main.maxTilesX / 6);
				int limit = Main.maxTilesX - spawn;
				int attempts = 0;
				int x = 0;
				int y = (int)WorldGen.worldSurface;
				while (!success)
				{
					attempts++;
					if (attempts > 1000)
					{
						success = true;
						continue;
					}
					x = WorldGen.genRand.Next(Main.maxTilesX/6, Main.maxTilesX / 3);
					y = (int)WorldGen.worldSurfaceLow;
					while (!Main.tile[x, y].active() && (double)y < Main.worldSurface)
					{
						y++;
					}
					if (Main.tile[x, y].type == TileID.Grass || Main.tile[x, y].type == TileID.Dirt || Main.tile[x, y].type == TileID.Mud)
					{
						y--;
						if (y > 150 && CanPlaceReachs(x, y))
						{
							success = true;
							continue;
						}
					}
				}
				PlaceReachs(x, y);
			}));
		
		}
    }
}