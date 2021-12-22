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
			Color color = TextColour * (Main.mouseTextColor / 255f);
			lines[currentLine++] = $"[c/{color.R}{color.G}{color.B}:{Language.GetTextValue("LegacyTooltip.50")} {price} tokens]";
		}
	}
}
