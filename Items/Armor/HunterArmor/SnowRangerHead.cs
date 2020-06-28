using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.HunterArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class SnowRangerHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fur Helmet");
		}
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = ItemRarityID.Green;

			item.vanity = true;
		}
	}
}
