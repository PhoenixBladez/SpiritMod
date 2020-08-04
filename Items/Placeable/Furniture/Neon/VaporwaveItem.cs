using SpiritMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Material;

namespace SpiritMod.Items.Placeable.Furniture.Neon
{
	public class VaporwaveItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Retrofuturistic Wall Panel");
			Tooltip.SetDefault("'A mix of the past and present'");
		}

		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 28;
			item.value = item.value = Terraria.Item.buyPrice(0, 0, 10, 0);
			item.rare = 1;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<VaporwaveTile>();
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Material.SynthMaterial>(), 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}