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
			Item.width = 30;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().ShieldCore = true;
		}
	}
}
