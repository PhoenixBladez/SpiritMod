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
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
    public class HalloweenGrass : ModTile
    {
        public static int _type;

        public override void SetDefaults() {
            Main.tileSolid[Type] = true;
            SetModTree(new SpookyTree());
            Main.tileMerge[Type][ModContent.TileType<HalloweenGrass>()] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            //Main.tileLighted[Type] = true;
            AddMapEntry(new Color(0, 255, 167));
            drop = ModContent.ItemType<Items.Placeable.Tiles.HalloweenGrass>();
        }

        public static bool PlaceObject(int x, int y, int type, bool mute = false, int style = 0, int alternate = 0, int random = -1, int direction = -1) {
            TileObject toBePlaced;
            if(!TileObject.CanPlace(x, y, type, style, direction, out toBePlaced, false)) {
                return false;
            }
            toBePlaced.random = random;
            if(TileObject.Place(toBePlaced) && !mute) {
                WorldGen.SquareTileFrame(x, y, true);
                //   Main.PlaySound(0, x * 16, y * 16, 1, 1f, 0f);
            }
            return false;
        }

        public override void RandomUpdate(int i, int j) {
            if(!Framing.GetTileSafely(i, j - 1).active() && Main.rand.Next(20) == 0) {
                int style = Main.rand.Next(23);
                if(PlaceObject(i, j - 1, Ambient.SpookyFoliage._type, false, style))
                    NetMessage.SendObjectPlacment(-1, i, j - 1, Ambient.SpookyFoliage._type, style, 0, -1, -1);
            }
            //else if (Main.rand.Next(100) == 0)
            //{
            //	if (Framing.GetTileSafely(i-1, j).type == _type &&
            //		!Framing.GetTileSafely(i-1, j-1).active()&&
            //		!Framing.GetTileSafely(i, j-1).active()&&
            //		!Framing.GetTileSafely(i-1, j-2).active()&&
            //		!Framing.GetTileSafely(i, j-2).active())
            //	{

            //	}
            //}
        }

        public override int SaplingGrowthType(ref int style) {
            style = 0;
            return ModContent.TileType<SpookySapling>();
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
            r = 0.028f;
            g = 0.153f;
            b = 0.081f;
        }
    }
}

