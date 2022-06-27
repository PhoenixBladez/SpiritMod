using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Skies.Overlays;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture.AuroraMonoliths
{
    public abstract class AuroraMonolith : ModTile
    {
        internal virtual int AuroraType => AuroraOverlay.UNUSED_BASIC;
        internal virtual int DropType => ModContent.ItemType<NormalAuroraMonolithItem>();

        public sealed override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(75, 139, 166));

            DustType = DustID.Stone;
            AnimationFrameHeight = 72;
            TileID.Sets.DisableSmartCursor[Type] = true;
            //adjTiles = new int[] { TileID.LunarMonolith };
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(i * 16, j * 16, 32, 48, DropType);

        public sealed override void NearbyEffects(int i, int j, bool closer)
        {
			if (Main.tile[i, j].TileFrameY >= AnimationFrameHeight)
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
            Texture2D texture = Main.canDrawColorTile(i, j) ? Main.tileAltTexture[Type, tile.TileColor] : TextureAssets.Tile[Type].Value;

            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            int height = tile.TileFrameY % AnimationFrameHeight == 54 ? 18 : 16;

            Main.spriteBatch.Draw(texture, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
            return false;
        }

        public override bool RightClick(int i, int j)
        {
            SoundEngine.PlaySound(SoundID.Mech, i * 16, j * 16, 0);
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
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			offsetY = 2;
		}
		public sealed override void HitWire(int i, int j)
        {
            int x = i - Main.tile[i, j].TileFrameX / 18 % 2;
            int y = j - Main.tile[i, j].TileFrameY / 18 % 4;

            for (int l = x; l < x + 2; l++)
            {
                for (int m = y; m < y + 4; m++)
                {
                    if (Main.tile[l, m] == null)
                        Main.tile[l, m] = new Tile();

                    if (Main.tile[l, m].HasTile && Main.tile[l, m].TileType == Type)
                    {
                        if (Main.tile[l, m].TileFrameY < AnimationFrameHeight)
                            Main.tile[l, m].TileFrameY += (short)AnimationFrameHeight;
                        else
                            Main.tile[l, m].TileFrameY -= (short)AnimationFrameHeight;
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
            Item.width = 22;
            Item.height = 32;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.createTile = PlaceType;
        }

        public override void AddRecipes() => SafeAddRecipes();

        public abstract void SafeAddRecipes();
    }
}