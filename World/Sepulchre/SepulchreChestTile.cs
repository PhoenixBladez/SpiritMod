using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.NPCs.Enchanted_Armor;
using SpiritMod.Projectiles;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.World.Sepulchre
{
	public class SepulchreChestTile : ModTile
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
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.newTile.HookCheck = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.FindEmptyChest), -1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.AfterPlacement_Hook), -1, 0, false);
			TileObjectData.newTile.AnchorInvalidTiles = new int[] { 127 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Sepulchre Chest");
			AddMapEntry(new Color(120, 82, 49), name);
			dustType = DustID.Dirt;
			disableSmartCursor = true;
			adjTiles = new int[] { TileID.Containers };
			chestDrop = ModContent.ItemType<SepulchreChest>();
			chest = "Sepulchre Chest";
            TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.BasicChest[Type] = true;
		}
        public override bool HasSmartInteract() => true;

        public string MapChestName(string name, int i, int j)
        {
            int left = i;
            int top = j;
            Tile tile = Main.tile[i, j];
            if (tile.frameX % 36 != 0)
                left--;
            if (tile.frameY != 0)
                top--;
            int chest = Chest.FindChest(left, top);
            if (Main.chest[chest].name == "")
                return name;
            else
                return name + ": " + Main.chest[chest].name;
        }

		public override void NumDust(int i, int j, bool fail, ref int num) => num = 1;

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 32, ModContent.ItemType<SepulchreChest>());
            Chest.DestroyChest(i, j);
            Main.PlaySound(SoundID.NPCKilled, i * 16, j * 16, 6);
        }

		public override bool IsLockedChest(int i, int j) => true;

		public override bool NewRightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
			bool anycursedarmor = false;
			for(int indexX = -70; indexX <= 70; indexX++) {
				for (int indexY = -90; indexY <= 90; indexY++) {
					if(Framing.GetTileSafely(indexX + i, indexY + j).type == mod.TileType("CursedArmor")) {
						WorldGen.KillTile(indexX + i, indexY + j);

						if(Main.netMode != NetmodeID.SinglePlayer)
							NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, indexX + i, indexY + j);
						anycursedarmor = true;
					}
				}
			}
			if(anycursedarmor) {
				CombatText.NewText(new Rectangle(i * 16, j * 16, 20, 10), Color.GreenYellow, "Trapped!");
				return true;
			}
			else if(NPC.AnyNPCs(ModContent.NPCType<Enchanted_Armor>())) {
				Main.PlaySound(new LegacySoundStyle(SoundID.NPCKilled, 6).WithPitchVariance(0.2f).WithVolume(0.5f), new Vector2(i * 16, j * 16));
				foreach (NPC npc in Main.npc.Where(x => x.active && x.type == ModContent.NPCType<Enchanted_Armor>()))
					npc.ai[1] = 30;

				return true;
			}

            Main.mouseRightRelease = false;
            int left = i;
            int top = j;
            if (tile.frameX % 36 != 0)
                left--;
            if (tile.frameY != 0)
                top--;
            if (player.sign >= 0)
            {
                Main.PlaySound(SoundID.MenuClose);
                player.sign = -1;
                Main.editSign = false;
                Main.npcChatText = "";
            }
            if (Main.editChest)
            {
                Main.PlaySound(SoundID.MenuTick);
                Main.editChest = false;
                Main.npcChatText = "";
            }
            if (player.editedChestName)
            {
                NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f, 0f, 0f, 0, 0, 0);
                player.editedChestName = false;
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                if (left == player.chestX && top == player.chestY && player.chest >= 0)
                {
                    player.chest = -1;
                    Recipe.FindRecipes();
                    Main.PlaySound(SoundID.MenuClose);
                }
                else
                {
                    NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, top, 0f, 0f, 0, 0, 0);
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
                        Main.PlaySound(SoundID.MenuClose);
                    }
                    else
                    {
                        player.chest = chest;
                        Main.playerInventory = true;
                        Main.recBigList = false;
                        player.chestX = left;
                        player.chestY = top;
                        Main.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
                    }
                    Recipe.FindRecipes();
                }
            }
            return true;
        }

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height) => offsetY = 2;

		public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            int left = i;
            int top = j;
            if (tile.frameX % 36 != 0)
                left--;
            if (tile.frameY != 0)
                top--;
            int chest = Chest.FindChest(left, top);
            player.showItemIcon2 = -1;
            if (chest < 0)
            {
                player.showItemIconText = Language.GetTextValue("LegacyChestType.0");
            }
            else
            {
                player.showItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : "Sepulchre Chest";
                if (player.showItemIconText == "Sepulchre Chest")
                {
                    player.showItemIcon2 = ModContent.ItemType<SepulchreChest>();
                    player.showItemIconText = "";
                }
            }
            player.noThrow = 2;
            player.showItemIcon = true;
        }

        public override void MouseOverFar(int i, int j)
        {
            MouseOver(i, j);
            Player player = Main.LocalPlayer;
            if (player.showItemIconText == "")
            {
                player.showItemIcon = false;
                player.showItemIcon2 = 0;
            }
        }
    }
}