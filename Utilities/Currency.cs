using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.Localization;

namespace SpiritMod.Utilities
{
	public class Currency : CustomCurrencySingleCoin
	{
		public Currency(int coinItemID, long currencyCap) : base(coinItemID, currencyCap)
		{
		}

		public override void GetPriceText(string[] lines, ref int currentLine, int price)
		{
			Color glyphColor = Color.Orange * (Main.mouseTextColor / 255f);
			lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3} {4} {5}]",
					glyphColor.R,
					glyphColor.G,
					glyphColor.B,
					Language.GetTextValue("LegacyTooltip.50"),
					price,
					price > 1 ? "glyphs" : "glyph"
				);
		}
	}
}
