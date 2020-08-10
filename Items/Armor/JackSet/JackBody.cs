using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.JackSet
{
	[AutoloadEquip(EquipType.Body)]
	public class JackBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Handsome Jack's Suit");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.sellPrice(0, 1, 20, 0);
			item.rare = 6;

			item.vanity = true;
		}
    }
}
