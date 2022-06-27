using SpiritMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class TreasureChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Loot Chest");
			Tooltip.SetDefault("'It's already filled to the brim!'");

		}


		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 24;
			Item.value = 850;
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 99;
			Item.value = Terraria.Item.buyPrice(0, 0, 90, 0);
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<TreasureChestTile>();
		}
	}
}