using SpiritMod.Tiles.Furniture.Pylons;
using Terraria.Enums;
using Terraria.ModLoader;

namespace SpiritMod.Items.ByBiome.Spirit.Placeables.Furniture
{
	internal class SpiritPylonItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<SpiritPylonTile>());
			Item.SetShopValues(ItemRarityColor.Blue1, Terraria.Item.buyPrice(gold: 10));
		}
	}
}
