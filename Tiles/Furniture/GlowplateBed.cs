using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SpiritMod.Items.Placeable.Tiles;

namespace SpiritMod.Tiles.Furniture
{
	public class GlowplateBed : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2); //this style already takes care of direction for us
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Glowplate Bed");
			AddMapEntry(new Color(50, 50, 50), name);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
            TileID.Sets.DisableSmartCursor[Type] = true;
			AdjTiles = new int[] { TileID.Beds };
			bed = true;
            TileID.Sets.HasOutlines[Type] = true;
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

		public override void NumDust(int i, int j, bool fail, ref int num) => num = 1;

		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(i * 16, j * 16, 64, 32, ModContent.ItemType<GlowplateBedItem>());

		public override bool RightClick(int i, int j)
		{
			Player player = Main.player[Main.myPlayer];
			Tile tile = Main.tile[i, j];
			int spawnX = i - tile.TileFrameX / 18;
			int spawnY = j + 2;
			spawnX += tile.TileFrameX >= 72 ? 5 : 2;
			if (tile.TileFrameY % 38 != 0)
				spawnY--;
			player.FindSpawn();
			if (player.SpawnX == spawnX && player.SpawnY == spawnY) {
				player.RemoveSpawn();
				Main.NewText("Spawn point removed!", 255, 240, 20, false);
			}
			else if (Player.CheckSpawn(spawnX, spawnY)) {
				player.ChangeSpawn(spawnX, spawnY);
				Main.NewText("Spawn point set!", 255, 240, 20, false);
			}
			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.player[Main.myPlayer];
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<GlowplateBedItem>();
		}

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
                zero = Vector2.Zero;
            int height = tile.TileFrameY == 36 ? 18 : 16;
            Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Furniture/GlowplateBed_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(150, 150, 150, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
	}
	
	public class GlowplateBedItem : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Glowplate Bed");

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 28;
			Item.value = Item.value = Item.buyPrice(0, 0, 5, 0);
			Item.rare = ItemRarityID.Blue;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<GlowplateBed>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<TechBlockItem>(), 15);
			recipe.AddIngredient(ItemID.Silk, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
    }
}