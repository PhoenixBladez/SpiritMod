using Microsoft.Xna.Framework;
using SpiritMod.Backgrounds;
using SpiritMod.Dusts;
using SpiritMod.Gores;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Effects.Waters.Reach
{
	public class ReachWaterStyle : ModWaterStyle
	{
		public override bool ChooseWaterStyle() => Main.bgStyle == mod.GetSurfaceBgStyleSlot<ReachSurfaceBgStyle>() || Main.LocalPlayer.GetSpiritPlayer().fountainsActive["BRIAR"] > 0;
		public override int ChooseWaterfallStyle() => mod.GetWaterfallStyleSlot<ReachWaterfallStyle>();
		public override int GetSplashDust() => ModContent.DustType<ReachWaterSplash>();
		public override int GetDropletGore() => mod.GetGoreSlot<ReachDroplet>();
		public override Color BiomeHairColor() => Color.Green;

		public override void LightColorMultiplier(ref float r, ref float g, ref float b)
		{
			r = 1f;
			g = 1f;
			b = 1f;
		}
	}
}