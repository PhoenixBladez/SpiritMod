using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Biomes
{
	internal class BriarUndergrundBiome : ModBiome
	{
		//public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.Find<ModUndergroundBackgroundStyle>("SpiritMod/Biomes/SpiritUgBgStyle");

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ReachUnderground");

		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => base.BackgroundColor;

		public override bool IsBiomeActive(Player player) => (player.ZoneRockLayerHeight || player.ZoneDirtLayerHeight) && ModContent.GetInstance<BiomeTileCounts>().spiritCount >= 80;
	}
}
