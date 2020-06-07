using Microsoft.Xna.Framework;
using SpiritMod.Backgrounds;
using SpiritMod.Dusts;
using SpiritMod.Gores;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Waters.Reach
{
	public class ReachWaterStyle : ModWaterStyle
	{
		public override bool ChooseWaterStyle()
		{
			return Main.bgStyle == mod.GetSurfaceBgStyleSlot<ReachSurfaceBgStyle>();
		}

		public override int ChooseWaterfallStyle()
		{
			return mod.GetWaterfallStyleSlot<ReachWaterfallStyle>();
		}

		public override int GetSplashDust()
		{
			return ModContent.DustType<ReachWaterSplash>();
		}

		public override int GetDropletGore()
		{
			return mod.GetGoreSlot<ReachDroplet>();
		}

		public override void LightColorMultiplier(ref float r, ref float g, ref float b)
		{
			r = 1f;
			g = 1f;
			b = 1f;
		}

		public override Color BiomeHairColor()
		{
			return Color.Green;
		}
	}
}