using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class CryoliteOreTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMerge[Type][TileID.IceBlock] = true;
			Main.tileMerge[Type][TileID.SnowBlock] = true;
			Main.tileBlockLight[Type] = true;  //true for block to emit light
			Main.tileLighted[Type] = false;
			drop = ModContent.ItemType<CryoliteOre>();   //put your CustomBlock name
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Cryolite Ore");
			AddMapEntry(new Color(40, 0, 205), name);
			soundType = 21;
			minPick = 100;
			dustType = 68;

		}

	}
}