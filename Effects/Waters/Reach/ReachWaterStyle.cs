using Microsoft.Xna.Framework;
using SpiritMod.Dusts;
using SpiritMod.Gores;
using Terraria.ModLoader;

namespace SpiritMod.Effects.Waters.Reach
{
	public class ReachWaterStyle : ModWaterStyle
	{
		public override int ChooseWaterfallStyle() => ModContent.Find<ModWaterfallStyle>("SpiritMod/ReachWaterfallStyle").Slot;
		public override int GetSplashDust() => ModContent.DustType<ReachWaterSplash>();
		public override int GetDropletGore() => ModContent.GoreType<ReachDroplet>();
		public override Color BiomeHairColor() => Color.Green;

		public override void LightColorMultiplier(ref float r, ref float g, ref float b)
		{
			r = 1f;
			g = 1f;
			b = 1f;
		}
	}
}