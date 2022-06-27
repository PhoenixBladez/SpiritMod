using Terraria;
using Terraria.ID;
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
			Item.width = 24;
			Item.height = 24;
			Item.value = Item.buyPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;

			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().magnifyingGlass = true;
			player.GetCritChance(DamageClass.Generic) += 4;
		}
	}
}
