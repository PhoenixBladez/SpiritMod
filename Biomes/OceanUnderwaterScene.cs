using SpiritMod.Utilities;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Biomes
{
	internal class OceanUnderwaterScene : ModSceneEffect
	{
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("SpiritMod/OceanDepthsBGStyle");
		  
		public override int Music => ModContent.GetInstance<SpiritMusicConfig>().UnderwaterMusic ? MusicLoader.GetMusicSlot(Mod, "Sounds/Music/UnderwaterMusic") : -1;
		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
		public override bool IsSceneEffectActive(Player player) => player.ZoneBeach && !MyWorld.luminousOcean && (player.GetSpiritPlayer().Submerged(30) || player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight) && DetectThoriumCompat(player);
		private bool DetectThoriumCompat(Player player) => !ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod) || thoriumMod.Call("GetZoneAquaticDepths", player) is bool inDepths && !inDepths;
	}
}
