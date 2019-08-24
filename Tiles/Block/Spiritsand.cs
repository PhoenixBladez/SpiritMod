using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.World.Generation;

namespace SpiritMod.Tiles.Block
{
	public class Spiritsand : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			SetModCactus(new SpiritCactus());
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(135, 206, 235));
			drop = mod.ItemType("SpiritSandItem");
		}

		public override bool CanExplode(int i, int j)
		{
			return true;
		}
	}
}

