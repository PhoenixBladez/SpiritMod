using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace SpiritMod.Utilities
{
	class SpiritClientConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Screen Distortion")]
        [Tooltip("Enables screen distortion while in the Spirit Biome or when fighting the Starplate Voyager")]
        [DefaultValue(true)]
        public bool DistortionConfig { get; set; }

		[Label("Extra Particles")]
		[Tooltip("Enables extra particles in the foreground under certain conditions")]
		[DefaultValue(true)]
		public bool Particles { get; set; }

		[Label("Quick Sell Feature")]
		[Tooltip("Enables the quick-sell feature, which allows for easily selling all unwanted items to NPCs")]
		[DefaultValue(true)]
		public bool QuickSell { get; set; }

		[Label("Auto-reuse Tooltip")]
		[Tooltip("Enables an extra line in weapon tooltips displaying if an item is auto-reuse/autoswing")]
		[DefaultValue(true)]
		public bool AutoReuse { get; set; }

        [Label("Ambient Sounds")]
        [Tooltip("Enables ambient sound effects for some biomes")]
        [DefaultValue(true)]
        public bool AmbientSounds { get; set; }

        [Label("New Blizzard Music")]
		[Tooltip("Adds a unique track for Blizzards")]
		[DefaultValue(true)]
		public bool BlizzardMusic { get; set; }

		[Label("New Snow Night Music")]
		[Tooltip("Adds a unique track for the Snow biome at night")]
		[DefaultValue(true)]
		public bool SnowNightMusic { get; set; }

		[Label("New Desert Night Music")]
		[Tooltip("Adds a unique track for the Desert biome at night")]
		[DefaultValue(true)]
		public bool DesertNightMusic { get; set; }

        [Label("New Hallow Night Music")]
        [Tooltip("Adds a unique track for The Hallow at night")]
        [DefaultValue(true)]
        public bool HallowNightMusic { get; set; }

        [Label("New Granite Music")]
		[Tooltip("Adds a unique track for the Granite biome")]
		[DefaultValue(true)]
		public bool GraniteMusic { get; set; }

		[Label("New Marble Music")]
		[Tooltip("Adds a unique track for the Marble biome")]
		[DefaultValue(true)]
		public bool MarbleMusic { get; set; }

        [Label("New Spider Cavern Music")]
        [Tooltip("Adds a unique track for Spider caverns")]
        [DefaultValue(true)]
        public bool SpiderCaveMusic { get; set; }

        [Label("New Frost Legion Music")]
        [Tooltip("Adds a unique track for the Frost Legion")]
        [DefaultValue(true)]
        public bool FrostLegionMusic { get; set; }

        [Label("New Meteor Music")]
        [Tooltip("Adds a unique track for the Meteor")]
        [DefaultValue(true)]
        public bool MeteorMusic { get; set; }

        [Label("New Corrupt Night Music")]
        [Tooltip("Adds a unique track for the Corruption at nighttime")]
        [DefaultValue(true)]
        public bool CorruptNightMusic { get; set; }

		[Label("Aurora Music")]
		[Tooltip("Enables unique music for Auroras")]
		[DefaultValue(true)]
		public bool AuroraMusic { get; set; }

		[Label("Luminous Ocean Music")]
		[Tooltip("Enables unique music for Luminous Oceans")]
		[DefaultValue(true)]
		public bool LuminousMusic { get; set; }

        [Label("Calm Night Music")]
        [Tooltip("Enables unique music for Calm Nights")]
        [DefaultValue(true)]
        public bool CalmNightMusic { get; set; }

        [Label("Hyperspace Biome Music")]
        [Tooltip("Enables two unique tracks while in Hyperspace")]
        [DefaultValue(true)]
        public bool NeonBiomeMusic { get; set; }

	}
}