using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Walls.Natural
{
	public class BismiteWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = false;
			AddMapEntry(new Color(60, 80, 60));
		}
	}
}