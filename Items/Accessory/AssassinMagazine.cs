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
			Tooltip.SetDefault($"Double tap {tapDir} while holding a ranged weapon to swap ammo types");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			
		}

		public override void UpdateAccessory(Player player, bool hideVisual) 
			=> player.GetSpiritPlayer().assassinMag = true;
	}
}
