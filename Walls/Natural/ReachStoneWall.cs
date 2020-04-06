using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Walls.Natural
{
	public class ReachStoneWall : ModWall
	{
		public override void SetDefaults()
		{
			AddMapEntry(new Color(70, 70, 70));
		}
	}
}