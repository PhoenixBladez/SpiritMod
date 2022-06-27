using SpiritMod.Tiles.Ambient;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class HourglassItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Hourglass");

        }


		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 34;
			Item.value = 150;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<Hourglass>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SandstoneBrick, 15);
            recipe.AddIngredient(ItemID.SandBlock, 5);
            recipe.AddIngredient(ItemID.Glass, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}