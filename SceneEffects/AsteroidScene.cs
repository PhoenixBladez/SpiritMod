using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.SceneEffects
{
	internal class AsteroidScene : ModSceneEffect
	{
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Asteroids");
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
		public override bool IsSceneEffectActive(Player player) => player.GetSpiritPlayer().ZoneAsteroid;
	}
}
