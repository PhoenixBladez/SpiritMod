using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.DiverSet
{
	[AutoloadEquip(EquipType.Head)]
	public class DiverHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Diver's Helmet");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 30, 0);
			item.rare = ItemRarityID.Green;

			item.vanity = true;
		}
    }
}
