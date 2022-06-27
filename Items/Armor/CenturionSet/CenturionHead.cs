using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CenturionSet
{
	[AutoloadEquip(EquipType.Head)]
	public class CenturionHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Centurion's Helmet");
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Green;

			Item.vanity = true;
		}
    }
}
