using SpiritMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Placeable.Furniture
{
	public class WheezerPainting : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Famished Wheezer");
			Tooltip.SetDefault("'K. Rant'");
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 28;
			Item.value = Item.value = Terraria.Item.buyPrice(0, 0, 40, 10);
			Item.rare = ItemRarityID.White;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<WheezerPaintingTile>();
		}

	}
}