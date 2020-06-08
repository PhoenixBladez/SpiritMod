using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.SpiritGrass
{
    public class SpiritGrassA5 : ModTile
    {
        public override void SetDefaults() {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.addTile(Type);
            Main.tileCut[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = true;
            //Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16
            };
            TileObjectData.addTile(Type);
            dustType = 206;
            soundType = SoundID.Grass;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = 10;
        }



    }
}