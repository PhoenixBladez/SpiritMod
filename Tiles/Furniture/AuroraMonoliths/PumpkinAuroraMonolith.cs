using SpiritMod.Skies.Overlays;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Furniture.AuroraMonoliths
{
	public class PumpkinAuroraMonolith : AuroraMonolith
	{
        internal override int DropType => ModContent.ItemType<PumpkinAuroraMonolithItem>();
        internal override int AuroraType => AuroraOverlay.PUMPKINMOON;
    }

	public class PumpkinAuroraMonolithItem : AuroraMonolithItem
	{
        public override int PlaceType => ModContent.TileType<PumpkinAuroraMonolith>();

        public override void SafeAddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SpookyWood, 10);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}