using SpiritMod.Items.Placeable.Tiles;
using SpiritMod.Tiles.Walls.Natural;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Walls
{
	public class AcidWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrosive Brick Wall");
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

			Item.createWall = ModContent.WallType<AcidBrickWallTile>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(4);
			recipe.AddIngredient(ModContent.ItemType<AcidBrick>());
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.Register();

			Recipe recipe1 = Mod.CreateRecipe(ModContent.ItemType<AcidBrick>());
			recipe1.AddIngredient(this, 4);
			recipe1.AddTile(TileID.HeavyWorkBench);
			recipe1.Register();
		}
	}
}