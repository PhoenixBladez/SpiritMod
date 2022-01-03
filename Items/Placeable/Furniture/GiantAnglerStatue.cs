using SpiritMod.Tiles.Furniture;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using SpiritMod.Mechanics.QuestSystem.Quests;
using SpiritMod.Mechanics.QuestSystem;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class GiantAnglerStatue : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Giant Manshark Statue");
		}


		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 34;
			item.value = 5000;

			item.maxStack = 99;
			item.value = Item.buyPrice(gold: 3);

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<GiantAnglerStatueTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.FishCrate>(), 3);
			recipe.AddIngredient(ItemID.IronBar, 20);
			recipe.anyIronBar = true;
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}