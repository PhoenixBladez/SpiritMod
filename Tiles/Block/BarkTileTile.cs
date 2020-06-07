using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.World.Generation;
using SpiritMod.Items.Material;

namespace SpiritMod.Tiles.Block
{
	public class BarkTileTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(120, 60, 60));
			drop = ModContent.ItemType<AncientBark>();
		}

		public override bool CanExplode(int i, int j)
		{
			return true;
		}
	}
}

