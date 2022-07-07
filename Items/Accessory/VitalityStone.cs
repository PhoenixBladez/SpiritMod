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
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 56, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetSpiritPlayer().vitaStone = true;
	}
}
