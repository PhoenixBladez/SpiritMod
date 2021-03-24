using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture
{
    public class FestivalLanternItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Festival Lantern");
			Tooltip.SetDefault("Starts a lantern festival nearby when placed");
		}


		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 22;
			item.value = Item.sellPrice(0, 0, 10, 0);

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<FestivalLanternTile>();
		}
	}
	public class FestivalLanternTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileLighted[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorBottom = default(AnchorData);
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			dustType = -1;//ModContent.DustType<Pixel>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Festival Lantern");
            AddMapEntry(new Color(100, 100, 100), name);
            adjTiles = new int[] { TileID.Torches };
            dustType = -1;
        }


        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = .255f * 1.5f;
            g = .243f * 1.5f;
            b = .074f * 1.5f;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 48, 48, ModContent.ItemType<FestivalLanternItem>());
		}
        public override void NearbyEffects(int i, int j, bool closer)
		{
			MyPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
			modPlayer.ZoneLantern = true;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen) {
				zero = Vector2.Zero;
			}
			int height = tile.frameY == 36 ? 18 : 16;
			Main.spriteBatch.Draw(mod.GetTexture("Tiles/Furniture/FestivalLantern_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}