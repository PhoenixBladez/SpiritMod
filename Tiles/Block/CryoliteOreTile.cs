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
			Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<CryoliteOre>();   //put your CustomBlock name
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Cryolite Ore");
			AddMapEntry(new Color(40, 0, 205), name);
			soundType = SoundID.Tink;
			minPick = 100;
			dustType = 68;

		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = .064f * 1.5f;
			g = .112f * 1.5f;
			b = .128f * 1.5f;
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
}