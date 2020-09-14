
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Quest
{
	public class BriarSlayerScrollEmpty : ModItem
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
			if (MyWorld.numBriarMobsKilled > 0) {
				item.SetNameOverride("Incomplete Slayer's Contract: " + MyWorld.numBriarMobsKilled + "/10 Complete");
			}
			player.GetSpiritPlayer().emptyBriarMobsScroll = true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(mod, "ItemName", "Quest Item");
			line.overrideColor = new Color(100, 222, 122);
			tooltips.Add(line);
			TooltipLine line1 = new TooltipLine(mod, "FavoriteDesc", "Kill 10 of the following enemies:");
			line1.overrideColor = new Color(255, 255, 255);
			tooltips.Add(line1);
			TooltipLine line2 = new TooltipLine(mod, "SocialDesc", "Blossom Hound, Thorn Stalker");
			line2.overrideColor = new Color(92, 150, 69);
			tooltips.Add(line2);
		}
	}
}
