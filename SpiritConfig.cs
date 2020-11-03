using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace SpiritMod
{
	class SpiritClientConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

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

    }
}