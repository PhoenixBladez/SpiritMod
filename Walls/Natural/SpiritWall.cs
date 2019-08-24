using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Walls.Natural
{
	public class SpiritWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			drop = mod.ItemType("SpiritWallItem");
			AddMapEntry(new Color(110, 110, 110));
		}
	}
}