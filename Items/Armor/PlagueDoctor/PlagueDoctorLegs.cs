using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.PlagueDoctor
{
	[AutoloadEquip(EquipType.Legs)]
	public class PlagueDoctorLegs : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Plague Doctor's Greaves");

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.value = Terraria.Item.sellPrice(0, 0, 14, 0);
			Item.rare = ItemRarityID.Green;
			Item.vanity = true;
		}
	}
}
