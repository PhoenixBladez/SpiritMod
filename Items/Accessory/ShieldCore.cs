using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	//[AutoloadEquip(EquipType.Shield)]
	public class ShieldCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Interstellar Shield Core");
			Tooltip.SetDefault("You are surrounded by reflective shields");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 28;
			item.rare = ItemRarityID.Blue;
			item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().ShieldCore = true;
		}
	}
}
