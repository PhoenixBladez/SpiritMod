using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.World.Generation;
using SpiritMod;

namespace SpiritMod.Tiles.Block
{
    public class MossyStone : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlendAll[this.Type] = true;
            soundType = 21;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(130, 130, 130));
            drop = ItemID.StoneBlock;
            dustType = 1;
        }
	}
}

