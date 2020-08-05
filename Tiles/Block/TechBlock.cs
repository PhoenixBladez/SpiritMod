using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;

namespace SpiritMod.Tiles.Block
{
	public class TechBlock : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
		//	Main.tileMerge[Type][ModContent.TileType<SpiritDirt>()] = true;
			//Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			AddMapEntry(new Color(53, 59, 74));
			dustType = -1;
            soundType = SoundID.Tink;
			drop = ModContent.ItemType<TechBlockItem>();
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			if (tile.slope() == 0 && !tile.halfBrick()) {
				{
                    Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                    if (Main.drawToScreen)
                    {
                        zero = Vector2.Zero;
                    }
                    Main.spriteBatch.Draw(mod.GetTexture("Tiles/Block/TechBlockGlow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, 16), new Color(200, 200, 200, 200), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            }
		}
	}
}

