using SpiritMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Placeable.Furniture
{
	public class FloppaPainting : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Duality of Man");
			Tooltip.SetDefault("'Tix A. Luna'");
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 28;
			Item.value = Item.value = Terraria.Item.buyPrice(0, 10, 0, 0);
			Item.rare = -11;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<FloppaPainting_Tile>();
		}

	}
}