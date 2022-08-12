using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
//using SpiritMod.Backgrounds;
using SpiritMod.Dusts;
using SpiritMod.Gores;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Effects.Waters.Spirit
{
	public class SpiritWaterStyle : ModWaterStyle
	{
		public override int ChooseWaterfallStyle() => ModContent.Find<ModWaterfallStyle>("SpiritMod/SpiritWaterfallStyle").Slot;
		public override int GetSplashDust() => ModContent.DustType<SpiritWaterSplash>();
		public override int GetDropletGore() => ModContent.GoreType<SpiritDroplet>();

		public override void LightColorMultiplier(ref float r, ref float g, ref float b)
		{
			r = 1f;
			g = 1f;
			b = 1f;
		}

		public override Color BiomeHairColor() => Color.Blue;
		public override byte GetRainVariant() => (byte)Main.rand.Next(3);
		public override Asset<Texture2D> GetRainTexture() => ModContent.Request<Texture2D>("SpiritMod/Effects/Waters/Spirit/SpiritRain");
	}
}