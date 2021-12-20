using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Achievements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.Ocean
{
	public class OceanPirateChest : ModTile
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
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
			TileObjectData.newTile.HookCheck = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.FindEmptyChest), -1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.AfterPlacement_Hook), -1, 0, false);
			TileObjectData.newTile.AnchorInvalidTiles = new int[] { 127 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Pirate Chest");
			AddMapEntry(new Color(179, 146, 107), name, MapChestName);
			dustType = DustID.Dirt;
			disableSmartCursor = true;
			adjTiles = new int[] { TileID.Containers };
			chestDrop = ModContent.ItemType<Items.Sets.PirateStuff.PirateChest>();
			chest = "Pirate Chest";
            TileID.Sets.HasOutlines[Type] = true;
        }

		// Current animation is a little strange, needs some work 
		//also currently uses the duelist's legacy as an unlock item lol
		public override bool HasSmartInteract() => true;

		public override ushort GetMapOption(int i, int j) => (ushort)(IsLockedChest(i, j) ? 1 : 0);

		public override bool IsLockedChest(int i, int j) => Main.tile[i, j] != null && Main.tile[i, j].frameX / 54 == 2;

		public string MapChestName(string name, int i, int j)
		{
			Tile tile = Main.tile[i, j];
			if (tile == null)
				return name;

			int left = i, top = j;

			if (tile.frameX % 54 != 0)
				left--;

			if (tile.frameY != 0)
				top--;

			int chest = Chest.FindChest(left, top);
			if (chest != -1 && Main.chest[chest].name != "")
				name += ": " + Main.chest[chest].name;
			return name;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 32, chestDrop);
			Chest.DestroyChest(i, j);
		}

		public override bool NewRightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			Main.mouseRightRelease = false;

			int left = i, top = j;
			if (tile.frameX % 54 != 0)
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
				NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f);
				player.editedChestName = false;
			}

			bool isLocked = IsLockedChest(left, top);
			if (Main.netMode == NetmodeID.MultiplayerClient && !isLocked)
			{
				if (left == player.chestX && top == player.chestY && player.chest >= 0)
				{
					player.chest = -1;
					Recipe.FindRecipes();
					Main.PlaySound(SoundID.MenuClose);
				}
				else
				{
					NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, top);
					Main.stackSplit = 600;
				}
			}
			else
			{
				if (isLocked)
				{
					int chestKey = mod.ItemType("DuelistLegacy");
					for (int k = 0; k < player.inventory.Length; k++)
					{
						if (player.inventory[k].type == chestKey && player.inventory[k].stack > 0 && Chest.Unlock(left, top))
						{
							player.inventory[k].stack--;
							if (player.inventory[k].stack <= 0)
								player.inventory[k].TurnToAir();

							if (Main.netMode == NetmodeID.MultiplayerClient)
								NetMessage.SendData(MessageID.Unlock, -1, -1, null, player.whoAmI, 1f, left, top);
							break;
						}
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

							if (PlayerInput.GrappleAndInteractAreShared)
								PlayerInput.Triggers.JustPressed.Grapple = false;

							Main.recBigList = false;
							player.chestX = left;
							player.chestY = top;
							Main.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
						}
						Recipe.FindRecipes();
					}
				}
			}
			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			int left = i, top = j;

			if (tile.frameX % 36 != 0)
				left--;

			if (tile.frameY != 0)
				top--;

			int chest = Chest.FindChest(left, top);
			player.showItemIcon2 = -1;

			if (chest < 0)
				player.showItemIconText = Language.GetTextValue("LegacyChestType.0");
			else
			{
				player.showItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : "Pirate Chest";
				if (player.showItemIconText == "Pirate Chest")
				{
					player.showItemIcon2 = IsLockedChest(left, top) ? mod.ItemType("DuelistLegacy") : mod.ItemType("DuelistLegacy");
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