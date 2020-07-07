using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CenturionSet
{
	[AutoloadEquip(EquipType.Legs)]
	public class CenturionLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Centurion's Greaves");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 30, 0);
			item.rare = ItemRarityID.Green;

			item.vanity = true;
		}
    }
}
