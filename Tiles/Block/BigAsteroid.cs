using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
namespace SpiritMod.Tiles.Block
{
	public class BigAsteroid : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
            Main.tileBlendAll[this.Type] = true;
            Main.tileMergeDirt[Type] = true;
			AddMapEntry(new Color(200, 200, 200));
            Main.tileBlockLight[Type] = true;
            soundType = 21;
        }
	}
}