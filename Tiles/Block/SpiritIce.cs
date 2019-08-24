using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace SpiritMod.Tiles.Block
{
	public class SpiritIce : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(70, 130, 180));
			drop = mod.ItemType("SpiritIceItem");
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			{
				r = 0.08f;
				g = 0.12f;
				b = 0.28f;
			}
		}

		public override bool CanExplode(int i, int j)
		{
			return true;
		}
	}
}

