using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SpiritMod.Tiles.Furniture.NeonLights
{
	public class BlueNeonCandle : ModTile
	{
		public override void SetDefaults()
		{
            Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newTile.StyleWrapLimit = 2; //not really necessary but allows me to add more subtypes of chairs below the example chair texture
            TileObjectData.newTile.StyleMultiplier = 2; //same as above
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight; //allows me to place example chairs facing the same way as the player
            TileObjectData.addAlternate(1); //facing right will use the second texture style
            TileObjectData.addTile(Type);
            drop = ModContent.ItemType<Items.Placeable.Furniture.Neon.NeonCandleBlue>();
			ModTranslation name = CreateMapEntryName();
			Main.tileLighted[Type] = true;
			name.SetDefault("Fluorescent Candle");
            AddMapEntry(new Color(77, 255, 88), name);
            adjTiles = new int[] { TileID.Torches };
            dustType = -1;
            //TODO	Main.highlightMaskTexture[Type] = mod.GetTexture("Tiles/ScoreBoard_Outline");
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.082f * 1.75f;
            g = 0.125f * 1.75f;
            b = 0.255f * 1.75f;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen) {
				zero = Vector2.Zero;
			}
			int height = tile.frameY == 36 ? 18 : 16;
			Main.spriteBatch.Draw(mod.GetTexture("Tiles/Furniture/NeonLights/BlueNeonCandle_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Tile t = Main.tile[i, j];
		}
	}
}