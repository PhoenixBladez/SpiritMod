using SpiritMod.Tiles.Furniture.Pylons;
using Terraria.Enums;
using Terraria.ModLoader;

namespace SpiritMod.Items.ByBiome.Briar.Placeable.Furniture
{
	internal class BriarPylonItem : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Briar Pylon");

		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<BriarPylonTile>());
			Item.SetShopValues(ItemRarityColor.Blue1, Terraria.Item.buyPrice(gold: 10));
		}
	}
}
