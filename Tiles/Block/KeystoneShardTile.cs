using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class KeystoneShardTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = false;  //true for block to emit light
			Main.tileLighted[Type] = false;
			drop = mod.ItemType("KeystoneShard");   //put your CustomBlock name
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Mysterious Shard");
			AddMapEntry(new Color(100, 100, 110), name);
			soundType = 21;

		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)   //light colors
		{
			r = 0;
			g = 0.06f;
			b = 0;
		}
	}
}