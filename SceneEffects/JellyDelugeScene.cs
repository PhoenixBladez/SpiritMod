using SpiritMod.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.SceneEffects
{
	internal class JellyDelugeScene : ModSceneEffect
	{
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/JellySky");
		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
		public override bool IsSceneEffectActive(Player player) => MyWorld.jellySky && !Main.dayTime && (player.ZoneOverworldHeight || player.ZoneSkyHeight);
	}
}
