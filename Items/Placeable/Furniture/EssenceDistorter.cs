using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;
using EssenceDistorterTile = SpiritMod.Tiles.Furniture.EssenceDistorter;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class EssenceDistorter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Essence Distorter");
			Tooltip.SetDefault("'Where essences are warped and merged'");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.maxStack = 1;
			item.rare = ItemRarityID.LightPurple;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = item.useAnimation = 25;

			item.autoReuse = true;
			item.consumable = true;


			item.createTile = ModContent.TileType<EssenceDistorterTile>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DuneEssence>());
			recipe.AddIngredient(ModContent.ItemType<TidalEssence>());
			recipe.AddIngredient(ModContent.ItemType<FieryEssence>());
			recipe.AddIngredient(ModContent.ItemType<IcyEssence>());
			recipe.AddIngredient(ModContent.ItemType<PrimevalEssence>());
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
