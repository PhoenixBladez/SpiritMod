using SpiritMod.NPCs.Tides.Tide;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.SceneEffects
{
	internal class TideScene : ModSceneEffect
	{
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/DepthInvasion");
		public override SceneEffectPriority Priority => SceneEffectPriority.Event;
		public override bool IsSceneEffectActive(Player player) => TideWorld.TheTide && player.ZoneBeach;
	}
}
