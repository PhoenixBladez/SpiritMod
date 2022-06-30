using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace SpiritMod.World.Sepulchre
{
	public class SepulchreMirror : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
			TileObjectData.newTile.AnchorBottom = default;
			TileObjectData.newTile.AnchorTop = default;
			TileObjectData.newTile.AnchorWall = true;
			TileObjectData.addTile(Type);
			DustType = -1;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Sepulchre Mirror");
			AddMapEntry(new Color(100, 100, 100), name);
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 48, ModContent.ItemType<SepulchreMirrorItem>());
	}

	public class SepulchreMirrorItem : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Sepulchre Mirror");

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 30;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<SepulchreMirror>();
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Tiles.SepulchreBrickTwoItem>(), 10);
			recipe.AddIngredient(ItemID.Glass, 5);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.Register();
		}
	}
}