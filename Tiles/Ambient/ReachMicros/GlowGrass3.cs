using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.ReachMicros
{
	public class GlowGrass3 : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileLighted[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Width = 2;		
            TileObjectData.newTile.CoordinateHeights = new int[]
			{
			16,
			16,
            16
			};
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorBottom = default(AnchorData);
			TileObjectData.addTile(Type);
        	dustType = 244;
			adjTiles = new int[] { TileID.Torches };
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			{
				r = .245f;
				g = .185f;
				b = .066f;
			}
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			int height = tile.frameY == 36 ? 18 : 16;
			Main.spriteBatch.Draw(mod.GetTexture("Tiles/Ambient/ReachMicros/GlowGrass3_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Tile t = Main.tile[i, j];
		}
	}
}