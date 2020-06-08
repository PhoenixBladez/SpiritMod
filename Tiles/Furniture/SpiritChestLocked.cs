using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Furniture.Reach;
using SpiritMod.NPCs.Critters;
using SpiritMod.Mounts;
using SpiritMod.NPCs.Boss.SpiritCore;
using SpiritMod.Boss.SpiritCore;
using SpiritMod.Buffs.Candy;
using SpiritMod.Buffs.Potion;
using SpiritMod.Projectiles.Pet;
using SpiritMod.Buffs.Pet;
using SpiritMod.Projectiles.Arrow.Artifact;
using SpiritMod.Projectiles.Bullet.Crimbine;
using SpiritMod.Projectiles.Bullet;
using SpiritMod.Projectiles.Magic.Artifact;
using SpiritMod.Projectiles.Summon.Artifact;
using SpiritMod.Projectiles.Summon.LaserGate;
using SpiritMod.Projectiles.Flail;
using SpiritMod.Projectiles.Arrow;
using SpiritMod.Projectiles.Magic;
using SpiritMod.Projectiles.Sword.Artifact;
using SpiritMod.Projectiles.Summon.Dragon;
using SpiritMod.Projectiles.Sword;
using SpiritMod.Projectiles.Thrown.Artifact;
using SpiritMod.Items.Boss;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Projectiles.Returning;
using SpiritMod.Projectiles.Held;
using SpiritMod.Projectiles.Thrown;
using SpiritMod.Items.Equipment;
using SpiritMod.Projectiles.DonatorItems;
using SpiritMod.Buffs.Mount;
using SpiritMod.Items.Weapon.Yoyo;
using SpiritMod.Projectiles.Yoyo;
using SpiritMod.Items.Weapon.Spear;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.NPCs.Boss;
using SpiritMod.Items.Material;
using SpiritMod.Items.Pets;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Projectiles.Boss;
using SpiritMod.Items.BossBags;
using SpiritMod.Items.Consumable.Fish;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon;
using SpiritMod.NPCs.Spirit;
using SpiritMod.Items.Consumable;
using SpiritMod.Tiles.Block;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Items.Consumable.Quest;
using SpiritMod.Items.Consumable.Potion;
using SpiritMod.Items.Placeable.IceSculpture;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Buffs;
using SpiritMod.Items;
using SpiritMod.Items.Weapon;
using SpiritMod.Items.Weapon.Returning;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Accessory;

using SpiritMod.Items.Accessory.Leather;
using SpiritMod.Items.Ammo;
using SpiritMod.Items.Armor;
using SpiritMod.Dusts;
using SpiritMod.Buffs;
using SpiritMod.Buffs.Artifact;
using SpiritMod.NPCs;
using SpiritMod.NPCs.Asteroid;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.Tiles;
using SpiritMod.Tiles.Ambient;
using SpiritMod.Tiles.Ambient.IceSculpture;
using SpiritMod.Tiles.Ambient.ReachGrass;
using SpiritMod.Tiles.Ambient.ReachMicros;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Placeable.Furniture;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture
{
    public class SpiritChestLocked : ModTile
    {
        public override void SetDefaults() {
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
            name.SetDefault("Spirit Chest");
            AddMapEntry(new Color(0, 0, 255), name);
            dustType = 206;
            disableSmartCursor = true;
            adjTiles = new int[] { TileID.Containers };
            chest = "Spirit Chest";
        }

        public string MapChestName(string name, int i, int j) {
            int left = i;
            int top = j;
            Tile tile = Main.tile[i, j];
            if(tile.frameX % 36 != 0) {
                left--;
            }
            if(tile.frameY != 0) {
                top--;
            }
            int chest = Chest.FindChest(left, top);
            if(Main.chest[chest].name == "") {
                return name;
            } else {
                return name + ": " + Main.chest[chest].name;
            }
        }

        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = 1;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged) {
            Tile tile = Main.tile[i, j];
            int left = i;
            int top = j;
            if(tile.frameX % 36 != 0) {
                left--;
            }
            if(tile.frameY != 0) {
                top--;
            }
            return Chest.CanDestroyChest(left, top);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Item.NewItem(i * 16, j * 16, 32, 32, ModContent.ItemType<SpiritChest2>());
            Chest.DestroyChest(i, j);
        }

        public override void RightClick(int i, int j) {
            Player player = Main.player[Main.myPlayer];
            for(int num66 = 0; num66 < 58; num66++) {
                if(player.inventory[num66].type == ModContent.ItemType<SpiritKey>() && player.inventory[num66].stack > 0) {
                    /* player.inventory[num66].stack--; */
                    Chest.Unlock(i, j);
                    Chest.Unlock(i - 1, j - 1);
                    Chest.Unlock(i, j - 1);
                    Chest.Unlock(i - 1, j);
                    /*     if (player.inventory[num66].stack <= 0)
						 {
							 player.inventory[num66] = new Item();
						 } */

                }
            }

            Tile tile = Main.tile[i, j];
            if(tile.frameX != 72 && tile.frameX != 90) {
                Main.mouseRightRelease = false;
                int left = i;
                int top = j;
                if(tile.frameX % 36 != 0) {
                    left--;
                }
                if(tile.frameY != 0) {
                    top--;
                }
                if(player.sign >= 0) {
                    Main.PlaySound(11, -1, -1, 1);
                    player.sign = -1;
                    Main.editSign = false;
                    Main.npcChatText = "";
                }
                if(Main.editChest) {
                    Main.PlaySound(SoundID.MenuTick, -1, -1, 1);
                    Main.editChest = false;
                    Main.npcChatText = "";
                }
                if(player.editedChestName) {
                    NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f, 0f, 0f, 0, 0, 0);
                    player.editedChestName = false;
                }
                if(Main.netMode == 1) {
                    if(left == player.chestX && top == player.chestY && player.chest >= 0) {
                        player.chest = -1;
                        Recipe.FindRecipes();
                        Main.PlaySound(SoundID.MenuClose, -1, -1, 1);
                    } else {
                        NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, top, 0f, 0f, 0, 0, 0);
                        Main.stackSplit = 600;
                    }
                } else {
                    int chest = Chest.FindChest(left, top);
                    if(chest >= 0) {
                        Main.stackSplit = 600;
                        if(chest == player.chest) {
                            player.chest = -1;
                            Main.PlaySound(SoundID.MenuClose, -1, -1, 1);
                        } else {
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

        public override void MouseOver(int i, int j) {
            Player player = Main.player[Main.myPlayer];
            Tile tile = Main.tile[i, j];
            int left = i;
            int top = j;
            if(tile.frameX % 36 != 0) {
                left--;
            }
            if(tile.frameY != 0) {
                top--;
            }
            int chest = Chest.FindChest(left, top);
            player.showItemIcon2 = -1;
            if(chest < 0) {
                player.showItemIconText = Lang.chestType[0].Value;
            } else {
                player.showItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : "Spirit Chest";
                if(player.showItemIconText == "Spirit Chest") {
                    if(tile.frameX == 72 || tile.frameX == 90) {
                        player.showItemIcon2 = ModContent.ItemType<SpiritKey>();
                        player.showItemIconText = "";
                    }
                    //else
                    //{
                    //player.showItemIcon2 = ModContent.ItemType<CrystalChest>();
                    //}
                }
            }
            player.noThrow = 2;
            player.showItemIcon = true;
        }

        public override void MouseOverFar(int i, int j) {
            MouseOver(i, j);
            Player player = Main.player[Main.myPlayer];
            if(player.showItemIconText == "") {
                player.showItemIcon = false;
                player.showItemIcon2 = 0;
            }
        }
    }
}