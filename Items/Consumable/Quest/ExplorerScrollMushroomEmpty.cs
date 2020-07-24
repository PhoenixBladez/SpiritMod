
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Quest
{
	public class ExplorerScrollMushroomEmpty : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Empty Surveyor's Scroll");

		}


		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.rare = -11;
		}
		public override void UpdateInventory(Player player)
		{
			if (player.GetSpiritPlayer().explorerTimer > 0 && player.ZoneGlowshroom) {
				item.SetNameOverride("Empty Surveyor's Scroll: " + (int)(player.GetSpiritPlayer().explorerTimer / 900f * 100) + "% Charted");
			}
			player.GetSpiritPlayer().emptyExplorerScroll = true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(mod, "ItemName", "Quest Item");
			line.overrideColor = new Color(100, 222, 122);
			tooltips.Add(line);
			TooltipLine line1 = new TooltipLine(mod, "FavoriteDesc", "Explore and map out the following area:");
			line1.overrideColor = new Color(255, 255, 255);
			tooltips.Add(line1);
			TooltipLine line2 = new TooltipLine(mod, "SocialDesc", "Glowing Mushroom Caverns");
			line2.overrideColor = new Color(96, 125, 240);
			tooltips.Add(line2);
		}
	}
}
