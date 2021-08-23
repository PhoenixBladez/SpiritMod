using SpiritMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Placeable.Tiles;
using SpiritMod.Tiles.Block;
namespace SpiritMod.Items.Placeable.Furniture.Neon
{
	public class GeneratorItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glowplate Generator");
		}

		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 28;
			item.value = item.value = Terraria.Item.buyPrice(0, 0, 5, 0);
			item.rare = ItemRarityID.Blue;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<Generator>();
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<TechBlockItem>(), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}