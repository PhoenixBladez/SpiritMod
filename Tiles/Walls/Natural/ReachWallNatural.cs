using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Walls.Natural
{
	public class ReachWallNatural : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			WallID.Sets.Conversion.Grass[Type] = true;
			//drop = 747;
			AddMapEntry(new Color(58, 60, 60));
		}
	}
}