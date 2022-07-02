using SpiritMod.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.SceneEffects
{
	internal class SpiderScene : ModSceneEffect
	{
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/SpiderCave");
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
		public override bool IsSceneEffectActive(Player player) => ModContent.GetInstance<SpiritMusicConfig>().SpiderCaveMusic && player.GetSpiritPlayer().ZoneSpider 
			&& !player.ZoneHallow && !player.ZoneCorrupt && !player.ZoneCrimson && !player.ZoneOverworldHeight && !player.GetSpiritPlayer().ZoneSpirit;
	}
}
