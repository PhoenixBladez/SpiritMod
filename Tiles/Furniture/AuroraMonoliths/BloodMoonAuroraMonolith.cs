using SpiritMod.Skies.Overlays;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Furniture.AuroraMonoliths
{
	public class BloodMoonAuroraMonolith : AuroraMonolith
	{
        internal override int DropType => ModContent.ItemType<BloodMoonAuroraMonolithItem>();
        internal override int AuroraType => AuroraOverlay.BLOODMOON;
    }

	public class BloodMoonAuroraMonolithItem : AuroraMonolithItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Blood Moon Aurora Monolith");
		public override int PlaceType => ModContent.TileType<BloodMoonAuroraMonolith>();

        public override void SafeAddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Items.Sets.BloodcourtSet.DreamstrideEssence>(), 10);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}