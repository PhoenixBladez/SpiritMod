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
	public class YellowNeonPlant : ModTile
	{
		public override void SetDefaults()
		{
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newTile.StyleWrapLimit = 2; //not really necessary but allows me to add more subtypes of chairs below the example chair texture
            TileObjectData.newTile.StyleMultiplier = 2; //same as above
            TileObjectData.newTile.StyleHorizontal = true;
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight; //allows me to place example chairs facing the same way as the player
            TileObjectData.addAlternate(1);
            TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			Main.tileLighted[Type] = true;
			name.SetDefault("Fluorescent Plant");
            AddMapEntry(new Color(255, 243, 74), name);
            adjTiles = new int[] { TileID.Torches };
            dustType = -1;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Terraria.Item.NewItem(i * 16, j * 16, 32, 16, ModContent.ItemType<Items.Placeable.Furniture.Neon.NeonPlantYellow>());
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = .255f * 1.5f;
            g = .243f * 1.5f;
            b = .074f * 1.5f;
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
			Main.spriteBatch.Draw(mod.GetTexture("Tiles/Furniture/NeonLights/YellowNeonPlant_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Tile t = Main.tile[i, j];
		}
	}
}