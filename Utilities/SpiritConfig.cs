using SpiritMod.World;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace SpiritMod.Utilities
{
	[Label("Client Config")]
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

		[Label("Quest Book Button")]
		[Tooltip("Change where the quest book's button appears in your inventory.")]
		[DefaultValue(QuestUtils.QuestInvLocation.Minimap)]
		[DrawTicks]
		public QuestUtils.QuestInvLocation QuestBookLocation { get; set; }

		[Label("Town NPC Portraits")]
		[Tooltip("Enables the showing of NPC portraits when talking to a Town NPC.")]
		[DefaultValue(true)]
		public bool ShowNPCPortraits { get; set; }

		[Label("Town NPC Quest Notice")]
		[Tooltip("Enables the showing of quest exclamation marks when a Town NPC has a new quest.")]
		[DefaultValue(true)]
		public bool ShowNPCQuestNotice { get; set; }

		[Label("Fishing Encounters")]
		[Tooltip("Enables enemy encounters while fishing")]
		[DefaultValue(true)]
		public bool EnemyFishing { get; set; }

		[Label("Bandit Hideout and Arcane Tower Generation")]
		[Tooltip("Enables the generation of both the Bandit Hideout and Arcane Tower. Only one spawns per world by default.\nRecommended only for large worlds.")]
		[DefaultValue(false)]
		public bool DoubleHideoutGeneration { get; set; }

		[Label("Falling Leaf Ambience")]
		[Tooltip("Enables falling leaf ambience during Calm Nights or while in the Briar\nDisable this to prevent strange interactions while playing Terraria Overhaul.")]
		[DefaultValue(true)]
		public bool LeafFall { get; set; }

		[Label("Boss Titles")]
		[Tooltip("Enables the showing of titles when spawning a boss, or what bosses titles display for")]
		[OptionStrings(new string[] { "Off", "Spirit Bosses Only", "Spirit and Vanilla Bosses Only", "All Applicable Bosses" })]
		[DefaultValue("Spirit and Vanilla Bosses Only")]
		public string DrawCondition { get; set; }

		[Label("Ocean Generation Shape")]
		[Tooltip("Modifies the Ocean generation in certain ways. Defaults to a Piecewise_V generation - this is the intended shape, and changing this is only meant for novelty.")]
		[DefaultValue(OceanGeneration.OceanShape.Piecewise_V)]
		public OceanGeneration.OceanShape OceanShape { get; set; }

		public enum SurfaceTransparencyOption : int
		{
			Ocean,
			Underworld,
			Both,
			Disabled
		}

		[ReloadRequired]
		[Label("Transparent Surface Water")]
		[Tooltip("Allows ocean water to be fully transparent")]
		[DefaultValue(SurfaceTransparencyOption.Ocean)]
		public SurfaceTransparencyOption SurfaceWaterTransparency { get; set; }
	}
}