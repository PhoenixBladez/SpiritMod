using SpiritMod.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.SceneEffects
{
	internal class CalmNightScene : ModSceneEffect
	{
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/CalmNight");
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
		public override bool IsSceneEffectActive(Player player) => ModContent.GetInstance<SpiritMusicConfig>().CalmNightMusic
				&& MyWorld.calmNight
				&& !player.ZoneSnow
				&& !player.GetSpiritPlayer().ZoneReach
				&& player.ZoneOverworldHeight
				&& !Main.dayTime
				&& !player.ZoneCorrupt
				&& !player.ZoneCrimson
				&& !player.ZoneJungle
				&& !player.ZoneBeach
				&& !player.ZoneHallow
				&& !player.ZoneMeteor
				&& !player.ZoneDesert
				&& !Main.raining
				&& !Main.bloodMoon;
	}
}
