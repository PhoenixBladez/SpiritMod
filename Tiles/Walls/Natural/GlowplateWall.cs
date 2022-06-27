using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Walls;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Walls.Natural
{
	public class GlowplateWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			AddMapEntry(new Color(40, 40, 40));
			ItemDrop = ModContent.ItemType<GlowplateWallItem>();
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