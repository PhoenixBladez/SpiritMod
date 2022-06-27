
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Furniture.Neon;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;

namespace SpiritMod.Tiles.Furniture
{
	public class TechCrateBig : ModTile
	{
		public override void SetStaticDefaults()
        {
            Main.tileTable[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Height = 2;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleWrapLimit = 2; //not really necessary but allows me to add more subtypes of chairs below the example chair texture
			TileObjectData.newTile.StyleMultiplier = 2; //same as above
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight; //allows me to place example chairs facing the same way as the player
			TileObjectData.addAlternate(1); //facing right will use the second texture style
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Glowplate Crate");
			AddMapEntry(new Color(60, 60, 60), name);
			DustType = -1;
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = .052f;
            g = .229f;
            b = .235f;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 48, 48, ModContent.ItemType<LargeTechCrate>());

		}
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = tile.TileFrameY == 36 ? 18 : 16;
            Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Furniture/TechCrateBig_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(150, 150, 150, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            Tile t = Main.tile[i, j];
        }
    }
}