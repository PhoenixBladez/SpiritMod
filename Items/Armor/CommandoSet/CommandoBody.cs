using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CommandoSet
{
	[AutoloadEquip(EquipType.Body)]
	public class CommandoBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Commando's Suit");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = 2;

			item.vanity = true;
		}
    }
}
