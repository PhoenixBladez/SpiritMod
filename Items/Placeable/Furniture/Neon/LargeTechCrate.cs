using SpiritMod.Tiles.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Placeable.Tiles;
using SpiritMod.Tiles.Block;
namespace SpiritMod.Items.Placeable.Furniture.Neon
{
	public class LargeTechCrate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Large Glowplate Crate");
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

			Item.createTile = ModContent.TileType<TechCrateBig>();
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TechBlockItem>(), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}