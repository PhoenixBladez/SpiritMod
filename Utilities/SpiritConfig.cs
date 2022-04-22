using SpiritMod.World;
using System.ComponentModel;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ModLoader.Config;

namespace SpiritMod.Utilities
{
	[Label("Client Config")]
	class SpiritClientConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Label("$Mods.SpiritMod.Screenshake")]
		[Tooltip("Modifies the intensity of screenshake applied by content within the mod\nSet to 0 to completely disable")]
		[Range(0f, 1f)]
		[Increment(.01f)]
		[DefaultValue(1f)]
		[Slider]
		public float ScreenShake { get; set; }

		[Label("$Mods.SpiritMod.Distortion")]
        [Tooltip("Enables screen distortion while in the Spirit Biome or when fighting the Starplate Voyager")]
        [DefaultValue(true)]
        public bool DistortionConfig { get; set; }

		[Label("$Mods.SpiritMod.Particles")]
		[Tooltip("Enables extra particles in the foreground under certain conditions")]
		[DefaultValue(true)]
		public bool ForegroundParticles { get; set; }

		[Label("$Mods.SpiritMod.Autoswing")]
		[Tooltip("Enables an extra line in weapon tooltips displaying if an item is auto-reuse/autoswing")]
		[DefaultValue(true)]
		public bool AutoReuse { get; set; }

		[Label("$Mods.SpiritMod.Quicksell")]
		[Tooltip("Enables the quick-sell feature, which allows for easily selling all unwanted items to NPCs")]
		[DefaultValue(true)]
		public bool QuickSell { get; set; }

        [Label("$Mods.SpiritMod.AmbientSounds")]
        [Tooltip("Enables ambient sound effects for some biomes")]
        [DefaultValue(true)]
        public bool AmbientSounds { get; set; }

		[Label("$Mods.SpiritMod.LeafFallAmbience")]
		[Tooltip("Enables falling leaf ambience during Calm Nights or while in the Briar\nDisable this to prevent strange interactions while playing Terraria Overhaul")]
		[DefaultValue(true)]
		public bool LeafFall { get; set; }

		[Label("$Mods.SpiritMod.QuestButton")]
		[Tooltip("Change where the quest book's button appears in your inventory")]
		[DefaultValue(QuestUtils.QuestInvLocation.Minimap)]
		[DrawTicks]
		public QuestUtils.QuestInvLocation QuestBookLocation { get; set; }

		/*[Label("Town NPC Portraits")]
		[Tooltip("Enables the showing of NPC portraits when talking to a Town NPC")]
		[DefaultValue(true)]
		public bool ShowNPCPortraits { get; set; }*/

		[Label("$Mods.SpiritMod.QuestIcons")]
		[Tooltip("Enables the showing of quest exclamation marks when a Town NPC has a new quest")]
		[DefaultValue(true)]
		public bool ShowNPCQuestNotice { get; set; }

		[Label("$Mods.SpiritMod.ArcaneHideoutGen")]
		[Tooltip("Enables the generation of both the Bandit Hideout and Arcane Tower\nOnly one spawns per world by default\nRecommended only for large worlds")]
		[DefaultValue(false)]
		public bool DoubleHideoutGeneration { get; set; }

		[Label("$Mods.SpiritMod.OceanShape")]
		[Tooltip("Modifies the Ocean generation in certain ways\nDefaults to a Piecewise_V generation\nThe Piecewise_V generation is recommended for Spirit playthroughs")]
		[DefaultValue(OceanGeneration.OceanShape.Piecewise_V)]
		public OceanGeneration.OceanShape OceanShape { get; set; }


		[Label("$Mods.SpiritMod.OceanVents")]
		[Tooltip("Enables the spawning of numerous critters around Hydrothermal Vents\nThese critters do not interfere with regular enemy spawnrates\nHowever, they do contribute to the maximum NPC limit")]
		[DefaultValue(true)]
		public bool VentCritters { get; set; }

		public enum SurfaceTransparencyOption : int
		{
			Ocean,
			Disabled
		}

		[ReloadRequired]
		[Label("$Mods.SpiritMod.OceanWater")]
		[Tooltip("Allows ocean water to be fully transparent\nToggle to change the transparency of ocean water and underworld lava")]
		[DefaultValue(SurfaceTransparencyOption.Ocean)]
		public SurfaceTransparencyOption SurfaceWaterTransparency { get; set; }

		[ReloadRequired]
		[Label("$Mods.SpiritMod.WaterEnemies")]
		[Tooltip("Enables enemy encounters while fishing")]
		[DefaultValue(true)]
		public bool EnemyFishing { get; set; }

		[OnDeserialized]
		internal void OnDeserializedMethod(StreamingContext context) => ScreenShake = Utils.Clamp(ScreenShake, 0f, 1f);
	}
}