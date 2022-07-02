using SpiritMod.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.SceneEffects
{
	internal class NightThemeScene : ModSceneEffect
	{
		private SpiritMusicConfig config => ModContent.GetInstance<SpiritMusicConfig>();
		private Player player => Main.LocalPlayer;

		private bool ValidCorruption => config.CorruptNightMusic && player.ZoneCorrupt && player.ZoneOverworldHeight
				&& !Main.dayTime && !player.ZoneHallow && !player.ZoneMeteor && !Main.bloodMoon;
		private bool ValidOcean => config.LuminousMusic && player.ZoneBeach && MyWorld.luminousOcean && !Main.dayTime;
		private bool ValidHallow => config.HallowNightMusic && player.ZoneHallow && player.ZoneOverworldHeight && !Main.dayTime && !Main.raining && !Main.bloodMoon
				&& !player.ZoneCorrupt && !player.ZoneCrimson && !player.ZoneJungle && !player.ZoneBeach && !player.ZoneMeteor;
		private bool ValidCrimson => config.CrimsonNightMusic && player.ZoneCrimson && player.ZoneOverworldHeight && !Main.dayTime
				&& !player.ZoneHallow && !player.ZoneMeteor && !Main.bloodMoon;
		private bool ValidSnow => config.SnowNightMusic && player.ZoneSnow && player.ZoneOverworldHeight && !Main.dayTime && !player.ZoneCorrupt
				&& !player.ZoneMeteor && !player.ZoneCrimson && !player.ZoneHallow && !MyWorld.aurora && !Main.raining && !Main.bloodMoon;
		private bool ValidDesert => config.DesertNightMusic && player.ZoneDesert && player.ZoneOverworldHeight && !Main.dayTime && !player.ZoneCorrupt
				&& !player.ZoneCrimson && !player.ZoneBeach;

		public override int Music => GetMusic();

		public int GetMusic()
		{
			int music = -1;

			if (ValidOcean)
				music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/OceanNighttime");

			if (ValidHallow)
				music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/HallowNight");

			if (ValidCorruption)
				music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/CorruptNight");

			if (ValidCrimson)
				music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/CrimsonNight");

			if (ValidSnow)
				music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/SnowNighttime");

			if (ValidDesert)
				music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/DesertNighttime");
			return music;
		}

		public override SceneEffectPriority Priority => ValidCorruption || ValidCrimson ? SceneEffectPriority.BiomeHigh : SceneEffectPriority.BiomeMedium;
		public override bool IsSceneEffectActive(Player player) => ValidOcean || ValidHallow || ValidCrimson || ValidCorruption || ValidSnow || ValidDesert;
	}
}
