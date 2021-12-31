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
			item.width = 30;
			item.height = 24;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
			item.createTile = ModContent.TileType<Kelp2x2>();
			item.maxStack = 999;
			item.autoReuse = true;
			item.consumable = true;
			item.useAnimation = 15;
			item.useTime = 10;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Items.Sets.FloatingItems.Kelp>(), 12);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class Kelp2x2 : ModTile
	{
		readonly Texture2D glowmask;

		public Kelp2x2()
		{
			glowmask = ModContent.GetTexture("SpiritMod/Tiles/Ambient/Kelp/Kelp2x2_Glow");
		}

		public override void SetDefaults()
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

			disableSmartCursor = true;
			dustType = DustID.Grass;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Kelp");
			AddMapEntry(new Color(24, 105, 25), name);
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (Framing.GetTileSafely(i, j).frameY == 0) {
				r = 0.28f * 1.5f;
				g = 0.28f * 1.5f;
				b = 0;
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile t = Framing.GetTileSafely(i, j);
			Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
			spriteBatch.Draw(glowmask, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(t.frameX, t.frameY, 16, 16), Color.LightYellow);
		}
	}
}
