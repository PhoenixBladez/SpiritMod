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
			Item.width = 44;
			Item.height = 40;
			Item.value = Item.buyPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
	}
}
