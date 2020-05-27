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
		
		static ushort IceType1 = (ushort)ModContent.TileType<Ambient.IceSculpture.Hostile.IceWheezerHostile>();
		static ushort IceType2 = (ushort)ModContent.TileType<Ambient.IceSculpture.IceWheezerPassive>();
		static ushort IceType3 = (ushort)ModContent.TileType<Ambient.IceSculpture.Hostile.IceVikingHostile>();
		static ushort IceType4 = (ushort)ModContent.TileType<Ambient.IceSculpture.IceVikingPassive>();
		static ushort IceType5 = (ushort)ModContent.TileType<Ambient.IceSculpture.Hostile.IceBatHostile>();
		static ushort IceType6 = (ushort)ModContent.TileType<Ambient.IceSculpture.IceBatPassive>();
		static ushort IceType7 = (ushort)ModContent.TileType<Ambient.IceSculpture.Hostile.IceFlinxHostile>();
		static ushort IceType8 = (ushort)ModContent.TileType<Ambient.IceSculpture.IceFlinxPassive>();
		public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);
			ushort eggType = (ushort)ModContent.TileType<Ambient.AvianEgg>();
			if (type != eggType && tileAbove.type == eggType)
			{
				return false;
			}
			else if (type != IceType1 && tileAbove.type == IceType1)
			{
				return false;
			}
			else if (type != IceType2 && tileAbove.type == IceType2)
			{
				return false;
			}
			else if (type != IceType3 && tileAbove.type == IceType3)
			{
				return false;
			}
			else if (type != IceType4 && tileAbove.type == IceType4)
			{
				return false;
			}
			else if (type != IceType5 && tileAbove.type == IceType5)
			{
				return false;
			}
			else if (type != IceType6 && tileAbove.type == IceType6)
			{
				return false;
			}
			else if (type != IceType7 && tileAbove.type == IceType7)
			{
				return false;
			}
			else if (type != IceType8 && tileAbove.type == IceType8)
			{
				return false;
			}
            return base.CanKillTile(i, j, type, ref blockDamaged);
		}
		public override bool CanExplode(int i, int j, int type)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);
			ushort eggType = (ushort)ModContent.TileType<Ambient.AvianEgg>();
			if (type == eggType || tileAbove.type == eggType)
			{
				return false;
			}
			else if (type != IceType1 && tileAbove.type == IceType1)
			{
				return false;
			}
			else if (type != IceType2 && tileAbove.type == IceType2)
			{
				return false;
			}
			else if (type != IceType3 && tileAbove.type == IceType3)
			{
				return false;
			}
			else if (type != IceType4 && tileAbove.type == IceType4)
			{
				return false;
			}
			else if (type != IceType5 && tileAbove.type == IceType5)
			{
				return false;
			}
			else if (type != IceType6 && tileAbove.type == IceType6)
			{
				return false;
			}
			else if (type != IceType7 && tileAbove.type == IceType7)
			{
				return false;
			}
			else if (type != IceType8 && tileAbove.type == IceType8)
			{
				return false;
			}
			 return base.CanExplode(i, j, type);
		}
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
            if (player.HasItem(mod.ItemType("Spineshot")))
            {
                if (type == 3 || type == 24 || type == 61 || type == 71 || type == 110 || type == 201)
                {
                    Item.NewItem(i * 16, j * 16, 64, 48, ItemID.Seed, Main.rand.Next(1, 3));
                }
            }
        }
        public override void FloorVisuals(int type, Player player)
        {
            if (type == TileID.Sand && player.GetSpiritPlayer().tumbleSoul)
            {
                player.moveSpeed += .16f;
                player.maxRunSpeed += .1f;
                if (player.velocity.X != 0f)
                {
                    int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 0, 0);
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].noGravity = true;
                }
            }
            if ((type == 2 || type == 60 || type == 70 || type == 109 || type == 199 || type == 23) && player.GetSpiritPlayer().floranCharm)
            {
                player.lifeRegen += 3;
                player.meleeSpeed += .5f;
            }
        }
        public override bool Drop(int i, int j, int type)
        {
            Player player = Main.LocalPlayer;
            MyPlayer modPlayer = player.GetSpiritPlayer();
            if (type == TileID.PalmTree && Main.rand.Next(3) == 0 && player.ZoneBeach)
            {
                if (Main.rand.Next(2) == 1)
                {
                    Item.NewItem(i * 16, j * 16, 64, 48, mod.ItemType("Coconut"), Main.rand.Next(5, 8));
                }
                if (NPC.CountNPCS(mod.NPCType("OceanSlime")) < 1)
                {
                    NPC.NewNPC(i * 16, (j - 10) * 16, mod.NPCType("OceanSlime"), 0, 0.0f, 0.0f, 0.0f, 0.0f, (int)byte.MaxValue);
                }
            }
            if (type == 72)
            {
                Item.NewItem(i * 16, j * 16, 64, 48, mod.ItemType("GlowRoot"), Main.rand.Next(0, 2));
            }
            return base.Drop(i, j, type);
        }
    }
}