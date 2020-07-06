using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Walls.Natural
{
	public class ElderbarkFenceWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			drop = ModContent.ItemType<Items.Placeable.Walls.ElderbarkFence>();
			AddMapEntry(new Color(92, 77, 61));
		}
	}
}