using Microsoft.Xna.Framework;
using SpiritMod.Backgrounds;
using SpiritMod.Dusts;
using SpiritMod.Gores;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Effects.Waters.Spirit
{
	public class SpiritWaterStyle : ModWaterStyle
	{
		public override bool ChooseWaterStyle()
		{
			return Main.bgStyle == Mod.GetSurfaceBgStyleSlot<SpiritSurfaceBgStyle>();
		}

		public override int ChooseWaterfallStyle()
		{
			return Mod.GetWaterfallStyleSlot<SpiritWaterfallStyle>();
		}

		public override int GetSplashDust()
		{
			return ModContent.DustType<SpiritWaterSplash>();
		}

		public override int GetDropletGore()
		{
			return Mod.GetGoreSlot<SpiritDroplet>();
		}

		public override void LightColorMultiplier(ref float r, ref float g, ref float b)
		{
			r = 1f;
			g = 1f;
			b = 1f;
		}

		public override Color BiomeHairColor()
		{
			return Color.Blue;
		}
	}
}