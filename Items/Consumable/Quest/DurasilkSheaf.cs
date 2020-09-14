using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;

namespace SpiritMod.Items.Consumable.Quest
{
	public class DurasilkSheaf : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Durasilk Sheaf");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.rare = -11;
			item.maxStack = 99;
		}
        public override bool OnPickup(Player player)
        {
            if (player.HasItem(ModContent.ItemType<Items.Consumable.Quest.DurasilkSheaf>()))
            {
                return false;
            }
            return true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(mod, "ItemName", "Quest Item");
			line.overrideColor = new Color(100, 222, 122);
			tooltips.Add(line);
			TooltipLine line1 = new TooltipLine(mod, "FavoriteDesc", "'The fabric is soft, but quite strong'");
			line1.overrideColor = new Color(255, 255, 255);
			tooltips.Add(line1);
		}
	}
}
