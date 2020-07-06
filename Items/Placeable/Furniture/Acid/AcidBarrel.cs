using SpiritMod.Items.Placeable.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using AcidBarrelTile = SpiritMod.Tiles.Furniture.Acid.AcidBarrelTile;

namespace SpiritMod.Items.Placeable.Furniture.Acid
{
	public class AcidBarrel : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrosive Barrel");
		}


		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 28;
			item.value = 200;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<AcidBarrelTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AcidBrick>(), 10);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}