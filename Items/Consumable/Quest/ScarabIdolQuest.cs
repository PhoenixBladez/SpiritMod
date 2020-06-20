using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Consumable.Quest
{
    public class ScarabIdolQuest : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Decrepit Idol");
        }


        public override void SetDefaults() {
            item.width = item.height = 16;
            item.rare = -11;
            item.maxStack = 99;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = new TooltipLine(mod, "ItemName", "Quest Item");
            line.overrideColor = new Color(100, 222, 122);
            tooltips.Add(line);
            TooltipLine line1 = new TooltipLine(mod, "FavoriteDesc", "'It's an ancient artifact that resembles a scarab beetle'");
            line1.overrideColor = new Color(255, 255, 255);
            tooltips.Add(line1);
        }
	}
}
