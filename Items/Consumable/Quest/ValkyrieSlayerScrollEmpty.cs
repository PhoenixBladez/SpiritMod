
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpiritMod;

namespace SpiritMod.Items.Consumable.Quest
{
    public class ValkyrieSlayerScrollEmpty : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Incomplete Slayer's Contract");

        }


        public override void SetDefaults() {
            item.width = 30;
            item.height = 20;
            item.rare = -11;
        }
        public override void UpdateInventory(Player player)
        {
            player.GetSpiritPlayer().emptyValkyrieScroll = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = new TooltipLine(mod, "ItemName", "Quest Item");
            line.overrideColor = new Color(100, 222, 122);
            tooltips.Add(line);
            TooltipLine line1 = new TooltipLine(mod, "FavoriteDesc", "Kill 1 of the following enemy:");
            line1.overrideColor = new Color(255, 255, 255);
            tooltips.Add(line1);
            TooltipLine line2 = new TooltipLine(mod, "SocialDesc", "Valkyrie");
            line2.overrideColor = new Color(255, 227, 156);
            tooltips.Add(line2);
        }
    }
}
