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
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Tiles.Block
{
    public class Asteroid : ModTile
    {
        public override void SetDefaults() {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            AddMapEntry(new Color(99, 79, 49));
            soundType = 21;
            Main.tileBlockLight[Type] = true;
            minPick = 100;
            drop = ModContent.ItemType<AsteroidBlock>();
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged) {
            Player player = Main.LocalPlayer;
            if(player.inventory[player.selectedItem].type == ItemID.ReaverShark) {
                return false;
            }
            return true;
        }
        //UNCOMMENT THIS IF YOU WANT CRYSTALS TO GROW ON REGULAR ASTEROIDS
        /*    public override void RandomUpdate(int i, int j)
            {
                if (!Framing.GetTileSafely(i, j - 1).active() && Main.rand.Next(50) == 0)
                {
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            ReachGrassTile.PlaceObject(i, j - 1, mod.TileType("GlowShard1"));
                            NetMessage.SendObjectPlacment(-1, i, j - 1, mod.TileType("GlowShard1"), 0, 0, -1, -1);
                            break;
                        case 1:
                            ReachGrassTile.PlaceObject(i, j - 1, ModContent.TileType<GreenShard>());
                            NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<GreenShard>(), 0, 0, -1, -1);
                            break;
                        case 2:
                            ReachGrassTile.PlaceObject(i, j - 1, ModContent.TileType<PurpleShard>());
                            NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<PurpleShard>(), 0, 0, -1, -1);
                            break;
                    }
                }
            }*/
    }
}