using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.OperativeSet
{
	[AutoloadEquip(EquipType.Legs)]
	public class OperativeLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Operative's Speedpants");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = 4;

			item.vanity = true;
		}
    }
}
