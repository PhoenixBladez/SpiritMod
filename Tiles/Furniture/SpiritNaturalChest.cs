/*using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture
{
	public class SpiritNaturalChest : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileContainer[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 1200;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileValue[Type] = 500;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.newTile.HookCheck = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.FindEmptyChest), -1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.AfterPlacement_Hook), -1, 0, false);
			TileObjectData.newTile.AnchorInvalidTiles = new int[] { 127 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
            name.SetDefault("Spirit Chest");
			AddMapEntry(new Color(200, 200, 200), name, MapChestName);
			dustType = mod.DustType("Sparkle");
			disableSmartCursor = true;
			adjTiles = new int[] { TileID.Containers };
			chest = "SpiritChest";
		}

		public string MapChestName(string name, int i, int j)
		{
			int left = i;
			int top = j;
			Tile tile = Main.tile[i, j];
			if (tile.frameX % 36 != 0)
			{
				left--;
			}
			if (tile.frameY != 0)
			{
				top--;
			}
			int chest = Chest.FindChest(left, top);
			if (Main.chest[chest].name == "")
			{
				return name;
			}
			else
			{
				return name + ": " + Main.chest[chest].name;
			}
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 1;
		}

		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			Tile tile = Main.tile[i, j];
			int left = i;
			int top = j;
			if (tile.frameX % 36 != 0)
			{
				left--;
			}
			if (tile.frameY != 0)
			{
				top--;
			}
			return Chest.CanDestroyChest(left, top);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Terraria.Item.NewItem(i * 16, j * 16, 32, 32, mod.ItemType("SpiritChest"));
			Chest.DestroyChest(i, j);
		}

        public override void RightClick(int i, int j)
        {
            Player player = Main.player[Main.myPlayer];
            if ((Main.tile[i, j].frameX % 36) > 0) { i--; }//Sets i to left most coord of chest
            if ((Main.tile[i, j].frameY % 36) > 0) { j--; }//Sets j to upper most coord of chest
            Tile tile = Main.tile[i, j];

            if (tile.frameX >= 36)//If the chest is locked
            {
                if (player.HasItem(mod.ItemType("SpiritKey")) || tile.frameX == 36)//If the player has a key or if the chest was unlocked in legacy (remove the second check to discontinue legacy format support)
                {
                    Main.PlaySound(22, i * 16, j * 16, 1);

                    for (int n = 0; n < 4; n++)
                    {
                        Main.tile[n % 2, n / 2].frameX %= 36;
                        for (int k = 0; k < 4; k++)
                        {
                            Dust.NewDust(new Vector2((i + n % 2) * 16, (j + n / 2) * 16), 16, 16, 11);//Leave this line how it is, it uses int division
                        }
                    }

                    if (Main.netMode == 1)
                    {
                        NetMessage.SendData(52, -1, -1, null, 0, 1f, i, j);//number2=1f is for chests, number2=2f for doors
                    }
                }
            }
            else //if (tile.frameX != 72 && tile.frameX != 90)
            {
                Main.mouseRightRelease = false;
                int left = i;
                int top = j;
                /*if (tile.frameX % 36 != 0)
				{
					left--;
				}
				if (tile.frameY != 0)//You still need the % 36, otherwise it will have problems if right clicked while open
				{
					top--;
				}*/
             /*   if (player.sign >= 0)
                {
                    Main.PlaySound(11, -1, -1, 1);
                    player.sign = -1;
                    Main.editSign = false;
                    Main.npcChatText = "";
                }
                if (Main.editChest)
                {
                    Main.PlaySound(12, -1, -1, 1);
                    Main.editChest = false;
                    Main.npcChatText = "";
                }
                if (player.editedChestName)
                {
                    NetMessage.SendData(33, -1, -1, Main.chest[player.chest].name, player.chest, 1f);
                    player.editedChestName = false;
                }
                if (Main.netMode == 1)
                {
                    if (left == player.chestX && top == player.chestY && player.chest >= 0)
                    {
                        player.chest = -1;
                        Recipe.FindRecipes();
                        Main.PlaySound(11, -1, -1, 1);
                    }
                    else
                    {
                        NetMessage.SendData(31, -1, -1, null, left, top);
                        Main.stackSplit = 600;
                    }
                }
                else
                {
                    int chest = Chest.FindChest(left, top);
                    if (chest >= 0)
                    {
                        Main.stackSplit = 600;
                        if (chest == player.chest)
                        {
                            player.chest = -1;
                            Main.PlaySound(11, -1, -1, 1);
                        }
                        else
                        {
                            player.chest = chest;
                            Main.playerInventory = true;
                            Main.recBigList = false;
                            player.chestX = left;
                            player.chestY = top;
                            Main.PlaySound(player.chest < 0 ? 10 : 12, -1, -1, 1);
                        }
                        Recipe.FindRecipes();
                    }
                }
            }
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.player[Main.myPlayer];
            Tile tile = Main.tile[i, j];
            int left = i;
            int top = j;
            if (tile.frameX % 36 != 0)
            {
                left--;
            }
            if (tile.frameY != 0)
            {
                top--;
            }
            int chest = Chest.FindChest(left, top);
            player.showItemIcon2 = -1;
            if (chest < 0)
            {
                player.showItemIconText = Lang.chestType[0].Value;
            }
            else
            {
                player.showItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : "Crystal Chest";
                if (player.showItemIconText == "Crystal Chest")
                {
                    if (tile.frameX >= 36)
                    {
                        player.showItemIcon2 = mod.ItemType("SpiritKey");
                        player.showItemIconText = "";
                    }
                    else
                    {
                        player.showItemIcon2 = mod.ItemType("CrystalChest");
                    }
                }
            }
            player.noThrow = 2;
            player.showItemIcon = true;
        }

        public override void MouseOverFar(int i, int j)
        {
            MouseOver(i, j);
            Player player = Main.player[Main.myPlayer];
            if (player.showItemIconText == "")
            {
                player.showItemIcon = false;
                player.showItemIcon2 = 0;
            }
        }
    }
}*/