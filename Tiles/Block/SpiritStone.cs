using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.World.Generation;
using SpiritMod.Items.Placeable.Tiles;

namespace SpiritMod.Tiles.Block
{
	public class SpiritStone : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<SpiritDirt>()] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(70, 130, 180));
			soundType = 21;
			drop = ModContent.ItemType<SpiritStoneItem>();
		}

		public override bool CanExplode(int i, int j)
		{
			return true;
		}
	}
}

