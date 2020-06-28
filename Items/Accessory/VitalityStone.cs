using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class VitalityStone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vitality Stone");
			Tooltip.SetDefault("Hearts are more likely to drop from enemies\n'The night is dark and full of terrors'");
		}


		public override void SetDefaults()
		{
			item.width = 48;
			item.height = 49;
			item.value = Item.sellPrice(0, 0, 56, 0);
			item.rare = ItemRarityID.Green;

			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().vitaStone = true;
		}
	}
}
