using SpiritMod.Tiles.Block;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Tiles
{
	public class NeonBlockGreenItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Green Fluorescent Block");
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

			item.createTile = ModContent.TileType<NeonBlockGreen>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Material.SynthMaterial>(), 1);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 30);
			recipe.AddRecipe();
		}
	}
}