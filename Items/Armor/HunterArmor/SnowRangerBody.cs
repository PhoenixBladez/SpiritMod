using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.HunterArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class SnowRangerBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fur Coverings");
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
