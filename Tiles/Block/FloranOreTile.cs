using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
    public class FloranOreTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSpelunker[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            drop = mod.ItemType("FloranOre");   //put your CustomBlock name
			ModTranslation name = CreateMapEntryName();
            name.SetDefault("Floran Ore");
            AddMapEntry(new Color(30, 200, 25), name);
			soundType = 21;
            dustType = 3;
            minPick = 45;
            
        }
    }
}