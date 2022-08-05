using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.AstronautVanity
{
	[AutoloadEquip(EquipType.Head)]
	public class AstronautHelm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astronaut Helmet");

		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Terraria.Item.sellPrice(0, 0, 25, 0);
			Item.rare = ItemRarityID.Green;
			Item.vanity = true;
		}
	}
}
