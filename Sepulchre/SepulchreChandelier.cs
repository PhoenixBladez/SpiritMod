using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Sepulchre
{
	public class SepulchreChandelier : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileLighted[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorBottom = default(AnchorData); TileObjectData.newTile.CoordinateHeights = new[]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.Origin = new Point16(1, 0);
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Sepulchre Chandelier");
			dustType = -1;
			AddMapEntry(new Color(179, 146, 107), name);
			adjTiles = new int[] { TileID.Chandeliers };
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0f;
			g = 1.0f;
			b = 0.4f;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 48, ModContent.ItemType<Sepulchre.SepulchreChandelierItem>());
		}
	}
	public class SepulchreChandelierItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sepulchre Chandelier");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 30;

			item.maxStack = 999;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<SepulchreChandelier>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Tiles.SepulchreBrickTwoItem>(), 8);
			recipe.AddIngredient(ItemID.CursedTorch, 1);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}