using SpiritMod.Items.Placeable.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using AcidSofaTile = SpiritMod.Tiles.Furniture.Acid.AcidSofaTile;

namespace SpiritMod.Items.Placeable.Furniture.Acid
{
	public class AcidSofa : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrosive Sofa");
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

			item.createTile = ModContent.TileType<AcidSofaTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AcidBrick>(), 5);
            recipe.AddIngredient(ItemID.Silk, 2);
            recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}