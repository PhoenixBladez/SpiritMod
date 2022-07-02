using SpiritMod.Items.Placeable.Tiles;
using Terraria;
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
			Item.width = 22;
			Item.height = 22;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 7;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createWall = ModContent.WallType<AsteroidWallWall>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(4);
			recipe.AddIngredient(ModContent.ItemType<AsteroidBlock>());
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();

			Recipe recipe1 = Recipe.Create(ModContent.ItemType<AsteroidBlock>());
			recipe1.AddIngredient(this, 4);
			recipe1.AddTile(TileID.WorkBenches);
			recipe1.Register();
		}
	}
}