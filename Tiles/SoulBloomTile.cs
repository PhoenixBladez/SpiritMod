using Microsoft.Xna.Framework.Graphics;
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
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Tiles
{
    public class SoulBloomTile : ModTile
    {
        public override void SetDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileCut[Type] = true;
            //Main.tileAlch[Type] = true;
            Main.tileNoFail[Type] = true;
            //Main.tileLavaDeath[Type] = true;
            dustType = 187;
            //disableSmartCursor = true;
            //AddMapEntry(new Color(13, 88, 130), "Banner");
            //TileObjectData.newTile.Width = 1;
            //TileObjectData.newTile.Height = 1;
            //TileObjectData.newTile.Origin = Point16.Zero;
            //TileObjectData.newTile.UsesCustomCanPlace = true;
            //TileObjectData.newTile.CoordinateHeights = new int[]
            //{
            //	20
            //};
            //TileObjectData.newTile.CoordinateWidth = 16;
            //TileObjectData.newTile.CoordinatePadding = 2;
            //TileObjectData.newTile.DrawYOffset = -1;
            //TileObjectData.newTile.StyleHorizontal = true;
            //TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);
            //TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            //TileObjectData.newTile.LavaDeath = true;
            //TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
            //TileObjectData.addBaseTile(out TileObjectData.StyleAlch);
            TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
            TileObjectData.newTile.AnchorValidTiles = new int[]
            {
                TileType<Block.SpiritGrass>()
            };
            TileObjectData.newTile.AnchorAlternateTiles = new int[]
            {
                78, //ClayPot
				TileID.PlanterBox
            };
            TileObjectData.addTile(Type);
            //drop = mod.ItemType()
        }

        //public override bool CanPlace(int i, int j)
        //{
        //	return base.CanPlace(i, j);
        //}

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) {
            if(i % 2 == 1) {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }

        public override bool Drop(int i, int j) {
            int stage = Main.tile[i, j].frameX / 18;
            if(stage == 1) {
                Item.NewItem(i * 16, j * 16, 64, 32, ItemType<SoulBloom>());
            }
            if(stage == 2) {
                Item.NewItem(i * 16, j * 16, 64, 32, ItemType<SoulBloom>());
                Item.NewItem(i * 16, j * 16, 0, 0, ItemType<Items.Placeable.SoulSeeds>());
            }
            return false;
        }

        public override void RandomUpdate(int i, int j) {
            if(Main.tile[i, j].frameX == 0) {
                Main.tile[i, j].frameX += 18;
            } else if(Main.tile[i, j].frameX == 18) {
                Main.tile[i, j].frameX += 18;
            }
        }

        //public override void RightClick(int i, int j)
        //{
        //	base.RightClick(i, j);
        //}
    }
}
