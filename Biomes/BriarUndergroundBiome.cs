using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Biomes
{
	internal class BriarUndergroundBiome : ModBiome
	{
		//public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.Find<ModUndergroundBackgroundStyle>("SpiritMod/Biomes/SpiritUgBgStyle");
		public override void SetStaticDefaults() => DisplayName.SetDefault("Underground Briar");
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ReachUnderground");

		public override string BestiaryIcon => base.BestiaryIcon;
		public override Color? BackgroundColor => base.BackgroundColor;
		public override string BackgroundPath => MapBackground;
		public override string MapBackground => "SpiritMod/Backgrounds/BriarMapBG";

		public override bool IsBiomeActive(Player player) => (player.ZoneRockLayerHeight || player.ZoneDirtLayerHeight) && BiomeTileCounts.InBriar;
		public override void OnEnter(Player player) => player.GetSpiritPlayer().ZoneReach = true;
		public override void OnLeave(Player player) => player.GetSpiritPlayer().ZoneReach = false;
	}
}
