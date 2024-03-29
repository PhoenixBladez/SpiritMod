using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Effects.Waters.Reach;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture.Fountains
{
    public abstract class BaseFountain : ModTile
    {
		internal virtual int DropType => ModContent.ItemType<BriarFountainItem>();
		internal virtual int WaterStyle => ModContent.GetInstance<ReachWaterStyle>().Type;

		public sealed override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
            TileObjectData.newTile.Origin = new Point16(3, 2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(75, 139, 166));

            dustType = DustID.Stone;
            animationFrameHeight = 72;
            disableSmartCursor = true;
            adjTiles = new int[] { TileID.WaterFountain };
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(i * 16, j * 16, 32, 48, DropType);

		public sealed override void NearbyEffects(int i, int j, bool closer)
		{
			if (Framing.GetTileSafely(i, j).frameY >= 72)
				Main.LocalPlayer.GetSpiritPlayer().fountainsActive["BRIAR"] = 4;
		}

		public sealed override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frame = Main.tileFrame[TileID.WaterFountain];
            frameCounter = Main.tileFrameCounter[TileID.WaterFountain];
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Texture2D texture = Main.canDrawColorTile(i, j) ? Main.tileAltTexture[Type, tile.color()] : Main.tileTexture[Type];

            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            int animate = tile.frameY >= 72 ? (Main.tileFrame[Type] * (animationFrameHeight + 2)) + 2 : 0;

			Main.spriteBatch.Draw(texture, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.frameX, tile.frameY + animate, 16, 16), Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
            return false;
        }

        public override bool NewRightClick(int i, int j)
        {
            Main.PlaySound(SoundID.Waterfall, i * 16, j * 16, 0);
            HitWire(i, j);
            return true;
        }

        public sealed override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.showItemIcon = true;
            player.showItemIcon2 = DropType;
        }

        public sealed override void HitWire(int i, int j)
        {
            int x = i - Main.tile[i, j].frameX / 18 % 5;
            int y = j - Main.tile[i, j].frameY / 18 % 4;

            for (int l = x; l < x + 5; l++)
            {
                for (int m = y; m < y + 4; m++)
                {
                    if (Main.tile[l, m] == null)
                        Main.tile[l, m] = new Tile();

                    if (Main.tile[l, m].active() && Main.tile[l, m].type == Type)
                    {
                        if (Main.tile[l, m].frameY < 72)
                            Main.tile[l, m].frameY += 72;
                        else
                            Main.tile[l, m].frameY -= 72;
                    }
                }
            }

            if (Wiring.running)
            {
                for (int k = 0; k < 3; ++k)
                {
                    Wiring.SkipWire(x, y + k);
                    Wiring.SkipWire(x + 1, y + k);
                }
            }
            NetMessage.SendTileSquare(-1, x, y + 1, 3);
        }
    }

	public abstract class BaseFountainItem : ModItem
	{
		internal virtual int PlaceType => ModContent.TileType<BriarFountain>();

		public override void SetDefaults()
		{
			item.width = 52;
			item.height = 58;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 16;
			item.useTime = 16;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.rare = ItemRarityID.Blue;
			item.value = Item.buyPrice(0, 4, 0, 0);
			item.createTile = PlaceType;
		}
	}
}