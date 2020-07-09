using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class ShurikenLauncher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sharpshooter's Glove");
			Tooltip.SetDefault("Every 7th ranged hit grants the next shot increased velocity and 25% more damage");
		}

		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 38;
			item.value = Item.buyPrice(gold: 4);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().throwerGlove = true;
		}
	}
}
