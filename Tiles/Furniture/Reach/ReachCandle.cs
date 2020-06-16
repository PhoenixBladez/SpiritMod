using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Tiles.Furniture.Reach
{
    public class ReachCandle : ModTile
    {
        public override void SetDefaults() {
            Main.tileTable[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            adjTiles = new int[] { TileID.Torches };
            AddMapEntry(new Color(179, 146, 107));
            drop = ModContent.TileType<ReachCandle>();
        }

        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
            r = 0.9f;
            g = 0.8f;
            b = 0.5f;
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height) {
            offsetY = 2;
        }

    }
}