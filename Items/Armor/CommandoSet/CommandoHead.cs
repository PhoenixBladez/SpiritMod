using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CommandoSet
{
	[AutoloadEquip(EquipType.Head)]
	public class CommandoHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Commando's Visor");
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.rare = ItemRarityID.Green;

			Item.vanity = true;
		}
    }
}
