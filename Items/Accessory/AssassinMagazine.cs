using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class AssassinMagazine : ModItem
	{
		public override void SetStaticDefaults()
		{
			string tapDir = Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.DOWN" : "Key.UP");
			DisplayName.SetDefault("Assassin's Magazine");
			Tooltip.SetDefault($"Double tap {tapDir} while holding a ranged weapon to swap ammo types\nWorks while in the inventory");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string down = Main.ReversedUpDownArmorSetBonuses ? "UP" : "DOWN";

			foreach (TooltipLine line in tooltips)
			{
				if (line.mod == "Terraria" && line.Name == "Tooltip0")
					line.text = line.text.Replace("{0}", down);
			}
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}

		public override void UpdateInventory(Player player) => player.GetSpiritPlayer().assassinMag = true;
		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetSpiritPlayer().assassinMag = true;
	}
}
