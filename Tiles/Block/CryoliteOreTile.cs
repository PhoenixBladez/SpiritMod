using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
    public class CryoliteOreTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSpelunker[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = false;  //true for block to emit light
            Main.tileLighted[Type] = false;
            drop = mod.ItemType("CryoliteOre");   //put your CustomBlock name
			ModTranslation name = CreateMapEntryName();
            name.SetDefault("Cryolite Ore");
            AddMapEntry(new Color(40, 0, 205), name);
			soundType = 21;
            minPick = 75;
            dustType = 68;
            
        }
       
    }
}