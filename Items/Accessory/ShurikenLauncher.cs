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
			Tooltip.SetDefault("After every 7 strikes with a projectile, your next attack will do more damage and fly faster");
		}


		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 38;
			item.value = Item.buyPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.Green;

			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().throwerGlove = true;
		}
	}
}
