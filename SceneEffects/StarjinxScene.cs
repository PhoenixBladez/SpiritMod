using SpiritMod.NPCs.StarjinxEvent;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.SceneEffects
{
	internal class StarjinxScene : ModSceneEffect
	{
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Starjinx");
		public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;
		public override bool IsSceneEffectActive(Player player) => player.GetModPlayer<StarjinxPlayer>().zoneStarjinxEvent;
	}
}
