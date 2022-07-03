using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.PirateStuff;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.Ocean
{
	public class OceanPirateChest : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileContainer[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 1200;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileOreFinderPriority[Type] = 500;

			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.CanBeClearedDuringGeneration[Type] = false;
			TileID.Sets.CanBeClearedDuringOreRunner[Type] = false;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
			TileObjectData.newTile.AnchorInvalidTiles = new int[] { 127 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Pirate Chest");
			AddMapEntry(new Color(161, 115, 54), name, MapChestName);
			name = CreateMapEntryName(Name + "_Locked"); // With multiple map entries, you need unique translation keys.
			name.SetDefault("Locked Pirate Chest");
			AddMapEntry(new Color(87, 64, 31), name, MapChestName);

			DustType = DustID.Dirt;
			TileID.Sets.DisableSmartCursor[Type] = true;
			AdjTiles = new int[] { TileID.Containers };
			ChestDrop = ModContent.ItemType<PirateChest>();
			ContainerName.SetDefault("Pirate Chest");
        }

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

		public override ushort GetMapOption(int i, int j) => (ushort)(IsLockedChest(i, j) ? 1 : 0);

		public override bool IsLockedChest(int i, int j) => Main.tile[i, j] != null && Main.tile[i, j].TileFrameX > 18;

		public string MapChestName(string name, int i, int j)
		{
			Tile tile = Main.tile[i, j];
			if (tile == null)
				return name;

			int left = i, top = j;

			if (tile.TileFrameX % 54 != 0)
				left--;

			if (tile.TileFrameY != 0)
				top--;

			int chest = Chest.FindChest(left, top);
			if (chest != -1 && Main.chest[chest].name != "")
				name += ": " + Main.chest[chest].name;
			return name;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ChestDrop);
			Chest.DestroyChest(i, j);
		}

		public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			Main.mouseRightRelease = false;

			int left = i, top = j;
			if (tile.TileFrameX % 36 != 0)
				left--;
			if (tile.TileFrameY != 0)
				top--;

			if (player.sign >= 0)
			{
				SoundEngine.PlaySound(SoundID.MenuClose);
				player.sign = -1;
				Main.editSign = false;
				Main.npcChatText = "";
			}

			if (Main.editChest)
			{
				SoundEngine.PlaySound(SoundID.MenuTick);
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
					SoundEngine.PlaySound(SoundID.MenuClose);
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
					int chestKey = ModContent.ItemType<PirateKey>();
					for (int k = 0; k < player.inventory.Length; k++)
					{
						if (player.inventory[k].type == chestKey && player.inventory[k].stack > 0)
						{
							for (int l = 0; l < 2; ++l) //Look into why Chest.Unlock(left, top) doesn't work???
								for (int v = 0; v < 2; ++v)
									Framing.GetTileSafely(left + l, top + v).TileFrameX -= 36;

							SoundEngine.PlaySound(SoundID.Unlock, new Vector2(left * 16, top * 16));

							if (--player.inventory[k].stack <= 0)
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
							SoundEngine.PlaySound(SoundID.MenuClose);
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
							SoundEngine.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
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

			if (tile.TileFrameX % 36 != 0)
				left--;

			if (tile.TileFrameY != 0)
				top--;

			int chest = Chest.FindChest(left, top);
			player.cursorItemIconID = -1;

			if (chest < 0)
				player.cursorItemIconText = Language.GetTextValue("LegacyChestType.0");
			else
			{
				player.cursorItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : "Pirate Chest";
				if (player.cursorItemIconText == "Pirate Chest")
				{
					player.cursorItemIconID = IsLockedChest(left, top) ? ModContent.ItemType<PirateKey>() : ModContent.ItemType<PirateChest>();
					player.cursorItemIconText = "";
				}
			}
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
		}

		public override void MouseOverFar(int i, int j)
		{
			MouseOver(i, j);
			Player player = Main.LocalPlayer;
			if (player.cursorItemIconText == "")
			{
				player.cursorItemIconEnabled = false;
				player.cursorItemIconID = 0;
			}
		}
	}
}