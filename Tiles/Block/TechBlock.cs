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
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
		//	Main.tileMerge[Type][ModContent.TileType<SpiritDirt>()] = true;
			//Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			AddMapEntry(new Color(53, 59, 74));
			DustType = -1;
            HitSound = SoundID.Tink;
			ItemDrop = ModContent.ItemType<TechBlockItem>();
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			if (tile.Slope == 0 && !tile.IsHalfBlock) {
				{
                    Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                    if (Main.drawToScreen)
                    {
                        zero = Vector2.Zero;
                    }
                    Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Block/TechBlockGlow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(200, 200, 200, 200), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            }
		}
	}
}

