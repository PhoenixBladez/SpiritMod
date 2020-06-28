using SpiritMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Placeable.Furniture
{
	public class ReachPainting : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Just out of Reach");
			Tooltip.SetDefault("'M. Beigh'");
		}

		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 28;
			item.value = item.value = Terraria.Item.buyPrice(0, 0, 40, 10);
			item.rare = 0;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<ReachPaintingTile>();
		}

	}
}