using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.Ocean
{
	public class Mussel : ModTile
	{
		public override void SetStaticDefaults()
		{
			const int StyleRange = 3;
			const bool StyleHori = true;

			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileNoAttach[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileWaterDeath[Type] = true;

			TileID.Sets.FramesOnKillWall[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.RandomStyleRange = StyleRange;
			TileObjectData.newTile.StyleHorizontal = StyleHori;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorAlternateTiles = new int[] { TileID.WoodenBeam };
			TileObjectData.newAlternate.RandomStyleRange = StyleRange;
			TileObjectData.newAlternate.StyleHorizontal = StyleHori;
			TileObjectData.addAlternate(6);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorAlternateTiles = new int[] { TileID.WoodenBeam };
			TileObjectData.newAlternate.RandomStyleRange = StyleRange;
			TileObjectData.newAlternate.StyleHorizontal = StyleHori;
			TileObjectData.addAlternate(3);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidBottom | AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorAlternateTiles = new int[] { TileID.WoodenBeam };
			TileObjectData.newAlternate.RandomStyleRange = StyleRange;
			TileObjectData.newAlternate.StyleHorizontal = StyleHori;
			TileObjectData.addAlternate(9);
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(200, 200, 200));
			DustType = DustID.Stone;
			ItemDrop = ModContent.ItemType<MusselItem>();
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = Main.rand.Next(1, 3);
	}

	public class MusselItem : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Mussel");

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 24;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<Mussel>();
			Item.maxStack = 999;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(5);
			recipe.AddIngredient(ItemID.Coral, 2);
			recipe.AddIngredient(ItemID.Wood, 1);
			recipe.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe.Register();
		}
	}
}