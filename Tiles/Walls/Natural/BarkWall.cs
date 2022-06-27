using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Walls.Natural
{
	public class BarkWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			ItemDrop = ModContent.ItemType<Items.Placeable.Walls.BarkWall>();
			AddMapEntry(new Color(92, 77, 61));
		}
	}
}