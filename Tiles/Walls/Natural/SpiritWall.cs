using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Walls;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Walls.Natural
{
	public class SpiritWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			drop = ModContent.ItemType<SpiritWallItem>();
			AddMapEntry(new Color(110, 110, 110));
		}
	}
}