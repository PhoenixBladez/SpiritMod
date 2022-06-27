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
			Item.width = 40;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Green;

			Item.vanity = true;
		}
	}
}
