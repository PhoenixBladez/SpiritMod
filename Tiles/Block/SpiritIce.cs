using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
    public class SpiritIce : ModTile
    {
        public override void SetDefaults() {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            AddMapEntry(new Color(70, 130, 180));
            drop = ModContent.ItemType<SpiritIceItem>();
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
            if (!Framing.GetTileSafely(i, j - 1).active())
            {
                r = 0.08f;
                g = 0.12f;
                b = 0.28f;
            }
        }

        public override bool CanExplode(int i, int j) {
            return true;
        }
    }
}

