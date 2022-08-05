using SpiritMod.Tiles.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Material;
using SpiritMod.Items.Sets.MarbleSet;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class SynthwaveHeadItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hyperspace Bust");
			Tooltip.SetDefault("Creates an artificial Hyperspace Biome on right-click");
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 28;
			Item.value = Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.White;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<SynthwaveHead>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MarbleBlock, 15);
			recipe.AddIngredient(ModContent.ItemType<MarbleChunk>(), 1);
			recipe.AddIngredient(ModContent.ItemType<SynthMaterial>(), 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}