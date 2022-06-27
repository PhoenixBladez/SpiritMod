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
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().winterbornCharmMage = true;
		}
	}
}
