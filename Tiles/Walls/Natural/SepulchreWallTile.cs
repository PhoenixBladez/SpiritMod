using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Walls.Natural
{
	public class SepulchreWallTile : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			drop = ModContent.ItemType<SepulchreWallItem>();
			AddMapEntry(new Color(50, 50, 50));
		}
	}
}