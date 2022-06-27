using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AcidCandelabraTile = SpiritMod.Tiles.Furniture.Acid.AcidCandelabraTile;

namespace SpiritMod.Items.Placeable.Furniture.Acid
{
	public class AcidCandelabra : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrosive Candelabra");
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

			Item.createTile = ModContent.TileType<AcidCandelabraTile>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<AcidBrick>(), 5);
			recipe.AddIngredient(ItemID.Torch, 3);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.Register();
		}
	}
}