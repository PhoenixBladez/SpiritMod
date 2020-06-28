using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.Neck)]
	public class SepulchrePendant : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Necrotic Pendant");
			Tooltip.SetDefault("Getting hit occasionally sets nearby enemies ablaze with cursed flame");

		}


		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 22;
			item.value = Item.buyPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Blue;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().sepulchreCharm = true;
		}
	}
}
