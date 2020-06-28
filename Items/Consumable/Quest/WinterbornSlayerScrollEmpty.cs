
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Quest
{
	public class WinterbornSlayerScrollEmpty : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Incomplete Slayer's Contract");

		}


		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.rare = -11;
		}
		public override void UpdateInventory(Player player)
		{
			if(MyWorld.numWinterbornKilled > 0) {
				item.SetNameOverride("Incomplete Slayer's Contract: " + MyWorld.numWinterbornKilled + "/10 Complete");
			}
			player.GetSpiritPlayer().emptyWinterbornScroll = true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(mod, "ItemName", "Quest Item");
			line.overrideColor = new Color(100, 222, 122);
			tooltips.Add(line);
			TooltipLine line1 = new TooltipLine(mod, "FavoriteDesc", "Kill 10 of the following enemies:");
			line1.overrideColor = new Color(255, 255, 255);
			tooltips.Add(line1);
			TooltipLine line2 = new TooltipLine(mod, "SocialDesc", "Winterborn, Winterborn Herald");
			line2.overrideColor = new Color(135, 231, 255);
			tooltips.Add(line2);
		}
	}
}
