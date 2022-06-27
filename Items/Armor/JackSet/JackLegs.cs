using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.JackSet
{
	[AutoloadEquip(EquipType.Legs)]
	public class JackLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Handsome Jack's Pants");
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.LightPurple;

			Item.vanity = true;
		}
    }
}
