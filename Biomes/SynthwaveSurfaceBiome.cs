using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace SpiritMod.Biomes
{
	internal class SynthwaveSurfaceBiome : ModBiome
	{
		//public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("SpiritMod/Effects/Waters/Spirit/SpiritWaterStyle");
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("SpiritMod/Biomes/SynthwaveBGStyle");
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Mushroom;

		public override int Music => MusicLoader.GetMusicSlot(Mod, Main.dayTime ? "Sounds/Music/NeonTech1" : "Sounds/Music/NeonTech");

		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => base.BackgroundColor;
		public override string MapBackground => BackgroundPath;

		public override bool IsBiomeActive(Player player)
		{
			bool enoughTiles = ModContent.GetInstance<BiomeTileCounts>().spiritCount >= 80;
			bool surface = player.ZoneSkyHeight || player.ZoneOverworldHeight;
			return enoughTiles && surface;
		}
	}
}
