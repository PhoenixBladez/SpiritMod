using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles
{
    public class GTile : GlobalTile
    {
        int[] TileArray2 = { 0, 3, 185, 186, 187, 71, 28 };
        public int tremorItem = 0;
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Player player = Main.LocalPlayer;
            MyPlayer modPlayer = player.GetSpiritPlayer();
            if (type == 1 || type == 25 || type == 117 || type == 203 || type == 57)
            {
                if (Main.rand.Next(50) == 1 && modPlayer.gemPickaxe)
                {
                    tremorItem = Main.rand.Next(new int[] { 11, 12, 13, 14, 699, 700, 701, 702, 999, 182, 178, 179, 177, 180, 181 });
                    if (Main.hardMode)
                    {
                        tremorItem = Main.rand.Next(new int[] { 11, 12, 13, 14, 699, 700, 701, 702, 999, 182, 178, 179, 177, 180, 181, 364, 365, 366, 1104, 1105, 1106 });
                    }
                    Item.NewItem(i * 16, j * 16, 64, 48, tremorItem, Main.rand.Next(0, 2));
                }
            }
        }
        public override bool Drop(int i, int j, int type)
        {
            Player player = Main.LocalPlayer;
            MyPlayer modPlayer = player.GetSpiritPlayer();
            if (type == TileID.PalmTree && Main.rand.Next(3) == 0 && player.ZoneBeach)
            {
                if (NPC.CountNPCS(mod.NPCType("OceanSlime")) < 1)
                {
                    NPC.NewNPC(i * 16, (j - 10) * 16, mod.NPCType("OceanSlime"), 0, 0.0f, 0.0f, 0.0f, 0.0f, (int)byte.MaxValue);
                }
            }
            return base.Drop(i, j, type);
        }
    }
}