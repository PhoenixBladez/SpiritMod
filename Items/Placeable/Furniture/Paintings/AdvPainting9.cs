using SpiritMod.Tiles.Furniture.Paintings;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Placeable.Furniture.Paintings
{
	public class AdvPainting9 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clouded Shoreline");
			Tooltip.SetDefault("'S. Yaki'");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 32;
			item.value = item.value = Terraria.Item.buyPrice(0, 0, 40, 0);
			item.rare = ItemRarityID.White;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<AdvPainting9Tile>();
		}

	}
}