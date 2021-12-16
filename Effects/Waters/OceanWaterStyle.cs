using Microsoft.Xna.Framework;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Effects.Waters
{
	public class OceanWaterStyle : ModWaterStyle
	{
		public override bool ChooseWaterStyle() => ModContent.GetInstance<SpiritClientConfig>().OceanWaterStyleEnabled && Main.LocalPlayer.ZoneBeach;
		public override int ChooseWaterfallStyle() => 0;
		public override int GetSplashDust() => DustID.Water;
		public override int GetDropletGore() => 706;
		public override Color BiomeHairColor() => Color.Blue;

		public override void LightColorMultiplier(ref float r, ref float g, ref float b)
		{
			const float Multiplier = 1.08f;

			r = Multiplier;
			g = Multiplier;
			b = Multiplier;
		}
	}
}