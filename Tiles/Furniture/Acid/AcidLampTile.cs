using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Placeable.Furniture.Reach;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture.Acid
{
	public class AcidLampTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileLighted[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			ModTranslation name = CreateMapEntryName();
            name.SetDefault("Corrosive Lamp");
            dustType = -1;
            AddMapEntry(new Color(63, 204, 68), name);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.48f;
            g = 0.75f;
            b = 0.47f;
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
            Main.spriteBatch.Draw(mod.GetTexture("Tiles/Furniture/Acid/AcidLampTile_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            Tile t = Main.tile[i, j];
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
            Terraria.Item.NewItem(i * 16, j * 16, 16, 32, ModContent.ItemType<Items.Placeable.Furniture.Acid.AcidLamp>());
        }
    }
}