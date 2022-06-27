using SpiritMod.Effects.Waters.Reach;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Furniture.Fountains
{
	public class BriarFountain : BaseFountain
    {
		internal override int DropType => ModContent.ItemType<BriarFountainItem>();
		internal override int WaterStyle => ModContent.GetInstance<ReachWaterStyle>().Slot;
	}

	public class BriarFountainItem : BaseFountainItem
	{
		internal override int PlaceType => ModContent.TileType<BriarFountain>();

		public override void SetStaticDefaults() => DisplayName.SetDefault("Briar Water Fountain");
	}
}