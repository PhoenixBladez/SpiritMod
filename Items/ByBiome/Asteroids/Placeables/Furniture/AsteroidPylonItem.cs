using SpiritMod.Tiles.Furniture.Pylons;
using Terraria.Enums;
using Terraria.ModLoader;

namespace SpiritMod.Items.ByBiome.Asteroids.Placeables.Furniture
{
	internal class AsteroidPylonItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<AsteroidPylonTile>());
			Item.SetShopValues(ItemRarityColor.Blue1, Terraria.Item.buyPrice(gold: 10));
		}
	}
}
