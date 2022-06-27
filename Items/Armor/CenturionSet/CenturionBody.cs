using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CenturionSet
{
	[AutoloadEquip(EquipType.Body)]
	public class CenturionBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Centurion's Platemail");
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
