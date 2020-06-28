using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Walls;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Walls.Natural
{
	public class ScrapWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			AddMapEntry(new Color(100, 100, 100));
			drop = ModContent.ItemType<ScrapWallItem>();
		}

		/*	public override void NumDust(int i, int j, bool fail, ref int num)
            {
                num = fail ? 1 : 3;
            }

            public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
            {
                r = 0f;
                g = 0f;
                b = 2.5f; 
            } */
	}
}