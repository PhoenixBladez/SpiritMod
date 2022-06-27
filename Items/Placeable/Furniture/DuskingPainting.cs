using SpiritMod.Tiles.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.DuskingDrops;
using SpiritMod.Items.Material;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class DuskingPainting : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dusking's Wrath");
			Tooltip.SetDefault("'S. Kletony'");
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 28;
			Item.value = Item.value = Terraria.Item.buyPrice(0, 0, 40, 10);
			Item.rare = ItemRarityID.White;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<DuskingPaintingTile>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Canvas>());
			recipe.AddIngredient(ModContent.ItemType<DuskStone>());
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}