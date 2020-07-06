using SpiritMod.Items.Placeable.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using AcidDoorClosed = SpiritMod.Tiles.Furniture.Acid.AcidDoorClosed;

namespace SpiritMod.Items.Placeable.Furniture.Acid
{
	public class AcidDoor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrosive Door");
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

			item.createTile = ModContent.TileType<AcidDoorClosed>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AcidBrick>(), 6);
            recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}