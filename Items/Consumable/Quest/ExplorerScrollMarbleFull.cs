
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Quest
{
	public class ExplorerScrollMarbleFull : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Completed Surveyor's Scroll");
		}


		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = -11;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(mod, "ItemName", "Quest Item");
			line.overrideColor = new Color(100, 222, 122);
			tooltips.Add(line);
			TooltipLine line1 = new TooltipLine(mod, "FavoriteDesc", "A nearby Marble Cavern has been charted!");
			line1.overrideColor = new Color(255, 255, 255);
			tooltips.Add(line1);
		}
	}
}
