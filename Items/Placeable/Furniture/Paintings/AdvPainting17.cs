using SpiritMod.Tiles.Furniture.Paintings;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Placeable.Furniture.Paintings
{
	public class AdvPainting17 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dreams of a World Beyond");
			Tooltip.SetDefault("'P. Otat'");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 32;
			item.value = item.value = Terraria.Item.buyPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.White;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<AdvPainting18Tile>();
		}

	}
}