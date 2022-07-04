using SpiritMod.Utilities;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.SceneEffects
{
	internal class OceanUnderwaterScene : ModSceneEffect
	{
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/UnderwaterMusic");
		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
		public override bool IsSceneEffectActive(Player player) => ModContent.GetInstance<SpiritMusicConfig>().UnderwaterMusic && player.ZoneBeach && !MyWorld.luminousOcean && player.GetSpiritPlayer().isFullySubmerged && DetectThoriumCompat(player);
		private bool DetectThoriumCompat(Player player) => ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod) && thoriumMod.Call("GetZoneAquaticDepths", player) is bool inDepths && !inDepths;
	}
}
