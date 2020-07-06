using SpiritMod.Tiles.Block;
using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Tiles
{
	public class AcidBrick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrosive Brick");
		}


		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 14;

			item.maxStack = 999;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<AcidBrickTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Acid>(), 1);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this, 20);
			recipe.AddRecipe();
		}
	}
}