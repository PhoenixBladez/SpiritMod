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
			item.width = 48;
			item.height = 24;
			item.value = 850;
			item.rare = 2;
			item.maxStack = 99;
			item.value = Terraria.Item.buyPrice(0, 0, 90, 0);
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<TreasureChestTile>();
		}
	}
}