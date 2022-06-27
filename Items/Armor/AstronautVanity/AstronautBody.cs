using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Armor.AstronautVanity
{
	[AutoloadEquip(EquipType.Body)]
	public class AstronautBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astronaut Suit");

		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Terraria.Item.sellPrice(0, 0, 18, 0);
			Item.rare = ItemRarityID.Green;
			Item.vanity = true;
		}
	}
}
