using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace SpiritMod.Tiles.Block
{
	public class VeridianDirt : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileMerge[Type][mod.TileType("VeridianStone")] = true;
			Main.tileMergeDirt[Type] = true;
			AddMapEntry(new Color(0, 191, 255));
			drop = mod.ItemType("SpiritDirtItem");
		}



		public override bool CanExplode(int i, int j)
		{
			return true;
		}
	}
}

