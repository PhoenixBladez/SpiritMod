using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.Waist)]
	public class FrostGiantBelt : AccessoryItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Giant Belt");
			Tooltip.SetDefault("50% knockback resist when charging a club\nClub damage is increased proportionally to knockback done");
		}

		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 40;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
	}
}
