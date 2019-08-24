/*using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace SpiritMod.Tiles.Ambient
{
    public class SpiritAltar : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.addTile(Type);

            this.AddMapEntry(Colors.RarityCyan, "Spirit Altar");
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.player[Main.myPlayer];
            player.noThrow = 2;
            player.showItemIcon = true;
            player.showItemIcon2 = mod.ItemType("SpiritAltar_TempItem");
        }

        public override void RightClick(int i, int j)
        {
            if (!NPC.AnyNPCs(mod.NPCType("Overseer")))
            {
                Vector2 checkPos = new Vector2(i * 16, j * 16);

                int target = -1;
                int distance = 160;
                for (int p = 0; p < 255; ++p)
                {
                    if (Main.player[p].active && !Main.player[p].dead && (checkPos - Main.player[p].Center).Length() < distance)
                    {
                        NPC.SpawnOnPlayer(p, mod.NPCType("Overseer"));
                        break;
                    }
                }
            }
        }
    }
}*/
