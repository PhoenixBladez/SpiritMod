using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Walls.Natural
{
	public class AcidBrickWallTile : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			drop = ModContent.ItemType<Items.Placeable.Walls.AcidWall>();
			AddMapEntry(new Color(77, 84, 79));
		}
	}
}