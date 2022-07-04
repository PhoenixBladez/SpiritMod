using SpiritMod.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.SceneEffects
{
	internal class AuroraScene : ModSceneEffect
	{
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/AuroraSnow");
		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
		public override bool IsSceneEffectActive(Player player) => ModContent.GetInstance<SpiritMusicConfig>().AuroraMusic
				&& MyWorld.aurora
				&& player.ZoneSnow && player.ZoneOverworldHeight
				&& !player.ZoneCorrupt && !player.ZoneCrimson && !player.ZoneMeteor
				&& !Main.bloodMoon && !Main.dayTime;
	}
}
