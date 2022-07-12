using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace SpiritMod.Biomes
{
	internal class SpiritSurfaceBiome : ModBiome
	{
		public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("SpiritMod/SpiritWaterStyle");
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("SpiritMod/SpiritSurfaceBgStyle");
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Mushroom;

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/SpiritOverworld");

		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => MapBackground;
		public override Color? BackgroundColor => base.BackgroundColor;
		public override string MapBackground => "SpiritMod/Backgrounds/SpiritMapBackground";

		public override bool IsBiomeActive(Player player)
		{
			bool enoughTiles = ModContent.GetInstance<BiomeTileCounts>().spiritCount >= 80;
			bool surface = player.ZoneSkyHeight || player.ZoneOverworldHeight;
			return enoughTiles && surface;
		}

		public override void OnInBiome(Player player)
		{
			if (player.position.Y / 16 >= Main.maxTilesY - 330)
			{
				SpiritMod.glitchEffect.Parameters["Speed"].SetValue(0.2f); //0.4f is default
				SpiritMod.glitchScreenShader.UseIntensity(0.004f);
				player.ManageSpecialBiomeVisuals("SpiritMod:Glitch", true);
			}
			else if (player.ZoneRockLayerHeight && player.position.Y / 16 > (Main.rockLayer + Main.maxTilesY - 330) / 2f)
			{
				SpiritMod.glitchEffect.Parameters["Speed"].SetValue(0.1f); //0.4f is default
				SpiritMod.glitchScreenShader.UseIntensity(0.0005f);
				player.ManageSpecialBiomeVisuals("SpiritMod:Glitch", true);
			}
		}

		public override void OnEnter(Player player) => player.GetSpiritPlayer().ZoneSpirit = true;
		public override void OnLeave(Player player) => player.GetSpiritPlayer().ZoneSpirit = false;
	}
}
