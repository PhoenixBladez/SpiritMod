using SpiritMod.Items.Sets.HuskstalkSet;
using SpiritMod.Tiles.Walls.Natural;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Walls
{
	public class ElderbarkFence : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elderbark Fence");
		}


		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 7;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createWall = ModContent.WallType<ElderbarkFenceWall>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(4);
			recipe.AddIngredient(ModContent.ItemType<AncientBark>());
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();

			Recipe recipe1 = Recipe.Create(ModContent.ItemType<AncientBark>());
			recipe1.AddIngredient(this, 4);
			recipe1.AddTile(TileID.WorkBenches);
			recipe1.Register();
		}
	}
}