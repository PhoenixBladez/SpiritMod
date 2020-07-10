using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class GraniteOre : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileBlockLight[Type] = true;  //true for block to emit light
			Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<GraniteChunk>();   //put your CustomBlock name
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Enchanted Granite Chunk");
			AddMapEntry(new Color(30, 144, 255), name);
			soundType = SoundID.Tink;
			minPick = 65;

		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.155f/2.6f;
			g = 0.215f/2.6f;
			b = .4375f/2.6f;
		}
    }
}