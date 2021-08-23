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
			item.width = 30;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.LightPurple;

			item.vanity = true;
		}
    }
}
