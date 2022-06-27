using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Walls.Natural
{
	public class SpiritWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			WallID.Sets.Conversion.Grass[Type] = true;
			ItemDrop = ModContent.ItemType<SpiritWallItem>();
			AddMapEntry(new Color(70, 70, 70));
		}
	}
}