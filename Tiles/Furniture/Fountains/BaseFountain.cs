using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Effects.Waters.Reach;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture.Fountains
{
    public abstract class BaseFountain : ModTile
    {
		internal virtual int DropType => ModContent.ItemType<BriarFountainItem>();
		internal virtual int WaterStyle => ModContent.GetInstance<ReachWaterStyle>().Slot;

		public sealed override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
            TileObjectData.newTile.Origin = new Point16(3, 2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(75, 139, 166));

            DustType = DustID.Stone;
            AnimationFrameHeight = 72;
            TileID.Sets.DisableSmartCursor[Type] = true;
            AdjTiles = new int[] { TileID.WaterFountain };
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(i * 16, j * 16, 32, 48, DropType);

		public sealed override void NearbyEffects(int i, int j, bool closer)
		{
			if (Framing.GetTileSafely(i, j).TileFrameY >= 72)
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
            Texture2D texture = Main.canDrawColorTile(i, j) ? Main.tileAltTexture[Type, tile.TileColor] : TextureAssets.Tile[Type].Value;

            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            int animate = tile.TileFrameY >= 72 ? (Main.tileFrame[Type] * (AnimationFrameHeight + 2)) + 2 : 0;

			Main.spriteBatch.Draw(texture, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY + animate, 16, 16), Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
            return false;
        }

        public override bool RightClick(int i, int j)
        {
            SoundEngine.PlaySound(SoundID.Waterfall, i * 16, j * 16, 0);
            HitWire(i, j);
            return true;
        }

        public sealed override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = DropType;
        }

        public sealed override void HitWire(int i, int j)
        {
            int x = i - Main.tile[i, j].TileFrameX / 18 % 5;
            int y = j - Main.tile[i, j].TileFrameY / 18 % 4;

            for (int l = x; l < x + 5; l++)
            {
                for (int m = y; m < y + 4; m++)
                {
                    if (Main.tile[l, m] == null)
                        Main.tile[l, m] = new Tile();

                    if (Main.tile[l, m].HasTile && Main.tile[l, m].TileType == Type)
                    {
                        if (Main.tile[l, m].TileFrameY < 72)
                            Main.tile[l, m].TileFrameY += 72;
                        else
                            Main.tile[l, m].TileFrameY -= 72;
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
			Item.width = 52;
			Item.height = 58;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 16;
			Item.useTime = 16;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 4, 0, 0);
			Item.createTile = PlaceType;
		}
	}
}