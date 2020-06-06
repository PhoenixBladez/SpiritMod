using System;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
namespace SpiritMod.Tiles.Furniture.SpaceJunk
{
    public class ScrapItem1 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Salvaged Scrap");
        }
        public override void SetDefaults()
        {
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
        public override void SetDefaults()
        {
            Main.tileTable[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.addTile(Type);
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            offsetY = 2;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
            Terraria.Item.NewItem(i * 16, j * 16, 64, 32, mod.ItemType("ScrapItem1"));
        }
    }
    public class ScrapItem2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Salvaged Scrap");
        }
        public override void SetDefaults()
        {
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
        public override void SetDefaults()
        {
            Main.tileTable[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.addTile(Type);
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            offsetY = 2;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
            Terraria.Item.NewItem(i * 16, j * 16, 64, 32, mod.ItemType("ScrapItem2"));
        }
    }

    public class ScrapItem3 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Salvaged Scrap");
        }
        public override void SetDefaults()
        {
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
        public override void SetDefaults()
        {
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

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            offsetY = 2;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
            Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType("ScrapItem3"));
        }
    }
    public class ScrapItem4 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Salvaged Scrap");
        }
        public override void SetDefaults()
        {
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
        public override void SetDefaults()
        {
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

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            offsetY = 2;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
            Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType("ScrapItem4"));
        }
    }
    public class ScrapItem5 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Salvaged Scrap");
        }
        public override void SetDefaults()
        {
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
        public override void SetDefaults()
        {
            Main.tileTable[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.addTile(Type);
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            offsetY = 2;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
            Terraria.Item.NewItem(i * 16, j * 16, 64, 32, mod.ItemType("ScrapItem5"));
        }
    }
    public class ScrapItem6 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Salvaged Scrap");
        }
        public override void SetDefaults()
        {
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
        public override void SetDefaults()
        {
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
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            offsetY = 2;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
            Terraria.Item.NewItem(i * 16, j * 16, 64, 32, mod.ItemType("ScrapItem6"));
        }
    }
}