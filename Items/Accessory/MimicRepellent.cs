using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class MimicRepellent : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mimic Repellent");
			Tooltip.SetDefault("Prevents Crate Mimics from being fished up\n'Keep those tentacled freaks at bay!'");

		}


		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().mimicRepellent = true;
		}
	}
}
