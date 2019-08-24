using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class ThermiteOre : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;  //true for block to emit light
			Main.tileLighted[Type] = true;
			drop = mod.ItemType("ThermiteOre");   //put your CustomBlock name
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Thermite Ore");
			AddMapEntry(new Color(240, 20, 20), name);
			soundType = 21;
			minPick = 200;
			dustType = 6;

		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			{
				r = 0.4f;
				g = 0.17f;
				b = 0.17f;
			}
		}
	}
}