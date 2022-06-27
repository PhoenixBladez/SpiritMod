using SpiritMod.Skies.Overlays;
using Terraria;
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
		public override void SetStaticDefaults() => DisplayName.SetDefault("Pumpkin Moon Aurora Monolith");
		public override int PlaceType => ModContent.TileType<PumpkinAuroraMonolith>();

        public override void SafeAddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SpookyWood, 10);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddTile(TileID.CrystalBall);
			recipe.Register();
		}
	}
}