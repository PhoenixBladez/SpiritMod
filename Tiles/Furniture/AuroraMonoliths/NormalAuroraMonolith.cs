using SpiritMod.Skies.Overlays;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Furniture.AuroraMonoliths
{
	public class NormalAuroraMonolith : AuroraMonolith
	{
        internal override int DropType => ModContent.ItemType<NormalAuroraMonolithItem>();
        internal override int AuroraType => AuroraOverlay.PRIMARY;
    }

	public class NormalAuroraMonolithItem : AuroraMonolithItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aurora Monolith");
		}
		public override int PlaceType => ModContent.TileType<NormalAuroraMonolith>();

        public override void SafeAddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IceBlock, 10);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}