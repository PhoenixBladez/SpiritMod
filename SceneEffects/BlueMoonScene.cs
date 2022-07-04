using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.SceneEffects
{
	internal class BlueMoonScene : ModSceneEffect
	{
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/BlueMoon");
		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
		public override bool IsSceneEffectActive(Player player) => player.GetSpiritPlayer().ZoneBlueMoon && !Main.dayTime && (player.ZoneOverworldHeight || player.ZoneSkyHeight);
	}
}
