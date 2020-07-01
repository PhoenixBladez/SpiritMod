using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace SpiritMod.Tiles.Furniture
{
	public class CryoliteBar : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.addTile(Type);
			drop = ModContent.ItemType<Items.Material.CryoliteBar>();
			adjTiles = new int[] { TileID.MetalBars };
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Bar");
            AddMapEntry(new Color(200, 200, 200), name);
        }

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
}