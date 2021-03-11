using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.GameInput;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Mechanics.Fathomless_Chest
{
	public class Fathomless_Chest : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 1200;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
			TileObjectData.newTile.Origin = new Point16(0, 2);
			Main.tileValue[Type] = 1000;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Fathomless Vase");
			Main.tileSpelunker[Type] = true;
			AddMapEntry(new Color(112, 216, 238), name);
			disableSmartCursor = true;
			Main.tileLighted[Type] = true;
			dustType = 278;
			soundType = 42;
			soundStyle = 170;
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0f;
			g = 0.2f;
			b = 0.5f;
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return false;
		}
		public override void RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			WorldGen.KillTile(i, j, false, false, false);
		}
		public override bool HasSmartInteract()
		{
			return false;
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			player.showItemIcon2 = 73;
			player.showItemIconText = "";
			player.noThrow = 2;
			player.showItemIcon = true;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Structures.Fathomless_Chest.Fathomless_Chest_World.isThereAChest = false;
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(42, 186));
			Item.NewItem((int)(i * 16), (int)(j * 16) - 12, 16*2, 16*3, mod.ItemType("Black_Stone_Item"), Main.rand.Next(3,7), false, 0, false, false);
			for (int index1 = 0; index1 < 3; ++index1)
			{
				for (int index2 = 0; index2 < 2; ++index2)
				{
					int index3 = Gore.NewGore(new Vector2(i * 16, j * 16), new Vector2(0.0f, 0.0f), 99, 1.1f);
					Main.gore[index3].velocity *= 0.6f;
				}
			}
			for (int index = 0; index < 9; ++index)
			{
				float SpeedX = (float)(-(double)1 * (double)Main.rand.Next(40, 70) * 0.00999999977648258 + (double)Main.rand.Next(-20, 21) * 0.400000005960464);
				float SpeedY = (float)(-(double)1 * (double)Main.rand.Next(40, 70) * 0.00999999977648258 + (double)Main.rand.Next(-20, 21) * 0.400000005960464);
				int p = Projectile.NewProjectile((float)(i * 16) + 8 + SpeedX, (float)(j * 16) + 12 + SpeedY, SpeedX, SpeedY, mod.ProjectileType("Visual_Projectile"), 0, 0f, player.whoAmI, 0.0f, 0.0f);
				Main.projectile[p].scale = Main.rand.Next(30,150)*0.01f;
			}
			
			int spawnposXa = Main.rand.Next(Main.maxTilesX);
            int spawnposYa = WorldGen.genRand.Next(Convert.ToInt32(Main.rockLayer), Convert.ToInt32(Main.rockLayer + 500));
            bool safetyCheckLihzard = 
			Main.tile[spawnposXa - 10,spawnposYa - 10].type == TileID.LihzahrdBrick || Main.tile[spawnposXa + 10,spawnposYa + 10].type == TileID.LihzahrdBrick ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 87 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 87;
           

			bool safetyCheckDungeonWalls = 
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 7 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 7 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 8 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 8 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 9 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 9 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 94 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 94 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 98 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 98 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 96 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 96 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 97 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 97 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 99 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 99 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 95 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 95;
			
			bool safetyCheckDungeonTiles = 
			Main.tile[spawnposXa - 10,spawnposYa - 10].type == 41 || Main.tile[spawnposXa + 10,spawnposYa + 10].type == 41 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].type == 43 || Main.tile[spawnposXa + 10,spawnposYa + 10].type == 43 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].type == 44 || Main.tile[spawnposXa + 10,spawnposYa + 10].type == 44;
			
            while (safetyCheckLihzard || safetyCheckDungeonWalls || safetyCheckDungeonTiles)
            {
				spawnposXa = Main.rand.Next(Main.maxTilesX);
                spawnposYa = WorldGen.genRand.Next(Convert.ToInt32(Main.rockLayer), Convert.ToInt32(Main.rockLayer + 500));
				
				safetyCheckLihzard = 
				Main.tile[spawnposXa - 10,spawnposYa - 10].type == TileID.LihzahrdBrick || Main.tile[spawnposXa + 10,spawnposYa + 10].type == TileID.LihzahrdBrick ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 87 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 87;
				
				safetyCheckDungeonWalls = 
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 7 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 7 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 8 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 8 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 9 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 9 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 94 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 94 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 98 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 98 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 96 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 96 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 97 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 97 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 99 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 99 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 95 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 95;
				
				safetyCheckDungeonTiles = 
				Main.tile[spawnposXa - 10,spawnposYa - 10].type == 41 || Main.tile[spawnposXa + 10,spawnposYa + 10].type == 41 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].type == 43 || Main.tile[spawnposXa + 10,spawnposYa + 10].type == 43 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].type == 44 || Main.tile[spawnposXa + 10,spawnposYa + 10].type == 44;
            }
			if (Main.rand.Next(3)==0)
			{
				PlaceShrine(spawnposXa, spawnposYa, Structures.Fathomless_Chest.Fathomless_Chest_Arrays.ShrineShape1);
				PlaceShrineMiscs(spawnposXa, spawnposYa, Structures.Fathomless_Chest.Fathomless_Chest_Arrays.Miscs);
				Structures.Fathomless_Chest.Fathomless_Chest_World.isThereAChest = true;
			}
			
			//int randomEffectCounter = 5;
			int randomEffectCounter = Main.rand.Next(6);
			switch (randomEffectCounter)
			{
				case 0: //SPAWN ZOMBIES
				{
					BadLuck();
					int a = NPC.NewNPC((int)(i * 16) + -8 - 16 - 16, (j * 16) + 6, 3);
					int b = NPC.NewNPC((int)(i * 16) + -8 - 16, (j * 16) + 6, 3);
					int c = NPC.NewNPC((int)(i * 16) + 8, (j * 16) + 6, 430);
					int d = NPC.NewNPC((int)(i * 16) + 24 + 16, (j * 16) + 6, 3);
					int e = NPC.NewNPC((int)(i * 16) + 24 + 32, (j * 16) + 6, 3);
					break;
				}
				case 1: //DROP COINS
				{
					BadLuck();
					Main.PlaySound(new Terraria.Audio.LegacySoundStyle(18, 0));
					int num1 = 0;
					for (int index = 0; index < 59; ++index)
					{
						if (player.inventory[index].type >= 71 && player.inventory[index].type <= 74)
						{
							int number = Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, player.inventory[index].type, 1, false, 0, false, false);
							int num2 = player.inventory[index].stack / 5;
							if (Main.expertMode)
							{
								num2 = (int)((double)player.inventory[index].stack * 0.25);
							}

							int num3 = player.inventory[index].stack - num2;
							player.inventory[index].stack -= num3;
							if (player.inventory[index].type == 71)
							{
								num1 += num3;
							}

							if (player.inventory[index].type == 72)
							{
								num1 += num3 * 100;
							}

							if (player.inventory[index].type == 73)
							{
								num1 += num3 * 10000;
							}

							if (player.inventory[index].type == 74)
							{
								num1 += num3 * 1000000;
							}

							if (player.inventory[index].stack <= 0)
							{
								player.inventory[index] = new Item();
							}

							Main.item[number].stack = num3;
							Main.item[number].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
							Main.item[number].velocity.X = (float)Main.rand.Next(-20, 21) * 0.2f;
							Main.item[number].noGrabDelay = 600;
							if (index == 58)
							{
								Main.mouseItem = player.inventory[index].Clone();
							}
						}
					}
					player.lostCoins = num1;
					player.lostCoinString = Main.ValueToCoins(player.lostCoins);
					break;
				}
				case 2: //SPAWN POTIONS
				{
					GoodLuck();
					int randomPotion = Utils.SelectRandom<int>(Main.rand, new int[38] { 2344, 303, 300, 2325, 2324, 2356, 2329, 2346, 295, 2354, 2327, 291, 305, 2323, 304, 2348, 297, 292, 2345, 2352, 294, 293, 2322, 299, 288, 2347, 289, 298, 2355, 296, 2353, 2328, 290, 301, 2326, 2359, 302, 2349 });
					int randomPotion2 = Utils.SelectRandom<int>(Main.rand, new int[38] { 2344, 303, 300, 2325, 2324, 2356, 2329, 2346, 295, 2354, 2327, 291, 305, 2323, 304, 2348, 297, 292, 2345, 2352, 294, 293, 2322, 299, 288, 2347, 289, 298, 2355, 296, 2353, 2328, 290, 301, 2326, 2359, 302, 2349 });
					int randomPotion3 = Utils.SelectRandom<int>(Main.rand, new int[38] { 2344, 303, 300, 2325, 2324, 2356, 2329, 2346, 295, 2354, 2327, 291, 305, 2323, 304, 2348, 297, 292, 2345, 2352, 294, 293, 2322, 299, 288, 2347, 289, 298, 2355, 296, 2353, 2328, 290, 301, 2326, 2359, 302, 2349 });
					int randomPotion4 = Utils.SelectRandom<int>(Main.rand, new int[38] { 2344, 303, 300, 2325, 2324, 2356, 2329, 2346, 295, 2354, 2327, 291, 305, 2323, 304, 2348, 297, 292, 2345, 2352, 294, 293, 2322, 299, 288, 2347, 289, 298, 2355, 296, 2353, 2328, 290, 301, 2326, 2359, 302, 2349 });
					int number = Item.NewItem((int)(i * 16) + 8, (int)(j * 16) + 12, 16, 18, randomPotion, 1, false, 0, false, false);
					Main.item[number].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
					Main.item[number].velocity.X = (float)Main.rand.Next(-20, 21) * 0.2f;
					int number2 = Item.NewItem((int)(i * 16) + 8, (int)(j * 16) + 12, 16, 18, randomPotion2, 1, false, 0, false, false);
					Main.item[number2].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
					Main.item[number2].velocity.X = (float)Main.rand.Next(-20, 21) * 0.2f;
					int number3 = Item.NewItem((int)(i * 16) + 8, (int)(j * 16) + 12, 16, 18, randomPotion3, 1, false, 0, false, false);
					Main.item[number3].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
					Main.item[number3].velocity.X = (float)Main.rand.Next(-20, 21) * 0.2f;
					int number4 = Item.NewItem((int)(i * 16) + 8, (int)(j * 16) + 12, 16, 18, randomPotion4, 1, false, 0, false, false);
					Main.item[number4].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
					Main.item[number4].velocity.X = (float)Main.rand.Next(-20, 21) * 0.2f;
					break;
				}
				case 3: //SPAWN AN ITEM
				{
					GoodLuck();
					Item.NewItem((int)(i * 16) + 8, (int)(j * 16) + 12, 16, 18, 393, 1, false, 0, false, false);
					Item.NewItem((int)(i * 16) + 8, (int)(j * 16) + 12, 16, 18, 18, 1, false, 0, false, false);
					break;
				}
				case 4: //SPAWN BUNNIES
				{
					NeutralLuck();
					float npcposX = 0f;
					float npcposY = 0f;
					for (int g = 0; g < 8 +	Main.rand.Next(6); g++)
					{
						npcposX = (i * 16) + Main.rand.Next(-60,60);
						npcposY = (j * 16) + Main.rand.Next(-60,60);
						int a = NPC.NewNPC((int)npcposX, (int)npcposY, 46);
						Main.npc[a].value = 0;
						Main.npc[a].friendly = false;
						Main.npc[a].dontTakeDamage = true;
						Vector2 spinningpoint = new Vector2(0.0f, -3f).RotatedByRandom(3.14159274101257);
						float num1 = (float) 28;
						Vector2 vector2 = new Vector2(1.1f, 1f);
						for (float num2 = 0.0f; (double) num2 < (double) num1; ++num2)
						{
						  int dustIndex = Dust.NewDust(new Vector2(npcposX,npcposY), 0, 0, 15, 0.0f, 0.0f, 0, new Color(), 1f);
						  Main.dust[dustIndex].position = new Vector2(npcposX,npcposY);
						  Main.dust[dustIndex].velocity = spinningpoint.RotatedBy(6.28318548202515 * (double) num2 / (double) num1, new Vector2()) * vector2 * (float) (0.800000011920929 + (double) Main.rand.NextFloat() * 0.400000005960464);
						  Main.dust[dustIndex].noGravity = true;
						  Main.dust[dustIndex].scale = 2f;
						  Main.dust[dustIndex].fadeIn = Main.rand.NextFloat() * 2f;
						  Dust dust = Dust.CloneDust(dustIndex);
						  dust.scale /= 2f;
						  dust.fadeIn /= 2f;
						}
						Main.PlaySound(2, (int)npcposX, (int)npcposY, 6, 1f, 0f);
					}
					break;
				}
				case 5: //WATER BOTTLE N LAVA
				{
					NeutralLuck();
					Main.tile[i, j].lava(true);
					Main.tile[i, j].liquid = byte.MaxValue;
					Main.tile[i-4, j+2].lava(true);
					Main.tile[i-4, j+2].liquid = byte.MaxValue;
					Main.tile[i+4, j+2].lava(true);
					Main.tile[i+4, j+2].liquid = byte.MaxValue;
					Item.NewItem((int)(i * 16), (int)(j * 16) - 12, 16, 18, mod.ItemType("Water_Bottle"), 1, false, 0, false, false);
					break;
				}
			}
		}
		private void PlaceShrineMiscs(int i, int j, int[,] ShrineArray) 
		{
			for (int y = 0; y < ShrineArray.GetLength(0); y++) { // Third Loop Places Furnitures.(So that they have blocks to spawn on).
				for (int x = 0; x < ShrineArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					Tile tile = Framing.GetTileSafely(k, l);
					if (WorldGen.InWorld(k, l, 30) && (tile.type != 41 || tile.type != 43 || tile.type != 44 || tile.type != 226))
					{					
						switch (ShrineArray[y, x]) {
							case 8:
								
								WorldGen.PlaceObject(k, l, 42, false, 2); //Lantern
								break;
						}
					}
				}
			}
		}
		private void PlaceShrine(int i, int j, int[,] ShrineArray) 
		{
			for (int y = 0; y < ShrineArray.GetLength(0); y++) 
			{ // First loop is here to clear tiles properly, and not delete objects afterwards.
				for (int x = 0; x < ShrineArray.GetLength(1); x++) 
				{
					int k = i - 3 + x;
					int l = j - 6 + y;
					Tile tile = Framing.GetTileSafely(k, l);
					if (WorldGen.InWorld(k, l, 30) && (tile.type != 41 || tile.type != 43 || tile.type != 44 || tile.type != 226))
					{
						switch (ShrineArray[y, x])
						{		
							case 0:						
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 2:						
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 3:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 4:
								Framing.GetTileSafely(k, l).ClearEverything();
								break;
							case 5:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 6:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 7:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 8:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 9:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
						}
					}
				}
			}
			
			for (int y = 0; y < ShrineArray.GetLength(0); y++) { // Second Loop Places Blocks
				for (int x = 0; x < ShrineArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					Tile tile = Framing.GetTileSafely(k, l);
					if (WorldGen.InWorld(k, l, 30) && (tile.type != 41 || tile.type != 43 || tile.type != 44 || tile.type != 226)) {
						switch (ShrineArray[y, x]) {
							case 0:
								break;
							case 1:				
								break;
							case 2:
								
								WorldGen.PlaceTile(k, l, mod.TileType("Black_Stone")); // Dirt
								WorldGen.PlaceWall(k, l, 1); // Stone Wall	
								tile.active(true);
								break;
							case 3:
								
								WorldGen.PlaceTile(k, l, 311); // Dynasty Wood							
								tile.active(true);
								break;
							case 4:
								
								WorldGen.PlaceWall(k, l, 139); // Rich Mahogany Fence	
								WorldGen.PlaceObject(k, l, 105, false, 31); // Tree Statue								
								break;
							case 5:
								
								WorldGen.PlaceTile(k, l, 313); // Blue Dynasty Shingles			
								tile.active(true);
								break;
							case 6:
								
								WorldGen.PlaceTile(k, l, 313); // Blue Dynasty Shingles
								tile.active(true);
								tile.slope(0); // I give up, this is retarded
								break;
							case 7:
								WorldGen.PlaceTile(k, l, 311); // Dynasty Wood
								WorldGen.PlaceWall(k, l, 139); // Rich Mahogany Fence
								
								tile.active(true);
								break;
						}
					}
				}
			}
			
			for (int y = 0; y < ShrineArray.GetLength(0); y++) { // Third Loop Places Furnitures.(So that they have blocks to spawn on).
				for (int x = 0; x < ShrineArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (ShrineArray[y, x]) {
							case 8:
								
								WorldGen.PlaceObject(k, l, 105, false, 31); // Tree Statue
								break;
							case 9:
								
								WorldGen.PlaceTile(k, l, mod.TileType("Fathomless_Chest")); // Blue Dynasty Shingles
								break;
						}
					}
				}
			}
		}

		public void BadLuck()
		{
			Player player = Main.LocalPlayer;
			int index = CombatText.NewText(new Microsoft.Xna.Framework.Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(255,150,150), string.Concat((object)"Bad Luck!"), false, false);
			CombatText combatText = Main.combatText[index];
			NetMessage.SendData(81, -1, -1, NetworkText.FromLiteral(combatText.text), (int)combatText.color.PackedValue, combatText.position.X, combatText.position.Y, 0.0f, 0, 0, 0);
		}
		public void GoodLuck()
		{
			Player player = Main.LocalPlayer;
			int index = CombatText.NewText(new Microsoft.Xna.Framework.Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(150, 255, 150), string.Concat((object)"Good Luck!"), false, false);
			CombatText combatText = Main.combatText[index];
			NetMessage.SendData(81, -1, -1, NetworkText.FromLiteral(combatText.text), (int)combatText.color.PackedValue, combatText.position.X, combatText.position.Y, 0.0f, 0, 0, 0);
		}
		public void NeutralLuck()
		{
			Player player = Main.LocalPlayer;
			int index = CombatText.NewText(new Microsoft.Xna.Framework.Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(150, 150, 255), string.Concat((object)"Neutral Luck!"), false, false);
			CombatText combatText = Main.combatText[index];
			NetMessage.SendData(81, -1, -1, NetworkText.FromLiteral(combatText.text), (int)combatText.color.PackedValue, combatText.position.X, combatText.position.Y, 0.0f, 0, 0, 0);
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			int left = i - tile.frameX / 18;
			int top = j - tile.frameY / 18;
			int spawnX = left * 16;
            int spawnY = top * 16;	
			if (Main.rand.Next(20) == 0)
			{
				int index3 = Dust.NewDust(new Vector2(spawnX, spawnY), 16*2, 16*3, 180, 0.0f, 0.0f, 150, new Color(), 0.3f);
				Main.dust[index3].fadeIn = 0.75f;
				Dust dust = Main.dust[index3];
				Vector2 vector2_2 = dust.velocity * 0.1f;
				dust.velocity = vector2_2;
				Main.dust[index3].noLight = true;
				Main.dust[index3].noGravity = true;
			}
			return true;
		}
	}

	internal class Fathomless_Chest_Item : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fathomless Vase");
			Tooltip.SetDefault("You aren't supposed to have this!");
		}

		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 999;
			item.useTurn = true;
			item.value = 0;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.rare = 8;
			item.createTile = mod.TileType("Fathomless_Chest");
		}
	}
}