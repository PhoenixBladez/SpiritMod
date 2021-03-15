using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class WintryCharmMage : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wintry Charm");
			Tooltip.SetDefault("Attacks may slow down hit enemies\nThis effect does not apply to bosses");

		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Blue;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().winterbornCharmMage = true;
		}
	}
}
