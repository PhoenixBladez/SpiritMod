using SpiritMod.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.SceneEffects
{
	internal class BlizzardScene : ModSceneEffect
	{
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Blizzard");
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
		public override bool IsSceneEffectActive(Player player) => ModContent.GetInstance<SpiritMusicConfig>().BlizzardMusic
				&& player.ZoneSnow
				&& player.ZoneOverworldHeight
				&& !player.ZoneCorrupt
				&& !player.ZoneMeteor
				&& !player.ZoneCrimson
				&& Main.raining;
	}
}
