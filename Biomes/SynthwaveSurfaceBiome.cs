using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace SpiritMod.Biomes
{
	internal class SynthwaveSurfaceBiome : ModBiome
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Synthwave");
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("SpiritMod/SynthwaveBGStyle");
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Mushroom;
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;

		public override int Music => MusicLoader.GetMusicSlot(Mod, Main.dayTime ? "Sounds/Music/NeonTech1" : "Sounds/Music/NeonTech");

		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => base.BackgroundColor;
		public override string MapBackground => BackgroundPath;

		public override bool IsBiomeActive(Player player)
		{
			bool surface = player.ZoneSkyHeight || player.ZoneOverworldHeight;
			return ModContent.GetInstance<BiomeTileCounts>().inSynthwave && surface;
		}

		public override void OnEnter(Player player) => player.GetSpiritPlayer().ZoneSynthwave = true;
		public override void OnLeave(Player player) => player.GetSpiritPlayer().ZoneSynthwave = false;
	}
}
