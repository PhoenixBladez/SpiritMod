using Microsoft.Xna.Framework;
using SpiritMod.Backgrounds;
using SpiritMod.Dusts;
using SpiritMod.Gores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Effects.Waters
{
	public class OceanWaterStyle : ModWaterStyle
	{
		public override bool ChooseWaterStyle() => Main.LocalPlayer.ZoneBeach;
		public override int ChooseWaterfallStyle() => 0;
		public override int GetSplashDust() => DustID.Water;
		public override int GetDropletGore() => 706;
		public override Color BiomeHairColor() => Color.Blue;

		public override void LightColorMultiplier(ref float r, ref float g, ref float b)
		{
			const float Multiplier = 1.07f;

			r = Multiplier;
			g = Multiplier;
			b = Multiplier;
		}
	}
}