using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.FreemanSet
{
	[AutoloadEquip(EquipType.Legs)]
	public class FreemanLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Freeman's Pants");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;

			item.vanity = true;
		}
    }
}
