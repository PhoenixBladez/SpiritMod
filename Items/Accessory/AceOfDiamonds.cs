using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class AceOfDiamonds : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ace of Diamonds");
			Tooltip.SetDefault("Critical hits create diamond shields that boost your defense");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().AceOfDiamonds = true;
		}

	}
}
