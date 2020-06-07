using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.World.Generation;

namespace SpiritMod.Tiles.Block
{
	public class SpiritDirt : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<SpiritGrass>()] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(173, 216, 230));
			drop = ModContent.ItemType<SpiritDirtItem>();
			dustType = 103;
		}

		public override bool CanExplode(int i, int j)
		{
			return true;
		}
	}
}

