using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture.Acid
{
	public class AcidLanternTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileLighted[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorBottom = default(AnchorData);
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			DustType = -1;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Corrosive Lantern");
			AddMapEntry(new Color(63, 204, 68), name);
			AdjTiles = new int[] { TileID.Torches };
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.48f;
			g = 0.75f;
			b = 0.47f;
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
			int height = tile.TileFrameY == 36 ? 18 : 16;
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Furniture/Acid/AcidLanternTile_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Tile t = Main.tile[i, j];
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
			Terraria.Item.NewItem(i * 16, j * 16, 16, 32, ModContent.ItemType<Items.Placeable.Furniture.Acid.AcidLantern>());
		}
	}
}