using SpiritMod.Items.Placeable.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using AsteroidWallWall = SpiritMod.Tiles.Walls.Natural.AsteroidWall;

namespace SpiritMod.Items.Placeable.Walls
{
	public class AsteroidWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Asteroid Wall");
		}


		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 22;

			item.maxStack = 999;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 7;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createWall = ModContent.WallType<AsteroidWallWall>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AsteroidBlock>());
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(this, 4);
			recipe1.AddTile(TileID.WorkBenches);
			recipe1.SetResult(ModContent.ItemType<AsteroidBlock>());
			recipe1.AddRecipe();
		}
	}
}