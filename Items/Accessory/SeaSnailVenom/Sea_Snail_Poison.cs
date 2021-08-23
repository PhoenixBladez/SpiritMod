using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.SeaSnailVenom
{
	public class Sea_Snail_Poison : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sea Snail Venom");
			Tooltip.SetDefault("You leave a trail of mucus that envenoms enemies");
		}
		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 42;
			item.value = Item.sellPrice(gold: 1, silver: 20);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetSpiritPlayer().seaSnailVenom = true;
		}
	}
}