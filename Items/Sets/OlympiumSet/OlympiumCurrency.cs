using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.Localization;

namespace SpiritMod.Items.Sets.OlympiumSet
{
	class OlympiumCurrency : CustomCurrencySingleCoin
	{
		public static readonly Color TextColour = Color.Goldenrod;

		public OlympiumCurrency(int coinItemID, long currencyCap) : base(coinItemID, currencyCap) { }

		public override void GetPriceText(string[] lines, ref int currentLine, int price)
		{
			Color color = Color.Gold * (Main.mouseTextColor / 255f);
			lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3} {4} {5}]",
								color.R,
								color.G,
								color.B,
								Language.GetTextValue("LegacyTooltip.50"),
								price,
								price > 1 ? "Olympium Tokens" : "Olympium Tokens"
							);
		}
	}
}
