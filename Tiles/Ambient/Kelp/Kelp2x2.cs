using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.Kelp
{
	public class LargeKelpItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Large Glowing Plant");
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 24;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<Kelp2x2>();
			Item.maxStack = 999;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Sets.FloatingItems.Kelp>(), 12);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.Register();
		}
	}
	public class Kelp2x2 : ModTile
	{
		readonly Texture2D glowmask;

		public Kelp2x2()
		{
			glowmask = ModContent.Request<Texture2D>("SpiritMod/Tiles/Ambient/Kelp/Kelp2x2_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		}

		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileCut[Type] = false;
			Main.tileLighted[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			//TileObjectData.newTile.AnchorValidTiles = new int[] { TileID.Sand, TileID.Crimsand, TileID.Ebonsand };
			TileObjectData.newTile.RandomStyleRange = 1;
			TileObjectData.addTile(Type);

			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = DustID.Grass;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Kelp");
			AddMapEntry(new Color(24, 105, 25), name);
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (Framing.GetTileSafely(i, j).TileFrameY == 0) {
				r = 0.28f * 1.5f;
				g = 0.28f * 1.5f;
				b = 0;
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile t = Framing.GetTileSafely(i, j);
			Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
			spriteBatch.Draw(glowmask, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(t.TileFrameX, t.TileFrameY, 16, 16), Color.LightYellow);
		}
	}
}
