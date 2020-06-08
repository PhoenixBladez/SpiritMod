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
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
namespace SpiritMod.Tiles.Furniture.SpaceJunk
{
    public class ScrapItem1 : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Salvaged Scrap");
        }
        public override void SetDefaults() {
            item.width = 22;
            item.height = 22;
            item.value = Item.buyPrice(0, 0, 30, 0);

            item.maxStack = 999;

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 15;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = mod.TileType("ScrapTile1");
        }
    }
    public class ScrapTile1 : ModTile
    {
        public override void SetDefaults() {
            Main.tileTable[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.addTile(Type);
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height) {
            offsetY = 2;
        }
        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
            Terraria.Item.NewItem(i * 16, j * 16, 64, 32, mod.ItemType("ScrapItem1"));
        }
    }
    public class ScrapItem2 : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Salvaged Scrap");
        }
        public override void SetDefaults() {
            item.width = 22;
            item.height = 22;
            item.value = Item.buyPrice(0, 0, 30, 0);

            item.maxStack = 999;

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 15;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = mod.TileType("ScrapTile2");
        }
    }
    public class ScrapTile2 : ModTile
    {
        public override void SetDefaults() {
            Main.tileTable[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.addTile(Type);
        }

        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height) {
            offsetY = 2;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
            Terraria.Item.NewItem(i * 16, j * 16, 64, 32, mod.ItemType("ScrapItem2"));
        }
    }

    public class ScrapItem3 : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Salvaged Scrap");
        }
        public override void SetDefaults() {
            item.width = 22;
            item.height = 22;
            item.value = Item.buyPrice(0, 0, 30, 0);

            item.maxStack = 999;

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 15;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = mod.TileType("ScrapTile3");
        }
    }
    public class ScrapTile3 : ModTile
    {
        public override void SetDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16
            };
            TileObjectData.newTile.AnchorBottom = default(AnchorData);
            TileObjectData.newTile.AnchorTop = default(AnchorData);
            TileObjectData.newTile.AnchorWall = true;
            TileObjectData.addTile(Type);
        }

        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height) {
            offsetY = 2;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
            Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType("ScrapItem3"));
        }
    }
    public class ScrapItem4 : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Salvaged Scrap");
        }
        public override void SetDefaults() {
            item.width = 22;
            item.height = 22;
            item.value = Item.buyPrice(0, 0, 30, 0);

            item.maxStack = 999;

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 15;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = mod.TileType("ScrapTile4");
        }
    }
    public class ScrapTile4 : ModTile
    {
        public override void SetDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16
            };
            TileObjectData.newTile.AnchorBottom = default(AnchorData);
            TileObjectData.newTile.AnchorTop = default(AnchorData);
            TileObjectData.newTile.AnchorWall = true;
            TileObjectData.addTile(Type);
        }

        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height) {
            offsetY = 2;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
            Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType("ScrapItem4"));
        }
    }
    public class ScrapItem5 : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Salvaged Scrap");
        }
        public override void SetDefaults() {
            item.width = 22;
            item.height = 22;
            item.value = Item.buyPrice(0, 0, 30, 0);

            item.maxStack = 999;

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 15;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = mod.TileType("ScrapTile5");
        }
    }
    public class ScrapTile5 : ModTile
    {
        public override void SetDefaults() {
            Main.tileTable[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.addTile(Type);
        }

        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height) {
            offsetY = 2;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
            Terraria.Item.NewItem(i * 16, j * 16, 64, 32, mod.ItemType("ScrapItem5"));
        }
    }
    public class ScrapItem6 : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Salvaged Scrap");
        }
        public override void SetDefaults() {
            item.width = 22;
            item.height = 22;
            item.value = Item.buyPrice(0, 0, 30, 0);

            item.maxStack = 999;

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 15;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = mod.TileType("ScrapTile6");
        }
    }
    public class ScrapTile6 : ModTile
    {
        public override void SetDefaults() {
            Main.tileTable[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16
            };
            TileObjectData.addTile(Type);
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height) {
            offsetY = 2;
        }
        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
            Terraria.Item.NewItem(i * 16, j * 16, 64, 32, mod.ItemType("ScrapItem6"));
        }
    }
}