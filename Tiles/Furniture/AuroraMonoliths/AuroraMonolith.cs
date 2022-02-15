using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Skies.Overlays;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture.AuroraMonoliths
{
    public abstract class AuroraMonolith : ModTile
    {
        internal virtual int AuroraType => AuroraOverlay.UNUSED_BASIC;
        internal virtual int DropType => ModContent.ItemType<NormalAuroraMonolithItem>();

        public sealed override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(75, 139, 166));

            dustType = DustID.Stone;
            animationFrameHeight = 72;
            disableSmartCursor = true;
            //adjTiles = new int[] { TileID.LunarMonolith };
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(i * 16, j * 16, 32, 48, DropType);

        public sealed override void NearbyEffects(int i, int j, bool closer)
        {
			if (Main.tile[i, j].frameY >= animationFrameHeight)
				Main.LocalPlayer.GetSpiritPlayer().auroraMonoliths[AuroraType] = 6;
		}

        //public override void AnimateTile(ref int frame, ref int frameCounter)
        //{
        //    //frame = Main.tileFrame[TileID.LunarMonolith];
        //    //frameCounter = Main.tileFrameCounter[TileID.LunarMonolith];
        //}

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Texture2D texture = Main.canDrawColorTile(i, j) ? Main.tileAltTexture[Type, tile.color()] : Main.tileTexture[Type];

            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            int height = tile.frameY % animationFrameHeight == 54 ? 18 : 16;

            Main.spriteBatch.Draw(texture, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
            return false;
        }

        public override bool NewRightClick(int i, int j)
        {
            Main.PlaySound(SoundID.Mech, i * 16, j * 16, 0);
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
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
		}
		public sealed override void HitWire(int i, int j)
        {
            int x = i - Main.tile[i, j].frameX / 18 % 2;
            int y = j - Main.tile[i, j].frameY / 18 % 4;

            for (int l = x; l < x + 2; l++)
            {
                for (int m = y; m < y + 4; m++)
                {
                    if (Main.tile[l, m] == null)
                        Main.tile[l, m] = new Tile();

                    if (Main.tile[l, m].active() && Main.tile[l, m].type == Type)
                    {
                        if (Main.tile[l, m].frameY < animationFrameHeight)
                            Main.tile[l, m].frameY += (short)animationFrameHeight;
                        else
                            Main.tile[l, m].frameY -= (short)animationFrameHeight;
                    }
                }
            }

            if (Wiring.running)
            {
                for (int k = 0; k < 4; ++k)
                {
                    Wiring.SkipWire(x, y + k);
                    Wiring.SkipWire(x + 1, y + k);
                }
            }
            NetMessage.SendTileSquare(-1, x, y + 1, 4);
        }
    }

    public abstract class AuroraMonolithItem : ModItem
    {
		public virtual int PlaceType => ModContent.TileType<NormalAuroraMonolith>();

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 32;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 16;
            item.useTime = 16;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.rare = ItemRarityID.LightRed;
            item.value = Item.buyPrice(0, 2, 0, 0);
            item.createTile = PlaceType;
        }

        public override void AddRecipes() => SafeAddRecipes();

        public abstract void SafeAddRecipes();
    }
}