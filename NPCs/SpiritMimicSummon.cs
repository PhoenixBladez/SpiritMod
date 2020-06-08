using Terraria;
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
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
    public class ChestSummon : ModPlayer
    {
        public int LastChest = 0;

        public override void PreUpdateBuffs() {
            if(Main.netMode != 1) {
                if(player.chest == -1 && LastChest >= 0 && Main.chest[LastChest] != null) {
                    int x2 = Main.chest[LastChest].x;
                    int y2 = Main.chest[LastChest].y;
                    ChestItemSummonCheck(x2, y2, mod);
                }
                LastChest = player.chest;
            }
        }

        public static bool ChestItemSummonCheck(int x, int y, Mod mod) {
            if(Main.netMode == 1) {
                return false;
            }
            int num = Chest.FindChest(x, y);
            if(num < 0) {
                return false;
            }
            int numberExampleBlocks = 0;
            int numberOtherItems = 0;
            ushort tileType = Main.tile[Main.chest[num].x, Main.chest[num].y].type;
            int tileStyle = (int)(Main.tile[Main.chest[num].x, Main.chest[num].y].frameX / 36);
            if(tileType == TileID.Containers && (tileStyle < 5 || tileStyle > 6)) {
                for(int i = 0; i < 40; i++) {
                    if(Main.chest[num].item[i] != null && Main.chest[num].item[i].type > 0) {
                        if(Main.chest[num].item[i].type == ModContent.ItemType<SpiritKey>()) {
                            numberExampleBlocks += Main.chest[num].item[i].stack;
                        } else {
                            numberOtherItems++;
                        }
                    }
                }
            }
            if(numberOtherItems == 0 && numberExampleBlocks == 1) {
                if(Main.tile[x, y].type == 21) {
                    if(Main.tile[x, y].frameX % 36 != 0) {
                        x--;
                    }
                    if(Main.tile[x, y].frameY % 36 != 0) {
                        y--;
                    }
                    int number = Chest.FindChest(x, y);
                    for(int j = x; j <= x + 1; j++) {
                        for(int k = y; k <= y + 1; k++) {
                            if(Main.tile[j, k].type == 21) {
                                Main.tile[j, k].active(false);
                            }
                        }
                    }
                    for(int l = 0; l < 40; l++) {
                        Main.chest[num].item[l] = new Item();
                    }
                    Chest.DestroyChest(x, y);
                    NetMessage.SendData(34, -1, -1, null, 1, (float)x, (float)y, 0f, number, 0, 0);
                    NetMessage.SendTileSquare(-1, x, y, 3);
                }
                int npcToSpawn = ModContent.NPCType<SpiritMimic>();
                int npcIndex = NPC.NewNPC(x * 16 + 16, y * 16 + 32, npcToSpawn, 0, 0f, 0f, 0f, 0f, 255);
                Main.npc[npcIndex].whoAmI = npcIndex;
                NetMessage.SendData(23, -1, -1, null, npcIndex, 0f, 0f, 0f, 0, 0, 0);
                Main.npc[npcIndex].BigMimicSpawnSmoke();
            }

            return false;
        }
    }
}