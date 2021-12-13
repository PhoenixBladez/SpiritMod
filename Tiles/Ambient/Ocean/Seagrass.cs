using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Enums;
using Terraria;
using Microsoft.Xna.Framework;
using System;

namespace SpiritMod.Tiles.Ambient.Ocean
{
    internal class Seagrass : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileSolidTop[Type] = false;
			Main.tileSolid[Type] = false;
			Main.tileCut[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.newTile.AnchorValidTiles = new int[] { TileID.Sand, TileID.Crimsand, TileID.Ebonsand };
            TileObjectData.newTile.RandomStyleRange = 16;
            TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(Type);

			dustType = DustID.JungleGrass;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects) => effects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile t = Framing.GetTileSafely(i, j);
			Color col = Lighting.GetColor(i, j);
			Rectangle source = new Rectangle(t.frameX, t.frameY, 16, 16);

			Vector2 TileOffset = Lighting.lightMode > 1 ? Vector2.Zero : Vector2.One * 12; //Draw offset
			Vector2 drawPos = ((new Vector2(i + 0.4f, j + 1.15f) + TileOffset) * 16) - Main.screenPosition;

			float rotation = (float)Math.Sin(Main.GlobalTime * 1.2f) * Main.windSpeed;

			spriteBatch.Draw(Main.tileTexture[Type], drawPos, source, col, rotation, new Vector2(8, 16), 1f, SpriteEffects.None, 0f);
			return false;
		}
    }
}