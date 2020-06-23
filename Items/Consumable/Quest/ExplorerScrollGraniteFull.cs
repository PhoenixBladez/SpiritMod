
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Consumable.Quest
{
    public class ExplorerScrollGraniteFull : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Completed Surveyor's Scroll");
        }


        public override void SetDefaults() {
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
            TooltipLine line1 = new TooltipLine(mod, "FavoriteDesc", "A nearby Granite Cavern has been charted!");
            line1.overrideColor = new Color(255, 255, 255);
            tooltips.Add(line1);
        }
	}
}
