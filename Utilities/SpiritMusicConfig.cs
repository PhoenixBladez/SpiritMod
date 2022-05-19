using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace SpiritMod.Utilities
{
	[Label("Music Config")]
	class SpiritMusicConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Label("$Mods.SpiritMod.BlizzardMusic")]
		[Tooltip("Adds a unique track for Blizzards")]
		[DefaultValue(true)]
		public bool BlizzardMusic { get; set; }

		[Label("$Mods.SpiritMod.SnowNightMusic")]
		[Tooltip("Adds a unique track for the Snow biome at night")]
		[DefaultValue(true)]
		public bool SnowNightMusic { get; set; }

		[Label("$Mods.SpiritMod.DesertNightMusic")]
		[Tooltip("Adds a unique track for the Desert biome at night")]
		[DefaultValue(true)]
		public bool DesertNightMusic { get; set; }

		[Label("$Mods.SpiritMod.HallowNightMusic")]
		[Tooltip("Adds a unique track for The Hallow at night")]
        [DefaultValue(true)]
        public bool HallowNightMusic { get; set; }

		[Label("$Mods.SpiritMod.CorruptNightMusic")]
		[Tooltip("Adds a unique track for the Corruption at nighttime")]
		[DefaultValue(true)]
		public bool CorruptNightMusic { get; set; }

		[Label("$Mods.SpiritMod.CrimsonNightMusic")]
		[Tooltip("Adds a unique track for the Crimson at nighttime")]
		[DefaultValue(true)]
		public bool CrimsonNightMusic { get; set; }

		[Label("$Mods.SpiritMod.GraniteMusic")]
		[Tooltip("Adds a unique track for the Granite biome")]
		[DefaultValue(true)]
		public bool GraniteMusic { get; set; }

		[Label("$Mods.SpiritMod.MarbleMusic")]
		[Tooltip("Adds a unique track for the Marble biome")]
		[DefaultValue(true)]
		public bool MarbleMusic { get; set; }

		[Label("$Mods.SpiritMod.OceanDepthsMusic")]
		[Tooltip("Enables unique music for the Ocean while deep underwater")]
		[DefaultValue(true)]
		public bool UnderwaterMusic { get; set; }

		[Label("$Mods.SpiritMod.SpiderMusic")]
		[Tooltip("Adds a unique track for Spider caverns")]
        [DefaultValue(true)]
        public bool SpiderCaveMusic { get; set; }

		[Label("$Mods.SpiritMod.MeteorMusic")]
		[Tooltip("Adds a unique track for the Meteorite biome")]
		[DefaultValue(true)]
		public bool MeteorMusic { get; set; }

		[Label("$Mods.SpiritMod.FrostLegionMusic")]
		[Tooltip("Adds a unique track for the Frost Legion")]
        [DefaultValue(true)]
        public bool FrostLegionMusic { get; set; }

		[Label("$Mods.SpiritMod.SkeletonPrimeMusic")]
		[Tooltip("Adds a unique boss soundtrack for Skeletron Prime")]
		[DefaultValue(true)]
		public bool SkeletronPrimeMusic { get; set; }

		[Label("$Mods.SpiritMod.AuroraMusic")]
		[Tooltip("Enables unique music for Auroras")]
		[DefaultValue(true)]
		public bool AuroraMusic { get; set; }

		[Label("$Mods.SpiritMod.LuminousMusic")]
		[Tooltip("Enables unique music for Luminous Oceans")]
		[DefaultValue(true)]
		public bool LuminousMusic { get; set; }

		[Label("$Mods.SpiritMod.CalmNightMusic")]
		[Tooltip("Enables unique music for Calm Nights")]
        [DefaultValue(true)]
        public bool CalmNightMusic { get; set; }

		[Label("$Mods.SpiritMod.HyperspaceMusic")]
		[Tooltip("Enables two unique tracks while in Hyperspace")]
        [DefaultValue(true)]
        public bool NeonBiomeMusic { get; set; }

		[Label("$Mods.SpiritMod.AshfallMusic")]
		[Tooltip("Enables a unique track for the Ashfall weather in the Underworld")]
		[DefaultValue(true)]
		public bool AshfallMusic { get; set; }
	}
}