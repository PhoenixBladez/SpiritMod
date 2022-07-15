using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace SpiritMod.Biomes
{
	internal class AsteroidBiome : ModBiome
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Asteroids");
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Asteroids");
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => MapBackground;
		public override Color? BackgroundColor => base.BackgroundColor;
		public override string MapBackground => "SpiritMod/Backgrounds/AsteroidMapBG";

		public override bool IsBiomeActive(Player player)
		{
			bool enoughTiles = ModContent.GetInstance<BiomeTileCounts>().asteroidCount >= 40;
			bool surface = player.ZoneSkyHeight || player.ZoneOverworldHeight;
			return enoughTiles && surface;
		}

		public override void OnEnter(Player player) => player.GetSpiritPlayer().ZoneAsteroid = true;
		public override void OnLeave(Player player) => player.GetSpiritPlayer().ZoneAsteroid = false;
	}
}
