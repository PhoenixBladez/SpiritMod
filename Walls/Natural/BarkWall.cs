using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Walls.Natural
{
	public class BarkWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			drop = mod.ItemType("BarkWall");
			AddMapEntry(new Color(165, 60, 60));
		}
	}
}