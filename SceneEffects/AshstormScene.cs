using SpiritMod.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.SceneEffects
{
	internal class AshstormScene : ModSceneEffect
	{
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/AshStorm");
		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
		public override bool IsSceneEffectActive(Player player) => MyWorld.ashRain && player.ZoneUnderworldHeight && ModContent.GetInstance<SpiritMusicConfig>().AshfallMusic;
	}
}
