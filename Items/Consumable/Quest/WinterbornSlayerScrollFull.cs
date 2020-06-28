using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Quest
{
	public class WinterbornSlayerScrollFull : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Completed Slayer's Contract");

		}


		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.rare = -11;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(mod, "ItemName", "Quest Item");
			line.overrideColor = new Color(100, 222, 122);
			tooltips.Add(line);
			TooltipLine line1 = new TooltipLine(mod, "FavoriteDesc", "The Winterborn Slayer Contract has been completed!");
			line1.overrideColor = new Color(255, 255, 255);
			tooltips.Add(line1);
		}
	}
}
