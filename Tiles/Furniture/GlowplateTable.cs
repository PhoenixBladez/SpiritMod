using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Placeable.Tiles;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture
{
	public class GlowplateTable : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Glowplate Table");
			AddMapEntry(new Color(50, 50, 50), name);
			TileID.Sets.DisableSmartCursor[Type] = true;
            AdjTiles = new int[] { TileID.Tables };
        }

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 16, ModContent.ItemType<GlowplateTableItem>());
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
            Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Furniture/GlowplateTable_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(150, 150, 150, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            Tile t = Main.tile[i, j];
        }
	}
	public class GlowplateTableItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glowplate Table");
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 28;
			Item.value = Item.value = Terraria.Item.buyPrice(0, 0, 5, 0);
			Item.rare = ItemRarityID.Blue;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<GlowplateTable>();
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TechBlockItem>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}