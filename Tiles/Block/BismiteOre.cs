using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
    public class BismiteOre : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSpelunker[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            drop = mod.ItemType("BismiteCrystal");   //put your CustomBlock name
			ModTranslation name = CreateMapEntryName();
            name.SetDefault("Bismite Crystal");
            AddMapEntry(new Color(30, 100, 25), name);
			soundType = 21;
            dustType = 167;
            Main.tileBlendAll[this.Type] = true;

        }
    }
}