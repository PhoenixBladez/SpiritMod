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
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
namespace SpiritMod.Tiles.Piles
{
    public class CopperPile : ModTile
    {
        public override void SetDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Copper Pile");
            AddMapEntry(new Color(70, 70, 70), name);
            dustType = DustID.CopperCoin;
            disableSmartCursor = true;
        }
        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Item.NewItem(i * 16, j * 16, 36, 18, ItemID.CopperOre, Main.rand.Next(5, 6) + 1);
            Item.NewItem(i * 16, j * 10, 18, 36, ItemID.CopperOre);
            Item.NewItem(i * 10, j * 16, 36, 36, ItemID.CopperOre);
        }
    }
    public class TinPile : ModTile
    {
        public override void SetDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 1;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Tin Pile");
            AddMapEntry(new Color(70, 70, 70), name);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            dustType = DustID.SilverCoin;
        }
        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Item.NewItem(i * 16, j * 16, 18, 36, ItemID.TinOre, Main.rand.Next(5, 6) + 1);
            Item.NewItem(i * 16, j * 10, 36, 18, ItemID.TinOre);
            Item.NewItem(i * 16, j * 16, 36, 36, ItemID.TinOre);
        }
    }
    public class IronPile : ModTile
    {
        public override void SetDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 2;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Iron Pile");
            AddMapEntry(new Color(70, 70, 70), name);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            dustType = DustID.SilverCoin;
        }
        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Item.NewItem(i * 16, j * 16, 18, 36, ItemID.IronOre, Main.rand.Next(5, 7) + 3);
            Item.NewItem(i * 16, j * 10, 36, 18, ItemID.IronOre);
            Item.NewItem(i * 16, j * 16, 36, 36, ItemID.IronOre);
        }
    }
    public class LeadPile : ModTile
    {
        public override void SetDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 2;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Lead Pile");
            AddMapEntry(new Color(70, 70, 70), name);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            dustType = DustID.SilverCoin;
        }
        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Item.NewItem(i * 16, j * 16, 18, 36, ItemID.LeadOre, Main.rand.Next(5, 7) + 3);
            Item.NewItem(i * 16, j * 10, 36, 18, ItemID.LeadOre);
            Item.NewItem(i * 16, j * 16, 36, 36, ItemID.LeadOre);
        }
    }
    public class SilverPile : ModTile
    {
        public override void SetDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 2;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Silver Pile");
            AddMapEntry(new Color(70, 70, 70), name);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            dustType = DustID.SilverCoin;
        }
        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Item.NewItem(i * 16, j * 16, 18, 36, ItemID.SilverOre, Main.rand.Next(6, 8) + 4);
            Item.NewItem(i * 16, j * 10, 36, 18, ItemID.SilverOre);
            Item.NewItem(i * 16, j * 16, 36, 36, ItemID.SilverOre);
        }
    }
    public class TungstenPile : ModTile
    {
        public override void SetDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 2;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Tungsten Pile");
            AddMapEntry(new Color(70, 70, 70), name);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            dustType = DustID.SilverCoin;
        }
        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Item.NewItem(i * 16, j * 16, 18, 36, ItemID.TungstenOre, Main.rand.Next(6, 8) + 4);
            Item.NewItem(i * 16, j * 10, 36, 18, ItemID.TungstenOre);
            Item.NewItem(i * 16, j * 16, 36, 36, ItemID.TungstenOre);
        }
    }
    public class GoldPile : ModTile
    {
        public override void SetDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 2;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Gold Pile");
            AddMapEntry(new Color(70, 70, 70), name);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            dustType = DustID.GoldCoin;
        }
        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Item.NewItem(i * 16, j * 16, 18, 36, ItemID.GoldOre, Main.rand.Next(10, 12) + 4);
            Item.NewItem(i * 16, j * 10, 36, 18, ItemID.GoldOre);
            Item.NewItem(i * 16, j * 16, 36, 36, ItemID.GoldOre);
        }
    }
    public class PlatinumPile : ModTile
    {
        public override void SetDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 2;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Platinum Pile");
            AddMapEntry(new Color(70, 70, 70), name);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            dustType = DustID.SilverCoin;
        }
        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Item.NewItem(i * 16, j * 16, 18, 36, ItemID.PlatinumOre, Main.rand.Next(10, 12) + 4);
            Item.NewItem(i * 16, j * 10, 36, 18, ItemID.PlatinumOre);
            Item.NewItem(i * 16, j * 16, 36, 36, ItemID.PlatinumOre);
        }
    }
}