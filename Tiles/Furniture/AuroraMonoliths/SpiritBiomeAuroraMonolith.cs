using SpiritMod.Items.Sets.SpiritSet;
using SpiritMod.Skies.Overlays;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Furniture.AuroraMonoliths
{
	public class SpiritBiomeAuroraMonolith : AuroraMonolith
	{
        internal override int DropType => ModContent.ItemType<SpiritBiomeAuroraMonolithItem>();
        internal override int AuroraType => AuroraOverlay.SPIRIT;
    }

	public class SpiritBiomeAuroraMonolithItem : AuroraMonolithItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Spirit Biome Aurora Monolith");
		public override int PlaceType => ModContent.TileType<SpiritBiomeAuroraMonolith>();

        public override void SafeAddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 1);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddTile(TileID.CrystalBall);
			recipe.Register();
		}
	}
}