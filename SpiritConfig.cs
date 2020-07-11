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
		public bool BlizzardMusic;

		[Label("New Snow Night Music")]
		[Tooltip("Adds a unique track for the Snow biome at night")]
		[DefaultValue(true)]
		public bool SnowNightMusic;

		[Label("New Desert Night Music")]
		[Tooltip("Adds a unique track for the Desert biome at night")]
		[DefaultValue(true)]
		public bool DesertNightMusic;

		[Label("New Granite Music")]
		[Tooltip("Adds a unique track for the Granite biome")]
		[DefaultValue(true)]
		public bool GraniteMusic;

		[Label("New Marble Music")]
		[Tooltip("Adds a unique track for the Marble biome")]
		[DefaultValue(true)]
		public bool MarbleMusic;

		[Label("Aurora Music")]
		[Tooltip("Enables unique music for Auroras")]
		[DefaultValue(true)]
		public bool AuroraMusic;

		[Label("Luminous Ocean Music")]
		[Tooltip("Enables unique music for Luminous Oceans")]
		[DefaultValue(true)]
		public bool LuminousMusic;
	}
}