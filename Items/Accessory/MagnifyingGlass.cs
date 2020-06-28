
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class MagnifyingGlass : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magnifying Glass");
			Tooltip.SetDefault("4% increased critical strike chance\nRight click to zoom out when not holding a weapon");
		}


		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.buyPrice(0, 0, 20, 0);
			item.rare = 1;

			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().magnifyingGlass = true;
			player.magicCrit += 4;
			player.meleeCrit += 4;
			player.rangedCrit += 4;
		}
	}
}
