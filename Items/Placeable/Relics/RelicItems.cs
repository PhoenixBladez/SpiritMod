using SpiritMod.Tiles.Relics;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Relics
{
	internal class MJWRelicItem : BaseRelicItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Moon Jelly Relic");
		internal override int TileType => ModContent.TileType<MJWRelic>();
	}

	internal class VinewrathRelicItem : BaseRelicItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Vinewrath Bane Relic");
		internal override int TileType => ModContent.TileType<VinewrathRelic>();
	}

	internal class OccultistRelicItem : BaseRelicItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Occultist Relic");
		internal override int TileType => ModContent.TileType<OccultistRelic>();
	}
}
