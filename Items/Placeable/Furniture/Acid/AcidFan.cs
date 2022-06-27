using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AcidFanTile = SpiritMod.Tiles.Furniture.Acid.AcidFanTile;

namespace SpiritMod.Items.Placeable.Furniture.Acid
{
	public class AcidFan : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rusty Fan");
		}


		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 28;
			Item.value = 200;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<AcidFanTile>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<AcidBrick>(), 10);
			recipe.AddIngredient(ItemID.Wire, 5);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.Register();
		}
	}
}