using SpiritMod.Skies.Overlays;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Furniture.AuroraMonoliths
{
	public class FrostMoonAuroraMonolith : AuroraMonolith
	{
        internal override int DropType => ModContent.ItemType<FrostMoonAuroraMonolithItem>();
        internal override int AuroraType => AuroraOverlay.FROSTMOON;
    }

	public class FrostMoonAuroraMonolithItem : AuroraMonolithItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Moon Aurora Monolith");
		}
		public override int PlaceType => ModContent.TileType<FrostMoonAuroraMonolith>();

        public override void SafeAddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FrostCore, 1);
			recipe.AddIngredient(ItemID.Ectoplasm, 2);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}