using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class BloodWard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sanguine Ward");
			Tooltip.SetDefault("Enemies around you are inflicted with Blood Corruption\nKilling enemies inside your blood aura heals some life");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().Ward = true;
		}

	}
}
