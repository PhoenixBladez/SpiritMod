using SpiritMod.Tiles.Block;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.HuskstalkSet
{
	public class AncientBark : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elderbark");

		}


		public override void SetDefaults()
		{
			Item.width = Item.height = 16;
			Item.maxStack = 999;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 7;
			Item.useAnimation = 15;
			Item.rare = ItemRarityID.White;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<BarkTileTile>();
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Furniture.Reach.ReachPlatformTile>(), 2);
            recipe.Register();
        }
    }
}
