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
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(0, 2, 0, 0);
			item.rare = 2;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().mimicRepellent = true;
		}
	}
}
