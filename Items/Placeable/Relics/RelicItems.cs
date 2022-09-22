using SpiritMod.Tiles.Relics;

namespace SpiritMod.Items.Placeable.Relics
{
	public class MJWRelicItem : BaseRelicItem<MJWRelic>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Moon Jelly Relic");
	}

	public class VinewrathRelicItem : BaseRelicItem<VinewrathRelic>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Vinewrath Bane Relic");
	}

	public class OccultistRelicItem : BaseRelicItem<OccultistRelic>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Occultist Relic");
	}

	public class ScarabeusRelicItem : BaseRelicItem<ScarabeusRelic>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Scarabeus Relic");
	}

	public class DuskingRelicItem : BaseRelicItem<DuskingRelic>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Dusking Relic");
	}

	public class FrostSaucerRelicItem : BaseRelicItem<FrostSaucerRelic>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Snow Monger Relic");
	}

	public class AvianRelicItem : BaseRelicItem<AvianRelic>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Avian Relic");
	}
}
