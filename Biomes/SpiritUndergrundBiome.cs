﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Biomes
{
	internal class SpiritUndergrundBiome : ModBiome
	{
		public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.Find<ModUndergroundBackgroundStyle>("SpiritMod/Biomes/SpiritUgBgStyle");

		public override int Music => GetMusicFromDepth();

		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => base.BackgroundColor;

		private int GetMusicFromDepth()
		{
			Player player = Main.LocalPlayer;
			int music = -1;

			if (player.ZoneRockLayerHeight && player.position.Y / 16 < (Main.rockLayer + Main.maxTilesY - 330) / 2f)
				music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/SpiritLayer1");
			if (player.ZoneRockLayerHeight && player.position.Y / 16 > (Main.rockLayer + Main.maxTilesY - 330) / 2f)
				music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/SpiritLayer2");
			if (player.position.Y / 16 >= Main.maxTilesY - 330)
				music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/SpiritLayer3");
			return music;
		}

		public override bool IsBiomeActive(Player player) => (player.ZoneRockLayerHeight || player.ZoneDirtLayerHeight) && ModContent.GetInstance<BiomeTileCounts>().spiritCount >= 80;
	}
}