using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.AceCardsSet
{
	public class AceOfDiamonds : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ace of Diamonds");
			Tooltip.SetDefault("Enemies killed by critical hits drop Diamond Aces\nDiamond aces give you 15% increased damage for 3 seconds upon collecting");
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
