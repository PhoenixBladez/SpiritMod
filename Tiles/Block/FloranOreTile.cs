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
            Main.tileMerge[Type][mod.TileType("ReachGrassTile")] = true;
            Main.tileBlockLight[Type] = false;  //true for block to emit light
            Main.tileLighted[Type] = true;
            drop = mod.ItemType("FloranOre");   //put your CustomBlock name
			ModTranslation name = CreateMapEntryName();
            name.SetDefault("Floran Ore");
            AddMapEntry(new Color(30, 200, 25), name);
			soundType = 21;
            dustType = 3;
            minPick = 45;
            
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)   //light colors
        {
            if (!Main.dayTime)
            r = 0;
            g = 0.074f;
            b = 0;
        }
    }
}