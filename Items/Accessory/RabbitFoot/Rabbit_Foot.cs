using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SpiritMod.Items.Accessory.RabbitFoot
{
	public class Rabbit_Foot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rabbit's Foot");
			Tooltip.SetDefault("You have 1% critical strike chance");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.sellPrice(copper: 1);
			item.rare = 1;
			item.accessory = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.magicCrit = 1;
			player.meleeCrit = 1;
			player.rangedCrit = 1;
			
		}
		
		public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> tooltips) 
		{
			{
				TooltipLine b = new TooltipLine(mod, "Rabbit_Foot", "'This must be Lucky!'");
				tooltips.Insert(2, b);
			}
		}
	}
}