using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
namespace SpiritMod.Tiles.Block
{
    public class BigAsteroid : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[this.Type] = true;
            Main.tileMergeDirt[Type] = true;
            AddMapEntry(new Color(200, 200, 200));
            Main.tileBlockLight[Type] = true;
            soundType = 21;
            minPick = 100;
            drop = mod.ItemType("AsteroidBlock");
        }
        public override void RandomUpdate(int i, int j)
        {
            if (!Framing.GetTileSafely(i, j - 1).active() && Main.rand.Next(4) == 0)
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        ReachGrassTile.PlaceObject(i, j - 1, mod.TileType("GlowShard1"));
                        NetMessage.SendObjectPlacment(-1, i, j - 1, mod.TileType("GlowShard1"), 0, 0, -1, -1);
                        break;
                    case 1:
                        ReachGrassTile.PlaceObject(i, j - 1, mod.TileType("GlowShard2"));
                        NetMessage.SendObjectPlacment(-1, i, j - 1, mod.TileType("GlowShard2"), 0, 0, -1, -1);
                        break;
                    case 2:
                        ReachGrassTile.PlaceObject(i, j - 1, mod.TileType("GlowShard3"));
                        NetMessage.SendObjectPlacment(-1, i, j - 1, mod.TileType("GlowShard3"), 0, 0, -1, -1);
                        break;
                    case 3:
                        ReachGrassTile.PlaceObject(i, j - 1, mod.TileType("GlowShard4"));
                        NetMessage.SendObjectPlacment(-1, i, j - 1, mod.TileType("GlowShard4"), 0, 0, -1, -1);
                        break;
                }
            }
            if (!Framing.GetTileSafely(i, j + 1).active() && Main.rand.Next(4) == 0)
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        ReachGrassTile.PlaceObject(i, j + 1, mod.TileType("GlowShard1"));
                        NetMessage.SendObjectPlacment(-1, i, j + 1, mod.TileType("GlowShard1"), 0, 0, -1, -1);
                        break;
                    case 1:
                        ReachGrassTile.PlaceObject(i, j + 1, mod.TileType("GlowShard2"));
                        NetMessage.SendObjectPlacment(-1, i, j + 1, mod.TileType("GlowShard2"), 0, 0, -1, -1);
                        break;
                    case 2:
                        ReachGrassTile.PlaceObject(i, j + 1, mod.TileType("GlowShard3"));
                        NetMessage.SendObjectPlacment(-1, i, j + 1, mod.TileType("GlowShard3"), 0, 0, -1, -1);
                        break;
                    case 3:
                        ReachGrassTile.PlaceObject(i, j + 1, mod.TileType("GlowShard4"));
                        NetMessage.SendObjectPlacment(-1, i, j + 1, mod.TileType("GlowShard4"), 0, 0, -1, -1);
                        break;
                }
            }
            if (!Framing.GetTileSafely(i - 1, j).active() && Main.rand.Next(4) == 0)
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        ReachGrassTile.PlaceObject(i - 1, j, mod.TileType("GlowShard1"));
                        NetMessage.SendObjectPlacment(-1, i - 1, j, mod.TileType("GlowShard1"), 0, 0, -1, -1);
                        break;
                    case 1:
                        ReachGrassTile.PlaceObject(i - 1, j, mod.TileType("GlowShard2"));
                        NetMessage.SendObjectPlacment(-1, i - 1, j, mod.TileType("GlowShard2"), 0, 0, -1, -1);
                        break;
                    case 2:
                        ReachGrassTile.PlaceObject(i - 1, j, mod.TileType("GlowShard3"));
                        NetMessage.SendObjectPlacment(-1, i - 1, j, mod.TileType("GlowShard3"), 0, 0, -1, -1);
                        break;
                    case 3:
                        ReachGrassTile.PlaceObject(i - 1, j, mod.TileType("GlowShard4"));
                        NetMessage.SendObjectPlacment(-1, i - 1, j, mod.TileType("GlowShard4"), 0, 0, -1, -1);
                        break;
                }
            }
            if (!Framing.GetTileSafely(i + 1, j).active() && Main.rand.Next(4) == 0)
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        ReachGrassTile.PlaceObject(i + 1, j, mod.TileType("GlowShard1"));
                        NetMessage.SendObjectPlacment(-1, i + 1, j, mod.TileType("GlowShard1"), 0, 0, -1, -1);
                        break;
                    case 1:
                        ReachGrassTile.PlaceObject(i + 1, j, mod.TileType("GlowShard2"));
                        NetMessage.SendObjectPlacment(-1, i + 1, j, mod.TileType("GlowShard2"), 0, 0, -1, -1);
                        break;
                    case 2:
                        ReachGrassTile.PlaceObject(i + 1, j, mod.TileType("GlowShard3"));
                        NetMessage.SendObjectPlacment(-1, i + 1, j, mod.TileType("GlowShard3"), 0, 0, -1, -1);
                        break;
                    case 3:
                        ReachGrassTile.PlaceObject(i + 1, j, mod.TileType("GlowShard4"));
                        NetMessage.SendObjectPlacment(-1, i + 1, j, mod.TileType("GlowShard4"), 0, 0, -1, -1);
                        break;
                }
            }
        }
    }
}