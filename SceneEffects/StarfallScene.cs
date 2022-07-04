using SpiritMod.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.SceneEffects
{
	internal class StarfallScene : ModSceneEffect
	{
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Starfall");
		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
		public override bool IsSceneEffectActive(Player player) => MyWorld.rareStarfallEvent && !MyWorld.jellySky && !player.GetSpiritPlayer().ZoneAsteroid && !Main.dayTime && player.ZoneSkyHeight;
	}
}
