using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.Kelp
{
	public class Kelp1x2 : ModTile
	{
		readonly Texture2D glowmask;

		public Kelp1x2()
		{
			glowmask = ModContent.GetTexture("SpiritMod/Tiles/Ambient/Kelp/Kelp1x2_Glow");
		}

		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileCut[Type] = false;
			Main.tileLighted[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorValidTiles = new int[] { TileID.Sand, TileID.Crimsand, TileID.Ebonsand };
			TileObjectData.newTile.RandomStyleRange = 1;
			TileObjectData.addTile(Type);

			disableSmartCursor = true;
			dustType = DustID.Grass;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Kelp");
			AddMapEntry(new Color(24, 105, 25), name);
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) => spriteEffects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (Framing.GetTileSafely(i, j).frameY == 0) {
				r = 0.34f * 1.5f;
				g = 0.34f * 1.5f;
				b = 0;
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile t = Framing.GetTileSafely(i, j);
			if (t.frameY == 0)
			{
				Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
				spriteBatch.Draw(glowmask, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(t.frameX, t.frameY, 16, 16), Color.LightYellow);
			}
		}
	}
}
