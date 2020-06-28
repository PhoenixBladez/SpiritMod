
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient
{
	public class SkullStickFlip : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
			16,
			16,
			16,
			16
			};
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Skull Stick");
			AddMapEntry(new Color(107, 90, 64), name);
			adjTiles = new int[] { TileID.Lamps };
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 4;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int item = 0;
			switch(frameX / 54) {
				case 0:
				case 1:
				case 2:
				case 3:
					item = ModContent.ItemType<Items.Placeable.Furniture.SkullStick>();
					break;
			}
			if(item > 0) {
				Item.NewItem(i * 16, j * 16, 48, 48, item);
			}
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if(!Main.dayTime) {
				r = .235f;
				g = .174f;
				b = .052f;
			}
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if(!Main.dayTime) {
				Tile tile = Main.tile[i, j];
				Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
				if(Main.drawToScreen) {
					zero = Vector2.Zero;
				}
				int height = tile.frameY == 36 ? 18 : 16;
				Main.spriteBatch.Draw(mod.GetTexture("Tiles/Ambient/SkullStick_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), new Color(100, 100, 100, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				Tile t = Main.tile[i, j];
			}
		}
	}
}