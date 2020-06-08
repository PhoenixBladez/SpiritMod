
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.ReachMicros
{
    public class GlowGrass2 : ModTile
    {
        public override void SetDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileLighted[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
            16,
            16,
            16
            };
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Eerie Root");
            dustType = 244;
            AddMapEntry(new Color(193, 158, 32), name);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
            {
                r = .5f;
                g = .36f;
                b = .12f;
            }
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height) {
            offsetY = 2;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch) {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if(Main.drawToScreen) {
                zero = Vector2.Zero;
            }
            int height = tile.frameY == 36 ? 18 : 16;
            Main.spriteBatch.Draw(mod.GetTexture("Tiles/Ambient/ReachMicros/GlowGrass2_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            Tile t = Main.tile[i, j];
        }
    }
}