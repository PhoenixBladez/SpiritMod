using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class ShadowSingeFang : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow-Singed Fang");
			Tooltip.SetDefault("4% increased critical strike chance\n4% increased critical strike chance at night or underground\nAttacking enemies that have less than half health may strike them with shadows, at the cost of life");
		}


		public override void SetDefaults()
		{
			item.width = 48;
			item.height = 49;
			item.value = Item.sellPrice(0, 0, 76, 0);
			item.rare = ItemRarityID.Green;

			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().shadowFang = true;
			player.magicCrit += 4;
			player.meleeCrit += 4;
			player.rangedCrit += 4;
			if (player.ZoneRockLayerHeight || player.ZoneUnderworldHeight || !Main.dayTime) {
				player.magicCrit += 4;
				player.meleeCrit += 4;
				player.rangedCrit += 4;
			}
		}
	}
}
