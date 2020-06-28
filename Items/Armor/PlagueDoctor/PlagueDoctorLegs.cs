using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Armor.PlagueDoctor
{
	[AutoloadEquip(EquipType.Legs)]
	public class PlagueDoctorLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plague Doctor's Greaves");

		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.value = Terraria.Item.sellPrice(0, 0, 14, 0);
			item.rare = ItemRarityID.Green;
			item.vanity = true;
		}
	}
}
