using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Walls.Natural
{
	public class ReachWallNatural : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			drop = 747;
			AddMapEntry(new Color(58, 60, 60));
		}
	}
}