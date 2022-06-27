using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Walls.Natural
{
	public class SepulchreWallTile : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			ItemDrop = ModContent.ItemType<Items.Placeable.Walls.SepulchreWallItem>();
			AddMapEntry(new Color(50, 50, 50));
		}
	}
}