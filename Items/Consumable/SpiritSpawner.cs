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

namespace SpiritMod.Items.Consumable
{
    public class SpiritSpawner : ModItem
    {
		 private int WillGenn = 0;
		 private int Meme;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Spawner");
			Tooltip.SetDefault("Spawns the spirit biome \nCheat item \nDo not use if your world already has the Spirit Biome");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.rare = 1;
            item.maxStack = 1;
            item.noUseGraphic = true;
            item.useStyle = 1;
            item.useTime = item.useAnimation = 60;

            item.noMelee = true;
            item.consumable = true;
            item.autoReuse = false;

        }
        public override bool UseItem(Player player)
        {
            #region SpiritBiome
                    Main.NewText("The Spirits spread through the Land...", Color.Orange.R, Color.Orange.G, Color.Orange.B);
                    Random rand = new Random();
                    int XTILE;
                    if (Terraria.Main.dungeonX > Main.maxTilesX / 2) //rightside dungeon
                    {
                        XTILE = WorldGen.genRand.Next(Main.maxTilesX / 2, Main.maxTilesX - 500);
                    }
                    else //leftside dungeon
                    {
                        XTILE = WorldGen.genRand.Next(75, Main.maxTilesX / 2);
                    }
                    int xAxis = XTILE;
                    int xAxisMid = xAxis + 70;
                    int xAxisEdge = xAxis + 380;
                    int yAxis = 0;
                    for (int y = 0; y < Main.maxTilesY; y++)
                    {
                        yAxis++;
                        xAxis = XTILE;

                        for (int i = 0; i < 450; i++)
                        {
                            xAxis++;
							#region islands
							if (Main.rand.Next(21000) == 1)
							{
								WorldMethods.Island(xAxis, Main.rand.Next(100, 275), Main.rand.Next(10, 16), (float)(Main.rand.Next(11, 25) / 10), (ushort)mod.TileType("SpiritGrass"));
							}
							#endregion
                            
                            if (Main.tile[xAxis, yAxis] != null)
                            {
                                if (Main.tile[xAxis, yAxis].active())
                                {
                                    int[] TileArray = { 0 };
                                    if (TileArray.Contains(Main.tile[xAxis, yAxis].type))
                                    {
                                        if (Main.tile[xAxis, yAxis + 1] == null)
                                        {
                                            if (Main.rand.Next(0, 50) == 1)
                                            {
                                                WillGenn = 0;
                                                if (xAxis < xAxisMid - 1)
                                                {
                                                    Meme = xAxisMid - xAxis;
                                                    WillGenn = Main.rand.Next(Meme);
                                                }
                                                if (xAxis > xAxisEdge + 1)
                                                {
                                                    Meme = xAxis - xAxisEdge;
                                                    WillGenn = Main.rand.Next(Meme);
                                                }
                                                if (WillGenn < 10)
                                                {
                                                    Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("SpiritDirt");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            WillGenn = 0;
                                            if (xAxis < xAxisMid - 1)
                                            {
                                                Meme = xAxisMid - xAxis;
                                                WillGenn = Main.rand.Next(Meme);
                                            }
                                            if (xAxis > xAxisEdge + 1)
                                            {
                                                Meme = xAxis - xAxisEdge;
                                                WillGenn = Main.rand.Next(Meme);
                                            }
                                            if (WillGenn < 10)
                                            {
                                                Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("SpiritDirt");
                                            }
                                        }
                                    }
									int[] TileArray89 = {3, 24, 110, 113, 115, 201, 205, 52, 62, 32, 165};
                                        if (TileArray89.Contains(Main.tile[xAxis, yAxis].type))
                                        {
                                            if (Main.tile[xAxis, yAxis + 1] == null)
                                            {
                                                if (rand.Next(0, 50) == 1)
                                                {
                                                    WillGenn = 0;
                                                    if (xAxis < xAxisMid - 1)
                                                    {
                                                        Meme = xAxisMid - xAxis;
                                                        WillGenn = Main.rand.Next(Meme);
                                                    }
                                                    if (xAxis > xAxisEdge + 1)
                                                    {
                                                        Meme = xAxis - xAxisEdge;
                                                        WillGenn = Main.rand.Next(Meme);
                                                    }
                                                    if (WillGenn < 18)
                                                    {
                                                        Main.tile[xAxis, yAxis].active(false);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                WillGenn = 0;
                                                if (xAxis < xAxisMid - 1)
                                                {
                                                    Meme = xAxisMid - xAxis;
                                                    WillGenn = Main.rand.Next(Meme);
                                                }
                                                if (xAxis > xAxisEdge + 1)
                                                {
                                                    Meme = xAxis - xAxisEdge;
                                                    WillGenn = Main.rand.Next(Meme);
                                                }
                                                if (WillGenn < 18)
                                                {
                                                    Main.tile[xAxis, yAxis].active(false);
                                                }
                                            }
                                        }
                                    int[] TileArray1 = { 2, 23, 109, 199 };
                                    if (TileArray1.Contains(Main.tile[xAxis, yAxis].type))
                                    {
                                        if (Main.tile[xAxis, yAxis + 1] == null)
                                        {
                                            if (rand.Next(0, 50) == 1)
                                            {
                                                WillGenn = 0;
                                                if (xAxis < xAxisMid - 1)
                                                {
                                                    Meme = xAxisMid - xAxis;
                                                    WillGenn = Main.rand.Next(Meme);
                                                }
                                                if (xAxis > xAxisEdge + 1)
                                                {
                                                    Meme = xAxis - xAxisEdge;
                                                    WillGenn = Main.rand.Next(Meme);
                                                }
                                                if (WillGenn < 18)
                                                {
                                                    Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("SpiritGrass");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            WillGenn = 0;
                                            if (xAxis < xAxisMid - 1)
                                            {
                                                Meme = xAxisMid - xAxis;
                                                WillGenn = Main.rand.Next(Meme);
                                            }
                                            if (xAxis > xAxisEdge + 1)
                                            {
                                                Meme = xAxis - xAxisEdge;
                                                WillGenn = Main.rand.Next(Meme);
                                            }
                                            if (WillGenn < 18)
                                            {
                                                Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("SpiritGrass");
                                            }
                                        }
                                    }

                                    int[] TileArray2 = { 1, 25, 117, 203 };
                                    if (TileArray2.Contains(Main.tile[xAxis, yAxis].type))
                                    {
                                        if (Main.tile[xAxis, yAxis + 1] == null)
                                        {
                                            if (rand.Next(0, 50) == 1)
                                            {
                                                WillGenn = 0;
                                                if (xAxis < xAxisMid - 1)
                                                {

                                                    Meme = xAxisMid - xAxis;
                                                    WillGenn = Main.rand.Next(Meme);
                                                }
                                                if (xAxis > xAxisEdge + 1)
                                                {
                                                    Meme = xAxis - xAxisEdge;
                                                    WillGenn = Main.rand.Next(Meme);
                                                }
                                                if (WillGenn < 18)
                                                {
                                                    Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("SpiritStone");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            WillGenn = 0;
                                            if (xAxis < xAxisMid - 1)
                                            {
                                                Meme = xAxisMid - xAxis;
                                                WillGenn = Main.rand.Next(Meme);
                                            }
                                            if (xAxis > xAxisEdge + 1)
                                            {
                                                Meme = xAxis - xAxisEdge;
                                                WillGenn = Main.rand.Next(Meme);
                                            }
                                            if (WillGenn < 18)
                                            {
                                                Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("SpiritStone");
                                            }
                                        }
                                    }

                                    int[] TileArray3 = { 53, 116, 112, 234 };
                                    if (TileArray3.Contains(Main.tile[xAxis, yAxis].type))
                                    {
                                        if (Main.tile[xAxis, yAxis + 1] == null)
                                        {
                                            if (rand.Next(0, 50) == 1)
                                            {
                                                WillGenn = 0;
                                                if (xAxis < xAxisMid - 1)
                                                {

                                                    Meme = xAxisMid - xAxis;
                                                    WillGenn = Main.rand.Next(Meme);
                                                }
                                                if (xAxis > xAxisEdge + 1)
                                                {
                                                    Meme = xAxis - xAxisEdge;
                                                    WillGenn = Main.rand.Next(Meme);
                                                }
                                                if (WillGenn < 18)
                                                {
                                                    Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("Spiritsand");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            WillGenn = 0;
                                            if (xAxis < xAxisMid - 1)
                                            {
                                                Meme = xAxisMid - xAxis;
                                                WillGenn = Main.rand.Next(Meme);
                                            }
                                            if (xAxis > xAxisEdge + 1)
                                            {
                                                Meme = xAxis - xAxisEdge;
                                                WillGenn = Main.rand.Next(Meme);
                                            }
                                            if (WillGenn < 18)
                                            {
                                                Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("Spiritsand");
                                            }
                                        }
                                    }

                                    int[] TileArray4 = { 161, 163, 200, 164 };
                                    if (TileArray4.Contains(Main.tile[xAxis, yAxis].type))
                                    {
                                        if (Main.tile[xAxis, yAxis + 1] == null)
                                        {
                                            if (rand.Next(0, 50) == 1)
                                            {
                                                WillGenn = 0;
                                                if (xAxis < xAxisMid - 1)
                                                {

                                                    Meme = xAxisMid - xAxis;
                                                    WillGenn = Main.rand.Next(Meme);
                                                }
                                                if (xAxis > xAxisEdge + 1)
                                                {
                                                    Meme = xAxis - xAxisEdge;
                                                    WillGenn = Main.rand.Next(Meme);
                                                }
                                                if (WillGenn < 18)
                                                {
                                                    Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("SpiritIce");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            WillGenn = 0;
                                            if (xAxis < xAxisMid - 1)
                                            {
                                                Meme = xAxisMid - xAxis;
                                                WillGenn = Main.rand.Next(Meme);
                                            }
                                            if (xAxis > xAxisEdge + 1)
                                            {
                                                Meme = xAxis - xAxisEdge;
                                                WillGenn = Main.rand.Next(Meme);
                                            }
                                            if (WillGenn < 18)
                                            {
                                                Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("SpiritIce");
                                            }
                                        }
                                    }
                                }
                                if (Main.tile[xAxis, yAxis].type == mod.TileType("SpiritStone") && yAxis > WorldGen.rockLayer + 100 && Main.rand.Next(1500) == 6)
                                {
                                    WorldGen.TileRunner(xAxis, yAxis, (double)WorldGen.genRand.Next(5, 7), 1, mod.TileType("SpiritOreTile"), false, 0f, 0f, true, true);
                                }
                            }
                            
                        }
                    }
					int chests = 0;
					for (int r = 0; r < 380000; r++)
							{
								
								int success = WorldGen.PlaceChest(xAxis - Main.rand.Next(450), Main.rand.Next(100, 275), (ushort)mod.TileType("SpiritChestLocked"), false, 2);
								if (success > -1)
								{
									string[] lootTable = { "GhastKnife", "GhastStaff", "GhastStaffMage", "GhastSword", "GhastBeam",};
									Main.chest[success].item[0].SetDefaults(mod.ItemType(lootTable[chests]), false);

                    									int[] lootTable2 = {499, 1508, mod.ItemType("SpiritBar"), };
									
									Main.chest[success].item[1].SetDefaults(lootTable2[Main.rand.Next(3)], false);
									Main.chest[success].item[1].stack = WorldGen.genRand.Next(3,8);
									Main.chest[success].item[2].SetDefaults(lootTable2[Main.rand.Next(3)], false);
									Main.chest[success].item[2].stack = WorldGen.genRand.Next(3,8);
									Main.chest[success].item[3].SetDefaults(lootTable2[Main.rand.Next(3)], false);
									Main.chest[success].item[3].stack = WorldGen.genRand.Next(3,8);
									chests++;
									if (chests >= 5)
									{
									break;
									}
								}
							}
					#endregion
            return true;
        }

        }
}
