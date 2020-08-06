using SpiritMod.Items.Placeable.Tiles;
using SpiritMod.Tiles.Walls.Natural;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Walls
{
	public class GlowplateWallItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Smooth Glowplate Wall");
		}


		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;

			item.maxStack = 999;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 7;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createWall = ModContent.WallType<GlowplateWall>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TechBlockItem>());
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(this, 4);
			recipe1.AddTile(TileID.WorkBenches);
			recipe1.SetResult(ModContent.ItemType<TechBlockItem>());
			recipe1.AddRecipe();
		}
	}
}